using System;

namespace Authorization.Domain.Emails.UserLogin;

/// <summary>
/// User login email model.
/// </summary>
public class UserLoginEmailModel
{
    /// <summary>
    /// Full name.
    /// </summary>
    public string FullName { get; set; }

    /// <summary>
    /// User name.
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// Create time utc.
    /// </summary>
    public DateTime CreateTimeUtc { get; set; }
}
