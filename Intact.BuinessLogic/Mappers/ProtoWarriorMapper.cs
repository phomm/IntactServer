using Intact.BusinessLogic.Data.Models;
using Intact.BusinessLogic.Models;
using Riok.Mapperly.Abstractions;

namespace Intact.BusinessLogic.Mappers;

[Mapper]
public partial class ProtoWarriorMapper
{
    public partial ProtoWarrior Map(ProtoWarriorDao dao);

    public static IReadOnlyList<ProtoWarrior> Map(IReadOnlyList<ProtoWarriorDao> daos,
        IReadOnlyList<IGrouping<string, LocalizationDao>> localizations, List<AbilityDao> abilities)
    {
        var abilityNamesSet = abilities.Select(x => x.Id).ToHashSet();
        var mapper = new ProtoWarriorMapper();
        return daos.Select(d =>
        {
            var model = mapper.Map(d);
            model.SetupLocalization(d, localizations);
            model.Abilities = string.IsNullOrWhiteSpace(d.Abilities)
                ? null
                : d.Abilities.Split(',').Where(x => abilityNamesSet.Contains(x)).ToList();
            return model;
        }).OrderBy(x => x.Number).ToList();
    }
}