using Intact.BusinessLogic.Data.Models;
using Intact.BusinessLogic.Models;

namespace Intact.BusinessLogic.Mappers;

public class CommandMapper
{
    public static Command Map(CommandDao dao)
    {
        return new Command
        {
            PlayerIndex = dao.PlayerIndex,
            CommandId = dao.CommandId,
            QueueNumber = dao.QueueNumber,
            Value = dao.Value,
            Error = dao.Error
        };
    }
    
    public static IEnumerable<Command> Map(IEnumerable<CommandDao> daos)
    {
        return daos.Select(Map);
    }
    
    public static CommandDao Map(BaseCommand baseCommand, int roomId, int profileId, uint queueNumber)
    {
        return new CommandDao
        {
            RoomId = roomId,
            ProfileId = profileId,
            PlayerIndex = baseCommand.PlayerIndex,
            CommandId = baseCommand.CommandId,
            QueueNumber = queueNumber,
            Value = baseCommand.Value
        };
    }
}
