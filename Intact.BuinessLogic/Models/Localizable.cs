namespace Intact.BusinessLogic.Models;

public record Localizable
{
    public IReadOnlyDictionary<string, string> Name { get; set; }
    public IReadOnlyDictionary<string, string> Description { get; set; }
}