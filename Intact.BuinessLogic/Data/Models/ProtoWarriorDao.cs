﻿using Intact.BusinessLogic.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace Intact.BusinessLogic.Data.Models;

public record ProtoWarriorDao : LocalizableDao
{
    [Key]
    [StringLength(16)]
    public string Id { get; set; }
    public int Number { get; set; }
    [StringLength(16)]
    public string FactionId { get; set; }
    public Force Force { get; set; }
    [StringLength(32)]
    public string AssetId { get; set; }
    public bool IsHero { get; set; }
    public bool IsRanged { get; set; }
    public bool IsMelee { get; set; }
    public bool IsBlockFree { get; set; }
    public bool IsImmune { get; set; }
    public byte InLife { get; set; }
    public byte InMana { get; set; }
    public byte InMoves { get; set; }
    public byte InActs { get; set; }
    public byte InShots { get; set; }
    public byte Cost { get; set; }
}