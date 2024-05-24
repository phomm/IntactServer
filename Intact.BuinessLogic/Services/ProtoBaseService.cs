using Intact.BusinessLogic.Data;
using Intact.BusinessLogic.Data.Models;
using Intact.BusinessLogic.Data.RedisCache;
using Intact.BusinessLogic.Mappers;
using Intact.BusinessLogic.Models;
using Microsoft.EntityFrameworkCore;

namespace Intact.BusinessLogic.Services
{
    public interface IProtoBaseService
    {
        Task<ProtoBase> GetProtoBaseAsync(IEnumerable<string> languages, CancellationToken cancellationToken);
    }

    public class ProtoBaseService : IProtoBaseService
    {
        private readonly IDbContextFactory<AppDbContext> _dbContextFactory;
        private readonly IRedisCache _redisCache;

        public ProtoBaseService(IDbContextFactory<AppDbContext> dbContextFactory, IRedisCache redisCache)
        {
            _dbContextFactory = dbContextFactory;
            _redisCache = redisCache;
        }

        public async Task<ProtoBase> GetProtoBaseAsync(IEnumerable<string> languages, CancellationToken cancellationToken)
        {
            const string cacheSet = nameof(ProtoBase);
            const string key = nameof(ProtoBase);
            var protoBase = await _redisCache.GetAsync<ProtoBase>(cacheSet, key);
            if (protoBase is not null)
            {
                protoBase.FromCache = true;
                return FilterByLanguages(protoBase, languages);
            }

            List<LocalizationDao> localizations = null!;
            List<FactionDao> factions = null!;
            List<ProtoBuildingDao> protoBuildings = null!;
            List<ProtoWarriorDao> protoWarriors = null!;
            List<ProtoAbilityDao> protoAbilities = null!;

            await Task.WhenAll(
                Task.Run(async () =>
                    {
                        await using var context = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
                        factions = await context.Factions.AsNoTracking().ToListAsync(cancellationToken);
                    }, cancellationToken),
                Task.Run(async () =>
                    {
                        await using var context = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
                        protoBuildings = await context.ProtoBuildings.AsNoTracking().ToListAsync(cancellationToken);
                    }, cancellationToken),
                Task.Run(async () =>
                    {
                        await using var context = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
                        localizations = await context.Localizations.AsNoTracking().ToListAsync(cancellationToken);
                    }, cancellationToken),
                Task.Run(async () =>
                    {
                        await using var context = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
                        protoWarriors = await context.ProtoWarriors.AsNoTracking().ToListAsync(cancellationToken);
                    }, cancellationToken),
                Task.Run(async () =>
                {
                    await using var context = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
                    protoAbilities = await context.ProtoAbilities.AsNoTracking().ToListAsync(cancellationToken);
                }, cancellationToken)
            );

            var localizationGroups = localizations.GroupBy(x => x.TermId).ToList();

            protoBase = new ProtoBase
            {
                Factions = FactionMapper.Map(factions, protoWarriors, localizationGroups),
                ProtoBuildings = ProtoBuildingMapper.Map(protoBuildings, localizationGroups),
                ProtoWarriors = ProtoWarriorMapper.Map(protoWarriors, localizationGroups),
                ProtoAbilities = ProtoAbilityMapper.Map(protoAbilities, localizationGroups),
            };

            await _redisCache.AddAsync(cacheSet, key, protoBase);

            return FilterByLanguages(protoBase, languages);
        }

        private static ProtoBase FilterByLanguages(ProtoBase full, IEnumerable<string> languages)
        {
            // TODO
            return full;
        }
    }
}
