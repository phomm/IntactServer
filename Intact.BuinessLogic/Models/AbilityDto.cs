namespace Intact.BusinessLogic.Models;

// Dto suffix added because class cannot be named same as property
public record AbilityDto
{
    public int Number { get; set; }
    public string Id { get; init; }
    public int InitialPoints { get; set; }
    public string Ability { get; init; }
    public string AssetId { get; init; }
    public IReadOnlyDictionary<string, string> Name { get; set; }
    public IReadOnlyDictionary<string, string> Description { get; set; }
}