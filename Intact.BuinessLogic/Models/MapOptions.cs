using System.Text.Json.Serialization;

namespace Intact.BusinessLogic.Models;

public record MapOptions: Localizable
{
    public int Version { get; init; }
    public Scene Scene { get; set; }
    public int Width { get; init; }
    public int Height { get; init; }
}