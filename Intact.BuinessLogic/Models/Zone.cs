﻿namespace Intact.BusinessLogic.Models;

public record Zone
{
    public int Left { get; set; }
    public int Right { get; set; }
    public int Top { get; set; }
    public int Bottom { get; set; }
}