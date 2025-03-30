namespace Intact.BusinessLogic.Models;

public record ProtoBase
{
    public IReadOnlyCollection<Faction> Factions { get; set; }
    public IReadOnlyCollection<ProtoBuilding> ProtoBuildings { get; set; }
    public IReadOnlyCollection<ProtoWarrior> ProtoWarriors { get; set; }
    public IReadOnlyCollection<AbilityDto> Abilities { get; set; }
    public IReadOnlyCollection<Spell> Spells { get; set; }
    public IReadOnlyCollection<Trait> Traits { get; set; }

    // TODO temporary, to be removed after testing
    public bool FromCache { get; set; }
}