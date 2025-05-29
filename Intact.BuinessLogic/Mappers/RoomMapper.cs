using Intact.BusinessLogic.Data.Models;
using Intact.BusinessLogic.Models;
using Riok.Mapperly.Abstractions;

namespace Intact.BusinessLogic.Mappers;

[Mapper]
public partial class RoomMapper
{
    public partial Room Map(RoomDao dao);

    public static IReadOnlyList<Room> Map(IReadOnlyList<RoomDao> daos)
    {
        var mapper = new RoomMapper();
        return daos.Select(mapper.Map).ToList();
    }
}