using System.ComponentModel.DataAnnotations;

namespace Intact.BusinessLogic.Data.Models;

public record MapBuildingDao
{
    [Key]
    [StringLength(16)]
    public string MapId { get; set; }
    [Key] 
    public int Number { get; set; }
    [StringLength(16)]
    public string Proto { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
}