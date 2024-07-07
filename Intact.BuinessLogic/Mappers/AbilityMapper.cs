using Intact.BusinessLogic.Data.Models;
using Intact.BusinessLogic.Helpers;
using Intact.BusinessLogic.Models;
using Riok.Mapperly.Abstractions;

namespace Intact.BusinessLogic.Mappers;

[Mapper]
public partial class AbilityMapper
{
    public partial AbilityDto Map(AbilityDao dao);

    public static IReadOnlyList<AbilityDto> Map(IReadOnlyList<AbilityDao> daos,
        IReadOnlyList<IGrouping<string, LocalizationDao>> localizations)
    {
        var mapper = new AbilityMapper();
        return daos.Select(d =>
        {
            var model = mapper.Map(d);
            model.Name = localizations.Map(d.TermName, model.Id);
            model.Description = localizations.Map(d.TermDescription, model.Id);
            return model;
        }).ToList();
    }
}