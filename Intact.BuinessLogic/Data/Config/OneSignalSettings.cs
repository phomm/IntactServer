namespace Intact.BusinessLogic.Data.Config;

/// <summary>
/// Configuration settings for OneSignal push notification service
/// </summary>
public class OneSignalSettings
{
    /// <summary>
    /// OneSignal App ID
    /// </summary>
    public string AppId { get; set; } = string.Empty;

    /// <summary>
    /// OneSignal REST API Key
    /// </summary>
    public string RestApiKey { get; set; } = string.Empty;

    /// <summary>
    /// OneSignal User Auth Key (optional, for user-level operations)
    /// </summary>
    public string? UserAuthKey { get; set; }

    /// <summary>
    /// Enable or disable OneSignal notifications
    /// </summary>
    public bool Enabled { get; set; } = false;

    /// <summary>
    /// Default notification icon URL
    /// </summary>
    public string? DefaultIconUrl { get; set; }

    /// <summary>
    /// Default notification badge URL
    /// </summary>
    public string? DefaultBadgeUrl { get; set; }

    /// <summary>
    /// Default notification sound
    /// </summary>
    public string? DefaultSound { get; set; }

    /// <summary>
    /// Timeout for API requests in seconds
    /// </summary>
    public int TimeoutSeconds { get; set; } = 30;
}
