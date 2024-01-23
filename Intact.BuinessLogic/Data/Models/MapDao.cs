using System.ComponentModel.DataAnnotations;

namespace Intact.BusinessLogic.Data.Models;

public record MapDao : LocalizableDao
{
    [Key]
    [StringLength(16)]
    public string Id { get; init; }
    [Key]
    public int Version { get; init; }
    [StringLength(32)]
    public int Width { get; init; }
    public int Height { get; init; }
    [StringLength(128)]
    public string Factions { get; init; }
    public string SceneBackground { get; init;}
}