using System.ComponentModel.DataAnnotations;

namespace Intact.BusinessLogic.Data.Models;

public class RoomMemberDao
{
    [Key]
    public int Id { get; init; }
    public int RoomId { get; init; }

    public int ProfileId { get; init; }
}