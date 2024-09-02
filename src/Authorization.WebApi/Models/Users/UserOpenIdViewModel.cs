using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Authorization.WebApi.Models.Addresses;

namespace Authorization.WebApi.Models.Users;

/// <summary>
/// User open identifier view model.
/// </summary>
public class UserOpenIdViewModel
{
    /// <summary>
    /// Sub (user identifier).
    /// </summary>
    [JsonPropertyName("sub")]
    public Guid UserId { get; set; }

    /// <summary>
    /// Name (full name).
    /// </summary>
    [JsonPropertyName("name")]
    public string? FullName { get; set; }

    /// <summary>
    /// Given name.
    /// </summary>
    public string? GivenName { get; set; }

    /// <summary>
    /// Family name.
    /// </summary>
    public string? FamilyName { get; set; }

    /// <summary>
    /// Picture.
    /// </summary>
    public string? Picture { get; set; }

    /// <summary>
    /// Email.
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Email verified.
    /// </summary>
    public string? EmailVerified { get; set; }

    /// <summary>
    /// Phone number.
    /// </summary>
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// Phone number verified.
    /// </summary>
    public string? PhoneNumberVerified { get; set; }

    /// <summary>
    /// Locale.
    /// </summary>
    public string Locale => "en-US";

    /// <summary>
    /// Nick name.
    /// </summary>
    [JsonPropertyName("nickname")]
    public string? NickName { get; set; }

    /// <summary>
    /// Profile.
    /// </summary>
    [JsonPropertyName("profile")]
    public string? Profile { get; set; }

    /// <summary>
    /// Zone information.
    /// </summary>
    [JsonPropertyName("zoneinfo")]
    public string? ZoneInfo { get; set; }

    /// <summary>
    /// Address.
    /// </summary>
    [JsonPropertyName("address")]
    public AddressViewModel? Address { get; set; }

    /// <summary>
    /// Update time in UTC.
    /// </summary>
    [JsonPropertyName("updated_at")]
    public DateTime? UpdateTimeUtc { get; set; }

    /// <summary>
    /// Roles.
    /// </summary>
    [JsonPropertyName("role")]
    public List<string>? Roles { get; set; }
}