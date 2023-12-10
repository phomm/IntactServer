namespace Intact.BusinessLogic.Data.Config;

public record DbSettings
{
    public string ConnectionString { get; init; }
    public string PgConnectionString { get; init; }
}