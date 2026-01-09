using Intact.BusinessLogic.Data.Models;
using Intact.BusinessLogic.Models;
using Riok.Mapperly.Abstractions;

namespace Intact.BusinessLogic.Mappers;

[Mapper]
public partial class CommandMapper
{
    public partial Command Map(CommandDao dao);

    public static IReadOnlyList<Command> Map(IReadOnlyList<CommandDao> daos)
    {
        var mapper = new CommandMapper();
        return daos.Select(mapper.Map).ToList();
    }
}
