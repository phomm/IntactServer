namespace Intact.BusinessLogic.Data.Config;

public record DbSettings
{
    public bool UseSqlServer { get; init; }
    public string ConnectionString { get; init; }
    public string PgConnectionString { get; init; }
}