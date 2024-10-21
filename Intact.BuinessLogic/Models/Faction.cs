namespace Intact.BusinessLogic.Models;

public record Faction: Localizable
{
    public int Number { get; set; }
    public IReadOnlyList<string> Units { get; set; }
    public int TotalCount { get; set; }
    public int UnitCount { get; set; }
    public int HeroCount { get; set; }
}