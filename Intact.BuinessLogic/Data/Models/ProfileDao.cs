using System.ComponentModel.DataAnnotations;
using Intact.BusinessLogic.Data.Enums;
using Microsoft.EntityFrameworkCore;

namespace Intact.BusinessLogic.Data.Models;

public class ProfileDao
{
    [Key]
    public int Id { get; set; }
    
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