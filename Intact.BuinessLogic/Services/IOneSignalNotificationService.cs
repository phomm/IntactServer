namespace Intact.BusinessLogic.Services;

/// <summary>
/// Interface for OneSignal notification service
/// </summary>
public interface IOneSignalNotificationService
{
    /// <summary>
    /// Send a notification to specific user(s) by external user ID
    /// </summary>
    Task<bool> SendToUserAsync(string userId, string title, string message, Dictionary<string, object>? data = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Send a notification to multiple users by external user IDs
    /// </summary>
    Task<bool> SendToUsersAsync(IEnumerable<string> userIds, string title, string message, Dictionary<string, object>? data = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Send a notification to users with specific tags
    /// </summary>
    Task<bool> SendToSegmentAsync(Dictionary<string, string> tags, string title, string message, Dictionary<string, object>? data = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Send a broadcast notification to all users
    /// </summary>
    Task<bool> SendBroadcastAsync(string title, string message, Dictionary<string, object>? data = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Send an email-style notification through OneSignal (wrapping email into push notification)
    /// </summary>
    Task<bool> SendEmailNotificationAsync(string userId, string subject, string htmlBody, CancellationToken cancellationToken = default);

    /// <summary>
    /// Register a user device with OneSignal
    /// </summary>
    Task<bool> RegisterUserDeviceAsync(string userId, string deviceToken, string deviceType = "Android", CancellationToken cancellationToken = default);

    /// <summary>
    /// Update user tags for segmentation
    /// </summary>
    Task<bool> UpdateUserTagsAsync(string userId, Dictionary<string, string> tags, CancellationToken cancellationToken = default);
}
