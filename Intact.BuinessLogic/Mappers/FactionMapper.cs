using Intact.BusinessLogic.Data.Models;
using Intact.BusinessLogic.Helpers;
using Intact.BusinessLogic.Models;
using Riok.Mapperly.Abstractions;

namespace Intact.BusinessLogic.Mappers;

[Mapper]
public partial class FactionMapper
{
    public partial Faction Map(FactionDao dao);

    public static IReadOnlyList<Faction> Map(IReadOnlyList<FactionDao> factionDaos, IReadOnlyList<ProtoWarriorDao> warriorDaos, IReadOnlyList<IGrouping<string, LocalizationDao>> localizations)
    {
        var mapper = new FactionMapper();
        return factionDaos.Select(d =>
        {
            var model = mapper.Map(d);
            model.Name = localizations.Map(d.TermName, model.Id);
            model.Description = localizations.Map(d.TermDescription, model.Id);
            model.Units = warriorDaos.Where(x => x.FactionId == d.Id).OrderBy(x => x.Number).Select(x => x.Id).ToList();
            return model;
        }).OrderBy(x => x.Number).ToList();
    }
}