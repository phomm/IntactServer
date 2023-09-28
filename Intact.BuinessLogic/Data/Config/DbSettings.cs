#pragma warning disable 8618

namespace Intact.BusinessLogic.Data.Config;

public record DbSettings
{
    public string ConnectionString { get; set; }
}