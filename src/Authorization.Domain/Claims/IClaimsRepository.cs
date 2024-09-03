using Benraz.Infrastructure.Common.Repositories;
using System;

namespace Authorization.Domain.Claims
{
    /// <summary>
    /// Claims repository.
    /// </summary>
    public interface IClaimsRepository : IRepository<Guid, IdentityClaim>
    {
    }
}


