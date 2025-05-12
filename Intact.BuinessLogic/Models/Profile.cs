namespace Intact.BusinessLogic.Models;

public class Profile
{
    public string Name { get; init; }

    public DateTime CreateTime { get; set; }
    
    public DateTime LastPlayed { get; set; }

    public int Rating { get; set; }
    public string Status { get; init; }
}