#pragma warning disable 8618

namespace Intact.BusinessLogic.Models;

public record Faction
{
    public int Number { get; set; }
    public string Id { get; set; }
    public IReadOnlyDictionary<string, string> Name { get; set; }
    public IReadOnlyDictionary<string, string> Description { get; set; }
    public IReadOnlyList<string> Units { get; set; }
}