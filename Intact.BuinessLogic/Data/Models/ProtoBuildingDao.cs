using Intact.BusinessLogic.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace Intact.BusinessLogic.Data.Models;

public record ProtoBuildingDao
{
    [Key]
    [StringLength(16)]
    public string Id { get; set; }
    public int Number { get; set; }
    [StringLength(32)]
    public string TermName { get; set; }
    [StringLength(32)]
    public string TermDescription { get; set; }
    [StringLength(32)]
    public string AssetId { get; set; }
    public byte InLife { get; set; }
    public BuildingType BuildingType { get; set; }
}