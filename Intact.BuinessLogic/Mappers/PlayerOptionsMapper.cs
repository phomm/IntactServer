using Intact.BusinessLogic.Data.Models;
using Intact.BusinessLogic.Models;
using Riok.Mapperly.Abstractions;

namespace Intact.BusinessLogic.Mappers;

[Mapper]
public partial class PlayerOptionsMapper
{
    public partial PlayerOptions Map(PlayerOptionsDao dao);

    public static IReadOnlyList<PlayerOptions> Map(IReadOnlyList<PlayerOptionsDao> daos)
    {
        var mapper = new PlayerOptionsMapper();
        return daos.Select(d =>
        {
            var model = mapper.Map(d);
            model.StartZone = new Zone {Bottom = d.Bottom, Left = d.Left, Right = d.Right, Top = d.Top};
            return model;
        }).OrderBy(x => x.Number).ToList();
    }
}