using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Framework.Domain
{
    public interface IRepository<T> where T : IAggregateRoot
    {
        Task<T> AddAsync
            (T entity, CancellationToken cancellationToken = default);

        void Update(T entity);


        Task<T> FirstAsync
            (int id, CancellationToken cancellationToken = default);

        Task PublishEvent(T entity);
    }
}
