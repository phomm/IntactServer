namespace Intact.BusinessLogic.Models;

public record ProtoBase
{
    public IReadOnlyCollection<Faction> Factions { get; set; }
    public IReadOnlyCollection<ProtoBuilding> ProtoBuildings { get; set; }
    public IReadOnlyCollection<ProtoWarrior> ProtoWarriors { get; set; }

    // TODO temporary, to be removed after testing
    public bool FromCache { get; set; }
}