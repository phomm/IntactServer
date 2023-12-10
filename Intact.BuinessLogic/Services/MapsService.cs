using Intact.BusinessLogic.Data;
using Intact.BusinessLogic.Data.Models;
using Intact.BusinessLogic.Data.RedisCache;
using Intact.BusinessLogic.Mappers;
using Intact.BusinessLogic.Models;
using Microsoft.EntityFrameworkCore;

namespace Intact.BusinessLogic.Services
{
    public interface IMapsService
    {
        Task<IEnumerable<Map>> GetMapsAsync(IEnumerable<string> languages, CancellationToken cancellationToken);
    }

    public class MapsService : IMapsService
    {
        private readonly IDbContextFactory<AppDbContext> _dbContextFactory;
        private readonly IRedisCache _redisCache;

        public MapsService(IDbContextFactory<AppDbContext> dbContextFactory, IRedisCache redisCache)
        {
            _dbContextFactory = dbContextFactory;
            _redisCache = redisCache;
        }

        public async Task<IEnumerable<Map>> GetMapsAsync(IEnumerable<string> languages, CancellationToken cancellationToken)
        {
            const string cacheSet = nameof(Map);
            const string key = nameof(Map);
            var mapsResult = await _redisCache.GetAsync<IEnumerable<Map>>(cacheSet, key);
            if (mapsResult is not null)
            {
                return FilterByLanguages(mapsResult, languages);
            }

            List<LocalizationDao> localizations = null!;
            List<MapDao> maps = null!;
            List<MapBuildingDao> mapBuildings = null!;
            List<PlayerOptionsDao> playerOptions = null!;

            await Task.WhenAll(
                Task.Run(async () =>
                    {
                        await using var context = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
                        localizations = await context.Localizations.AsNoTracking().ToListAsync(cancellationToken);
                    }, cancellationToken),
                Task.Run(async () =>
                    {
                        await using var context = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
                        maps = await context.Maps.AsNoTracking().ToListAsync(cancellationToken);
                    }, cancellationToken),
                Task.Run(async () =>
                    {
                        await using var context = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
                        mapBuildings = await context.MapBuildings.AsNoTracking().ToListAsync(cancellationToken);
                    }, cancellationToken), 
                Task.Run(async () =>
                    {
                        await using var context = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
                        playerOptions = await context.PlayerOptions.AsNoTracking().ToListAsync(cancellationToken);
                    }, cancellationToken)
                );

            var localizationGroups = localizations.GroupBy(x => x.TermId).ToList();

            mapsResult = MapMapper.Map(maps, localizationGroups, MapBuildingMapper.Map(mapBuildings),
                PlayerOptionsMapper.Map(playerOptions));

            await _redisCache.AddAsync(cacheSet, key, mapsResult);

            return FilterByLanguages(mapsResult, languages);
        }

        private static IEnumerable<Map> FilterByLanguages(IEnumerable<Map> full, IEnumerable<string> languages)
        {
            // TODO
            return full;
        }

    }
}
