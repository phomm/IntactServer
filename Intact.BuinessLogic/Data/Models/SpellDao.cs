using System.ComponentModel.DataAnnotations;
using Intact.BusinessLogic.Data.Enums;

namespace Intact.BusinessLogic.Data.Models;

public record SpellDao : LocalizableDao
{
    [Key]
    [StringLength(16)]
    public string Id { get; init; }

    [StringLength(16)]
    public string Mechanics  { get; set; }

    public SpellTarget Target { get; set; }

    public SpellUse Use { get; set; }

    public SpellKind Kind { get; set; }

    public int Cost { get; set; }

    public int Range { get; set; }
    
    [StringLength(32)]
    public string AssetId { get; init; }
}