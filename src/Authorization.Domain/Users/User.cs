using Authorization.Domain.Addresses;
using Microsoft.AspNetCore.Identity;
using Benraz.Infrastructure.Domain.Authorization;
using System;
using System.Collections.Generic;

namespace Authorization.Domain.Users;

/// <summary>
/// User.
/// </summary>
public class User : IdentityUser
{
    /// <summary>
    /// Full name.
    /// </summary>
    public string FullName { get; set; }

    /// <summary>
    /// Status code.
    /// </summary>
    public UserStatusCode StatusCode { get; set; }

    /// <summary>
    /// Nickname.
    /// </summary>
    public string Nickname { get; set; }

    /// <summary>
    /// Profile.
    /// </summary>
    public string Profile { get; set; }

    /// <summary>
    /// Zone information.
    /// </summary>
    public string ZoneInfo { get; set; }

    /// <summary>
    /// User roles.
    /// </summary>
    public ICollection<UserRole> UserRoles { get; set; }

    /// <summary>
    /// Create time in UTC.
    /// </summary>
    public DateTime? CreateTimeUtc { get; set; }

    /// <summary>
    /// Update time in UTC.
    /// </summary>
    public DateTime? UpdateTimeUtc { get; set; }

    /// <summary>
    /// Address identifier.
    /// </summary>
    public Guid? AddressId { get; set; }

    /// <summary>
    /// Address.
    /// </summary>
    public virtual Address Address { get; set; }

    /// <summary>
    /// User mfas.
    /// </summary>
    public ICollection<UserMfa> UserMfas { get; set; }
}