using Intact.BusinessLogic.Data.Models;
using Intact.BusinessLogic.Helpers;
using Intact.BusinessLogic.Models;
using Riok.Mapperly.Abstractions;

namespace Intact.BusinessLogic.Mappers;

[Mapper]
public partial class ProtoWarriorMapper
{
    public partial ProtoWarrior Map(ProtoWarriorDao dao);

    public static IReadOnlyList<ProtoWarrior> Map(IReadOnlyList<ProtoWarriorDao> daos,
        IReadOnlyList<IGrouping<string, LocalizationDao>> localizations, 
        List<AbilityDao> abilities, List<SpellDao> spells, List<TraitDao> traits)
    {
        var abilityNamesSet = abilities.Select(x => x.Id).ToHashSet();
        var spellNamesSet = spells.Select(x => x.Id).ToHashSet();
        var traitNamesSet = traits.Select(x => x.Id).ToHashSet();
        var mapper = new ProtoWarriorMapper();
        return daos.Select(d =>
        {
            var model = mapper.Map(d);
            model.SetupLocalization(d, localizations);
            model.Abilities = RefsFromString(d.Abilities, abilityNamesSet);
            model.Spells = RefsFromString(d.Spells, spellNamesSet);
            model.Traits = RefsFromString(d.Traits, traitNamesSet);
            return model;
        }).OrderBy(x => x.Number).ToList();
    }

    private static List<string>? RefsFromString(string value, HashSet<string> namesSet)
    {
        return string.IsNullOrWhiteSpace(value)
            ? null
            : value.Split(',').Where(namesSet.Contains).ToList();
    }
}