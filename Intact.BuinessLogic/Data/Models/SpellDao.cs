using System.ComponentModel.DataAnnotations;
using Intact.BusinessLogic.Data.Enums;

namespace Intact.BusinessLogic.Data.Models;

public record SpellDao : LocalizableDao
{
    [StringLength(16)]
    public string Mechanics  { get; init; }

    public SpellTarget Target { get; init; }

    public SpellUse Use { get; init; }

    public SpellKind Kind { get; init; }

    public int Cost { get; init; }

    public int Range { get; init; }
    
    [StringLength(32)]
    public string AssetId { get; init; }
}