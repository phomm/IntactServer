namespace Intact.BusinessLogic.Models;

public class ProtoAbility
{
    public int Number { get; set; }
    public string Id { get; init; }
    public int InitialPoints { get; set; }
    public string Ability { get; init; }
    public string AssetId { get; init; }
    public IReadOnlyDictionary<string, string> Name { get; set; }
    public IReadOnlyDictionary<string, string> Description { get; set; }
}