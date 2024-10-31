namespace Intact.BusinessLogic.Models;

public record Zone
{
    public int Left { get; init; }
    public int Right { get; init; }
    public int Top { get; init; }
    public int Bottom { get; init; }
}