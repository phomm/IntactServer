using Intact.BusinessLogic.Data.Enums;
using System.Text.Json.Serialization;

namespace Intact.BusinessLogic.Models;

public record ProtoBuilding: Localizable
{
    public int Number { get; set; }
    public string AssetId { get; set; }
    public byte InLife { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public BuildingType BuildingType { get; set; }
}