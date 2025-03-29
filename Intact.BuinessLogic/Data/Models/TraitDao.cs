using System.ComponentModel.DataAnnotations;

namespace Intact.BusinessLogic.Data.Models;

public record TraitDao : LocalizableDao
{
    [StringLength(32)]
    public string AssetId { get; init; }
}