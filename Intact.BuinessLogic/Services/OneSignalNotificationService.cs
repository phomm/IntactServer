using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Intact.BusinessLogic.Data.Config;
using OneSignalApi.Api;
using OneSignalApi.Client;
using OneSignalApi.Model;

namespace Intact.BusinessLogic.Services;

/// <summary>
/// OneSignal notification service implementation
/// </summary>
public class OneSignalNotificationService : IOneSignalNotificationService
{
    private readonly ILogger<OneSignalNotificationService> _logger;
    private readonly OneSignalSettings _settings;
    private readonly DefaultApi _oneSignalClient;

    public OneSignalNotificationService(
        ILogger<OneSignalNotificationService> logger,
        IOptions<OneSignalSettings> settings)
    {
        _logger = logger;
        _settings = settings.Value;

        // Configure OneSignal API client
        var config = new Configuration
        {
            BasePath = "https://onesignal.com/api/v1",
            Timeout = _settings.TimeoutSeconds * 1000
        };

        // Set REST API Key for authentication
        config.AccessToken = _settings.RestApiKey;

        _oneSignalClient = new DefaultApi(config);
    }

    public async Task<bool> SendToUserAsync(string userId, string title, string message, Dictionary<string, object>? data = null, CancellationToken cancellationToken = default)
    {
        if (!_settings.Enabled)
        {
            _logger.LogWarning("OneSignal is disabled. Notification not sent to user {UserId}", userId);
            return false;
        }

        try
        {
            var notification = BuildNotification(title, message, data);
            notification.IncludeExternalUserIds = new List<string> { userId };

            _logger.LogInformation("Sending OneSignal notification to user {UserId}: {Title}", userId, title);
            var response = await _oneSignalClient.CreateNotificationAsync(_settings.AppId, notification);

            _logger.LogInformation("OneSignal notification sent successfully. ID: {NotificationId}, Recipients: {Recipients}", 
                response.Id, response.Recipients);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send OneSignal notification to user {UserId}", userId);
            return false;
        }
    }

    public async Task<bool> SendToUsersAsync(IEnumerable<string> userIds, string title, string message, Dictionary<string, object>? data = null, CancellationToken cancellationToken = default)
    {
        if (!_settings.Enabled)
        {
            _logger.LogWarning("OneSignal is disabled. Notification not sent to users");
            return false;
        }

        var userIdList = userIds.ToList();
        if (!userIdList.Any())
        {
            _logger.LogWarning("No user IDs provided for notification");
            return false;
        }

        try
        {
            var notification = BuildNotification(title, message, data);
            notification.IncludeExternalUserIds = userIdList;

            _logger.LogInformation("Sending OneSignal notification to {Count} users: {Title}", userIdList.Count, title);
            var response = await _oneSignalClient.CreateNotificationAsync(_settings.AppId, notification);

            _logger.LogInformation("OneSignal notification sent successfully. ID: {NotificationId}, Recipients: {Recipients}", 
                response.Id, response.Recipients);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send OneSignal notification to users");
            return false;
        }
    }

    public async Task<bool> SendToSegmentAsync(Dictionary<string, string> tags, string title, string message, Dictionary<string, object>? data = null, CancellationToken cancellationToken = default)
    {
        if (!_settings.Enabled)
        {
            _logger.LogWarning("OneSignal is disabled. Notification not sent to segment");
            return false;
        }

        try
        {
            var notification = BuildNotification(title, message, data);
            
            // Build tag filters
            var filters = new List<object>();
            var isFirst = true;
            foreach (var tag in tags)
            {
                if (!isFirst)
                {
                    filters.Add(new { field = "tag", key = "operator", relation = "AND" });
                }
                filters.Add(new { field = "tag", key = tag.Key, relation = "=", value = tag.Value });
                isFirst = false;
            }
            
            notification.Filters = filters;

            _logger.LogInformation("Sending OneSignal notification to segment with {Count} tags: {Title}", tags.Count, title);
            var response = await _oneSignalClient.CreateNotificationAsync(_settings.AppId, notification);

            _logger.LogInformation("OneSignal notification sent successfully. ID: {NotificationId}, Recipients: {Recipients}", 
                response.Id, response.Recipients);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send OneSignal notification to segment");
            return false;
        }
    }

    public async Task<bool> SendBroadcastAsync(string title, string message, Dictionary<string, object>? data = null, CancellationToken cancellationToken = default)
    {
        if (!_settings.Enabled)
        {
            _logger.LogWarning("OneSignal is disabled. Broadcast notification not sent");
            return false;
        }

        try
        {
            var notification = BuildNotification(title, message, data);
            notification.IncludedSegments = new List<string> { "All" };

            _logger.LogInformation("Sending OneSignal broadcast notification: {Title}", title);
            var response = await _oneSignalClient.CreateNotificationAsync(_settings.AppId, notification);

            _logger.LogInformation("OneSignal broadcast notification sent successfully. ID: {NotificationId}, Recipients: {Recipients}", 
                response.Id, response.Recipients);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send OneSignal broadcast notification");
            return false;
        }
    }

    public async Task<bool> SendEmailNotificationAsync(string userId, string subject, string htmlBody, CancellationToken cancellationToken = default)
    {
        if (!_settings.Enabled)
        {
            _logger.LogWarning("OneSignal is disabled. Email notification not sent to user {UserId}", userId);
            return false;
        }

        try
        {
            // Send as a rich notification with email-like content
            var data = new Dictionary<string, object>
            {
                { "type", "email" },
                { "subject", subject },
                { "body", htmlBody }
            };

            var notification = BuildNotification(subject, ExtractTextFromHtml(htmlBody), data);
            notification.IncludeExternalUserIds = new List<string> { userId };

            _logger.LogInformation("Sending OneSignal email-style notification to user {UserId}: {Subject}", userId, subject);
            var response = await _oneSignalClient.CreateNotificationAsync(_settings.AppId, notification);

            _logger.LogInformation("OneSignal email notification sent successfully. ID: {NotificationId}, Recipients: {Recipients}", 
                response.Id, response.Recipients);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send OneSignal email notification to user {UserId}", userId);
            return false;
        }
    }

    public async Task<bool> RegisterUserDeviceAsync(string userId, string deviceToken, string deviceType = "Android", CancellationToken cancellationToken = default)
    {
        if (!_settings.Enabled)
        {
            _logger.LogWarning("OneSignal is disabled. Device registration skipped for user {UserId}", userId);
            return false;
        }

        try
        {
            var player = new Player
            {
                AppId = _settings.AppId,
                ExternalUserId = userId,
                DeviceType = GetDeviceType(deviceType),
                Identifier = deviceToken
            };

            _logger.LogInformation("Registering device for user {UserId} with OneSignal", userId);
            var response = await _oneSignalClient.CreatePlayerAsync(_settings.AppId, player);

            _logger.LogInformation("Device registered successfully for user {UserId}. Player ID: {PlayerId}", userId, response.Id);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to register device for user {UserId}", userId);
            return false;
        }
    }

    public async Task<bool> UpdateUserTagsAsync(string userId, Dictionary<string, string> tags, CancellationToken cancellationToken = default)
    {
        if (!_settings.Enabled)
        {
            _logger.LogWarning("OneSignal is disabled. Tags not updated for user {UserId}", userId);
            return false;
        }

        try
        {
            // Note: To update tags, we need the player ID. In a real implementation,
            // you would need to store the mapping between external user ID and player ID
            _logger.LogInformation("Updating tags for user {UserId}: {Tags}", userId, string.Join(", ", tags.Keys));
            
            // This is a placeholder - actual implementation would require player ID lookup
            _logger.LogWarning("Tag update requires player ID. Store player ID when registering devices.");

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update tags for user {UserId}", userId);
            return false;
        }
    }

    private Notification BuildNotification(string title, string message, Dictionary<string, object>? data = null)
    {
        var notification = new Notification
        {
            AppId = _settings.AppId,
            Headings = new StringMap { En = title },
            Contents = new StringMap { En = message }
        };

        // Add optional settings from configuration
        if (!string.IsNullOrEmpty(_settings.DefaultIconUrl))
        {
            notification.SmallIcon = _settings.DefaultIconUrl;
        }

        if (!string.IsNullOrEmpty(_settings.DefaultBadgeUrl))
        {
            notification.LargeIcon = _settings.DefaultBadgeUrl;
        }

        if (!string.IsNullOrEmpty(_settings.DefaultSound))
        {
            notification.AndroidSound = _settings.DefaultSound;
            notification.IosSound = _settings.DefaultSound;
        }

        // Add custom data
        if (data != null && data.Any())
        {
            notification.Data = data;
        }

        return notification;
    }

    private int GetDeviceType(string deviceType)
    {
        return deviceType.ToLowerInvariant() switch
        {
            "ios" => 0,
            "android" => 1,
            "amazon" => 2,
            "windowsphone" => 3,
            "chrome" => 4,
            "chromeapp" => 5,
            "firefox" => 6,
            "safari" => 7,
            "macos" => 9,
            "alexa" => 10,
            "email" => 11,
            "huawei" => 13,
            _ => 1 // Default to Android
        };
    }

    private string ExtractTextFromHtml(string html)
    {
        if (string.IsNullOrEmpty(html))
            return string.Empty;

        // Simple HTML tag removal for notification preview
        var text = System.Text.RegularExpressions.Regex.Replace(html, "<.*?>", string.Empty);
        
        // Limit length for notification
        if (text.Length > 200)
            text = text.Substring(0, 197) + "...";

        return text;
    }
}
