﻿namespace Intact.BusinessLogic.Data.Config;

public record DbSettings
{
    public string ConnectionString { get; set; }
    public string PgConnectionString { get; set; }
}