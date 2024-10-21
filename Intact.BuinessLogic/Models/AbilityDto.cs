namespace Intact.BusinessLogic.Models;

// Dto suffix added because class cannot be named same as property
public record AbilityDto: Localizable
{
    public int InitialPoints { get; set; }
    public string Ability { get; init; }
    public string AssetId { get; init; }
}