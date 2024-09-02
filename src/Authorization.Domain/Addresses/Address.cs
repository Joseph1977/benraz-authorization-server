using Benraz.Infrastructure.Common.EntityBase;
using System;

namespace Authorization.Domain.Addresses;

/// <summary>
/// Address.
/// </summary>
public class Address :  IAggregateRoot<Guid>
{
    /// <summary>
    /// Identifier.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Street address.
    /// </summary>
    public string StreetAddress { get; set; }

    /// <summary>
    /// Locality.
    /// </summary>
    public string Locality { get; set; }

    /// <summary>
    /// Region.
    /// </summary>
    public string Region { get; set; }

    /// <summary>
    /// Postal code.
    /// </summary>
    public string PostalCode { get; set; }

    /// <summary>
    /// Country.
    /// </summary>
    public string Country { get; set; }

    /// <summary>
    /// Create time UTC.
    /// </summary>
    public DateTime CreateTimeUtc { get; set; }
    
    /// <summary>
    /// Update time UTC.
    /// </summary>
    public DateTime UpdateTimeUtc { get; set; }
}