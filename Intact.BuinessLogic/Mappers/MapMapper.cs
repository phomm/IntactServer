using Intact.BusinessLogic.Data.Models;
using Intact.BusinessLogic.Helpers;
using Intact.BusinessLogic.Models;
using Riok.Mapperly.Abstractions;

namespace Intact.BusinessLogic.Mappers;

[Mapper]
public partial class MapMapper
{
    public partial MapOptions Map(MapDao dao);

    public static IReadOnlyList<Map> Map(IReadOnlyList<MapDao> daos, IReadOnlyList<IGrouping<string, LocalizationDao>> localizations,
        IReadOnlyList<MapBuilding> mapObjects, IReadOnlyList<PlayerOptions> playerOptions)
    {
        var mapper = new MapMapper();
        return daos.Select(d =>
        {
            var resultMap = new Map();
            var model = mapper.Map(d);
            model.Name = localizations.Map(d.TermName, model.Id);
            model.Description = localizations.Map(d.TermDescription, model.Id);
            model.Scene = new Scene {BackGround = d.SceneBackground};

            resultMap.MapOptions = model;
            resultMap.AvailableFactions = d.Factions.Split(',');
            resultMap.PlayerOptions = playerOptions.Where(x => x.MapId == d.Id).ToList();
            resultMap.MapObjects = mapObjects.Where(x => x.MapId == d.Id).ToList();
            return resultMap;
        }).OrderBy(x => x.MapOptions.Id).ToList();
    }
}