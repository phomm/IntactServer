using Intact.BusinessLogic.Data.Models;
using Intact.BusinessLogic.Helpers;
using Intact.BusinessLogic.Models;
using Riok.Mapperly.Abstractions;

namespace Intact.BusinessLogic.Mappers;

[Mapper]
public partial class ProtoBuildingMapper
{
    public partial ProtoBuilding Map(ProtoBuildingDao dao);

    public static IReadOnlyList<ProtoBuilding> Map(IReadOnlyList<ProtoBuildingDao> daos,
        IReadOnlyList<IGrouping<string, LocalizationDao>> localizations)
    {
        var mapper = new ProtoBuildingMapper();
        return daos.Select(d =>
        {
            var model = mapper.Map(d);
            model.SetupLocalization(d, localizations);
            return model;
        }).OrderBy(x => x.Number).ToList();
    }
}