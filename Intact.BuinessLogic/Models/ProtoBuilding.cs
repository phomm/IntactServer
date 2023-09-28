﻿#pragma warning disable 8618

using Intact.BusinessLogic.Data.Enums;

namespace Intact.BusinessLogic.Models;

public record ProtoBuilding
{
    public int Number { get; set; }
    public string Id { get; set; }
    public IReadOnlyDictionary<string, string> Name { get; set; }
    public IReadOnlyDictionary<string, string> Description { get; set; }
    public string AssetId { get; set; }
    public byte InLife { get; set; }
    public BuildingType BuildingType { get; set; }
}