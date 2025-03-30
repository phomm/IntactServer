using Intact.BusinessLogic.Data.Models;
using Intact.BusinessLogic.Helpers;
using Intact.BusinessLogic.Models;
using Riok.Mapperly.Abstractions;

namespace Intact.BusinessLogic.Mappers;

[Mapper]
public partial class TraitMapper
{
    public partial Trait Map(TraitDao dao);

    public static IReadOnlyList<Trait> Map(IReadOnlyList<TraitDao> daos,
        IReadOnlyList<IGrouping<string, LocalizationDao>> localizations)
    {
        var mapper = new TraitMapper();
        return daos.Select(d =>
        {
            var model = mapper.Map(d);
            model.SetupLocalization(d, localizations);
            return model;
        }).ToList();
    }
}