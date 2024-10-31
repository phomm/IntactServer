namespace Intact.BusinessLogic.Models;

public record Faction: Localizable
{
    public int Number { get; init; }
    public IReadOnlyList<string> Units { get; set; }
}