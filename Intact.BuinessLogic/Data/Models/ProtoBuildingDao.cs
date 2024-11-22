using Intact.BusinessLogic.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace Intact.BusinessLogic.Data.Models;

public record ProtoBuildingDao : LocalizableDao
{
    public int Number { get; init; }
    [StringLength(32)]
    public string AssetId { get; init; }
    public byte InLife { get; init; }
    public BuildingType BuildingType { get; init; }
}