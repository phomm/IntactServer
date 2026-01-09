using Intact.BusinessLogic.Data.Models;
using Intact.BusinessLogic.Models;

namespace Intact.BusinessLogic.Mappers;

public class CommandMapper
{
    public static Command Map(CommandDao dao)
    {
        return new Command
        {
            RoomId = dao.RoomId,
            ProfileId = dao.ProfileId,
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
    
    public static CommandDao Map(PostCommand postCommand, int roomId, int profileId, uint queueNumber)
    {
        return new CommandDao
        {
            RoomId = roomId,
            ProfileId = profileId,
            PlayerIndex = postCommand.PlayerIndex,
            CommandId = postCommand.CommandId,
            QueueNumber = queueNumber,
            Value = postCommand.Value
        };
    }
}
