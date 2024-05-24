using Intact.BusinessLogic.Data.Models;
using Intact.BusinessLogic.Helpers;
using Intact.BusinessLogic.Models;
using Riok.Mapperly.Abstractions;

namespace Intact.BusinessLogic.Mappers;

[Mapper]
public partial class ProtoAbilityMapper
{
    public partial ProtoAbility Map(ProtoAbilityDao dao);

    public static IReadOnlyList<ProtoAbility> Map(IReadOnlyList<ProtoAbilityDao> daos,
        IReadOnlyList<IGrouping<string, LocalizationDao>> localizations)
    {
        var mapper = new ProtoAbilityMapper();
        return daos.Select(d =>
        {
            var model = mapper.Map(d);
            model.Name = localizations.Map(d.TermName, model.Id);
            model.Description = localizations.Map(d.TermDescription, model.Id);
            return model;
        }).OrderBy(x => x.Number).ToList();
    }
}