using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Framework.Domain;

namespace VacationRental.Persistence
{
    public abstract class RepositoryInMemoryBase<T> :
     IRepository<T> where T : class, IAggregateRoot
    {
        public RepositoryInMemoryBase
            (IDictionary<int, T> db)
        {
            DB = db;
        }


        protected static IDictionary<int, T> DB { get; private set; }

        private int GetLastId()
        {
            return DB.Count == 0 ? 1 : DB.Max(c => c.Key) + 1;
        }
        public virtual Task<T> AddAsync
        (T entity, CancellationToken cancellationToken = default)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(paramName: nameof(entity));
            }

            entity.SetId(GetLastId());
            DB.Add(entity.Id, entity);

            return Task.FromResult(entity);
        }

        public void Update(T entity)
        {
            var e = DB.First(x => x.Key == entity.Id);
            if (e.Value == null) throw new ArgumentNullException(paramName: nameof(entity));


        }


        public Task<T> FirstAsync
            (int id, CancellationToken cancellationToken = default)
        {
            var entity = DB.FirstOrDefault(x => x.Key == id);

            return Task.FromResult(entity.Value);
        }
    }
}
