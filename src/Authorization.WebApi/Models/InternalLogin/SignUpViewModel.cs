using Authorization.Domain.Users;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Authorization.WebApi.Models.Addresses;

namespace Authorization.WebApi.Models.InternalLogin;

/// <summary>
/// Sign up view model.
/// </summary>
public class SignUpViewModel
{
    /// <summary>
    /// User name.
    /// </summary>
    [Required]
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// First name.
    /// </summary>
    [Required]
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// Last name.
    /// </summary>
    [Required]
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// Password.
    /// </summary>
    [Required]
    [RegularExpression(UserPassword.PASSWORD_REGEX)]
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Phone number.
    /// </summary>
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// Send confirmation email, if false, 
    /// server will not send the confirmation email to user.
    /// </summary>
    public bool SendConfirmationEmail { get; set; } = true;

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

    #nullable enable
    /// <summary>
    /// Address.
    /// </summary>
    [JsonPropertyName("address")]
    public AddressViewModel? Address { get; set; }
}