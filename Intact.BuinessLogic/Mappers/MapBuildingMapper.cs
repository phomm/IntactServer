using Intact.BusinessLogic.Data.Models;
using Intact.BusinessLogic.Models;
using Riok.Mapperly.Abstractions;

namespace Intact.BusinessLogic.Mappers;

[Mapper]
public partial class MapBuildingMapper
{
    public partial MapBuilding Map(MapBuildingDao dao);

    public static IReadOnlyList<MapBuilding> Map(IReadOnlyList<MapBuildingDao> daos)
    {
        var mapper = new MapBuildingMapper();
        return daos.Select(d =>
        {
            var model = mapper.Map(d);
            return model;
        }).OrderBy(x => x.X).ThenBy(x => x.Y).ToList();
    }
}