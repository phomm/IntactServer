namespace Intact.BusinessLogic.Data.Models;

public class RoomMemberDao
{
    public int RoomId { get; init; }

    public Guid UserId { get; set; }

    public string ProfileName { get; set; }
}