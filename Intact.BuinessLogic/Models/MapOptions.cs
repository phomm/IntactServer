using System.Text.Json.Serialization;

namespace Intact.BusinessLogic.Models;

public record MapOptions: Localizable
{
    [JsonIgnore]
    public string Id { get; set; }
    public int Version { get; set; }
    public Scene Scene { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
}