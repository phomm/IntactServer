using Intact.BusinessLogic.Data.Enums;
using System.Text.Json.Serialization;

namespace Intact.BusinessLogic.Models;

public record ProtoBuilding: Localizable
{
    public int Number { get; init; }
    public string AssetId { get; init; }
    public byte InLife { get; init; }
    public BuildingType BuildingType { get; init; }
}