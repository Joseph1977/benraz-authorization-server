namespace Authorization.Domain.Users;

/// <summary>
/// User action notifications service settings.
/// </summary>
public class UserActionNotificationsServiceSettings
{
    /// <summary>
    /// Receivers emails.
    /// </summary>
    public string ReceiversEmails { get; set; }

    /// <summary>
    /// Is login notify enabled.
    /// </summary>
    public bool IsLoginNotifyEnabled { get; set; }

    /// <summary>
    /// User login email subject.
    /// </summary>
    public string UserLoginEmailSubject { get; set; }
}
