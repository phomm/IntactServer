using System.ComponentModel.DataAnnotations;

namespace Intact.BusinessLogic.Data.Models;

public record MapDao : LocalizableDao
{
    [Key]
    [StringLength(16)]
    public string Id { get; set; }
    [Key]
    public int Version { get; set; }
    [StringLength(32)]
    public int Width { get; set; }
    public int Height { get; set; }
    [StringLength(128)]
    public string Factions { get; set; }
    public string SceneBackground { get; set;}
}