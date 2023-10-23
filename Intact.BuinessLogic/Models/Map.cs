using System.Text.Json.Serialization;

namespace Intact.BusinessLogic.Models;

public record Map
{
    public MapOptions MapOptions { get; set; }
    //public MapOptionals MapOptionals { get; set; }
    public IReadOnlyList<PlayerOptions> PlayerOptions { get; set; }
    public IReadOnlyList<string> AvailableFactions { get; set; }
    public IReadOnlyList<MapBuilding> MapObjects { get; set; }
}