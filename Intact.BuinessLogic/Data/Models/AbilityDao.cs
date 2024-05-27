using System.ComponentModel.DataAnnotations;

namespace Intact.BusinessLogic.Data.Models;

public record AbilityDao : LocalizableDao
{
    [Key]
    [StringLength(16)]
    public string Id { get; init; }
    public int Number { get; init; }
    public int InitialPoints { get; set; }
    [StringLength(16)]
    public string Ability { get; init; }
    [StringLength(32)]
    public string AssetId { get; init; }
}