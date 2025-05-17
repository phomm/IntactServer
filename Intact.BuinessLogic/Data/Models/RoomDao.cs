using System.ComponentModel.DataAnnotations;
using Intact.BusinessLogic.Data.Enums;

namespace Intact.BusinessLogic.Data.Models;

public class RoomDao
{
    [Key]
    public int Id { get; init; }
    
    [StringLength(32)]
    public string Title { get; init; }
    
    public int Creator { get; set; }
    
    public DateTime CreateDate { get; set; }

    public RoomState State { get; set; }
}