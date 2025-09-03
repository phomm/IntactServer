using Microsoft.AspNetCore.Identity;

namespace Intact.BusinessLogic.Models;

public class User : IdentityUser
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastLoginAt { get; set; }
}