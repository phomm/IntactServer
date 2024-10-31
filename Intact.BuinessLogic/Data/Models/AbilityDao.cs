using System.ComponentModel.DataAnnotations;

namespace Intact.BusinessLogic.Data.Models;

public record AbilityDao : LocalizableDao
{
    public int InitialPoints { get; set; }
    [StringLength(16)]
    public string Ability { get; init; }
    [StringLength(32)]
    public string AssetId { get; init; }
}