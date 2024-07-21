using System.Collections;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;
using Talabat.Infrastructure.Data;
using Talabat.Infrastructure.Generic_Repository;

namespace Talabat.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreContext context;
        private Hashtable _repositories;
        public UnitOfWork(StoreContext context)
        {
            this.context = context;
            _repositories = new Hashtable();

        }
        public Task<int> CompleteAsync()
            => context.SaveChangesAsync();

        public ValueTask DisposeAsync()
            => context.DisposeAsync();

        // create repository per request  
        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            // if repository<order> => key = order
            var key = typeof(TEntity).Name;
            if (!_repositories.ContainsKey(key))
            {
                var repo = new GenericRepository<TEntity>(context);

                _repositories.Add(key, repo);
            }

            return _repositories[key] as IGenericRepository<TEntity>;
        }
    }
}
