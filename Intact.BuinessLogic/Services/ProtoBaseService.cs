using Intact.BusinessLogic.Data;
using Intact.BusinessLogic.Data.Models;
using Intact.BusinessLogic.Mappers;
using Intact.BusinessLogic.Models;
using Microsoft.EntityFrameworkCore;

namespace Intact.BusinessLogic.Services
{
    public interface IProtoBaseService
    {
        Task<ProtoBase> GetProtoBaseAsync(CancellationToken cancellationToken);
    }

    public class ProtoBaseService : IProtoBaseService
    {
        private readonly IDbContextFactory<IntactDbContext> _dbContextFactory;

        public ProtoBaseService(IDbContextFactory<IntactDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public async Task<ProtoBase> GetProtoBaseAsync(CancellationToken cancellationToken)
        {
            List<LocalizationDao> localizations = null!;
            List<FactionDao> factions = null!;
            List<ProtoBuildingDao> protoBuildings = null!;
            List<ProtoWarriorDao> protoWarriors = null!;

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
                    }, cancellationToken)
            );

            var localizationGroups = localizations.GroupBy(x => x.TermId).ToList();

            return new ProtoBase
            {
                Factions = FactionMapper.Map(factions, protoWarriors, localizationGroups),
                ProtoBuildings = ProtoBuildingMapper.Map(protoBuildings, localizationGroups),
                ProtoWarriors = ProtoWarriorMapper.Map(protoWarriors, localizationGroups),
            };

        }
    }
}
