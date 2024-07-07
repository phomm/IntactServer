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
        IReadOnlyList<IGrouping<string, LocalizationDao>> localizations, List<AbilityDao> abilities)
    {
        var abilityNamesSet = abilities.Select(x => x.Id).ToHashSet();
        var mapper = new ProtoWarriorMapper();
        return daos.Select(d =>
        {
            var model = mapper.Map(d);
            model.Name = localizations.Map(d.TermName, model.Id);
            model.Description = localizations.Map(d.TermDescription, model.Id);
            model.Abilities = string.IsNullOrWhiteSpace(d.Abilities)
                ? null
                : d.Abilities.Split(',').ToList();
            var notFoundName = model.Abilities?.FirstOrDefault(x => !abilityNamesSet.Contains(x));
            if (notFoundName is not null)
                throw new IndexOutOfRangeException($"The ability '{notFoundName}' is not found");
            return model;
        }).OrderBy(x => x.Number).ToList();
    }
}