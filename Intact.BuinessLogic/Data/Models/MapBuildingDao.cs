using System.ComponentModel.DataAnnotations;

namespace Intact.BusinessLogic.Data.Models;

public record MapBuildingDao
{
    [Key]
    [StringLength(16)]
    public string MapId { get; init; }
    [Key] 
    public int Number { get; init; }
    [StringLength(16)]
    public string Proto { get; init; }
    public int X { get; init; }
    public int Y { get; init; }
}