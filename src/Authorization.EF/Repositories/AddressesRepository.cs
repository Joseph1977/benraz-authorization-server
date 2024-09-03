using Authorization.Domain.Addresses;
using Benraz.Infrastructure.EF;
using System;

namespace Authorization.EF.Repositories;

/// <summary>
/// Addresses repository.
/// </summary>
public class AddressesRepository : RepositoryBase<Guid, Address, AuthorizationDbContext>
{
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="context"></param>
    public AddressesRepository(AuthorizationDbContext context)
        : base(context)
    {
    }
}