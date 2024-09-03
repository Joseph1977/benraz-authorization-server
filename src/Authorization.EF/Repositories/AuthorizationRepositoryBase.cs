using Benraz.Infrastructure.Common.EntityBase;
using Benraz.Infrastructure.EF;

namespace Authorization.EF.Repositories
{
    /// <summary>
    /// Base authorization repository.
    /// </summary>
    /// <typeparam name="TKey">Key.</typeparam>
    /// <typeparam name="TEntity">Entity.</typeparam>
    public abstract class AuthorizationRepositoryBase<TKey, TEntity> :
        RepositoryBase<TKey, TEntity, AuthorizationDbContext>
        where TEntity : class, IAggregateRoot<TKey>
    {
        /// <summary>
        /// Creates repository.
        /// </summary>
        /// <param name="context">Context.</param>
        public AuthorizationRepositoryBase(AuthorizationDbContext context)
            : base(context)
        {
        }
    }
}


