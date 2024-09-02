using Benraz.Infrastructure.Common.Repositories;
using System;

namespace Authorization.Domain.Addresses;

/// <summary>
/// Addresses repository.
/// </summary>
public interface IAddressesRepository : IRepository<Guid,Address>
{
}