using System.ComponentModel.DataAnnotations;
using Intact.BusinessLogic.Data.Enums;
using Microsoft.EntityFrameworkCore;

namespace Intact.BusinessLogic.Data.Models;

[PrimaryKey(nameof(UserId), nameof(Name))]
public class ProfileDao
{
    public Guid UserId { get; init; }
    
    [StringLength(32)]
    public string Name { get; init; }

    public DateTime CreateTime { get; init; }
    
    public DateTime LastPlayed { get; set; }
    
    public ProfileState State { get; set; }

    public int Rating { get; set; }
    
    [StringLength(32)]
    public string Status { get; set; }
}