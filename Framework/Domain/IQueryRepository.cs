using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Framework.Domain
{
    public interface IQueryRepository<T> where T : IEntity
    {
        Task<IEnumerable<T>> Find
            (Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

        Task<T> FindAsync
            (long id, CancellationToken cancellationToken = default);
    }
}