﻿using Microsoft.EntityFrameworkCore;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Spepcifications;
using Talabat.Infrastructure.Data;

namespace Talabat.Infrastructure.Generic_Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StoreContext context;

        public GenericRepository(StoreContext context)
        {
            this.context = context;
        }
        public async Task<IReadOnlyList<T>> GetAllAsync()
            => await context.Set<T>().ToListAsync();

        public async Task<T?> GetAsync(int id)
            => await context.Set<T>().FindAsync(id);
        public async Task<IReadOnlyList<T>> GetAllWithSpecificationAsync(ISpecifications<T> specs)
            => await ApplySpecification(specs).ToListAsync();
        public async Task<T?> GetWithSpecificationAsync(ISpecifications<T> specs)
            => await ApplySpecification(specs).FirstOrDefaultAsync();

        private IQueryable<T> ApplySpecification(ISpecifications<T> specs)
            => SpecificationEvaluator<T>.BuildQuery(context.Set<T>(), specs);

        public async Task<int> GetCountAsync(ISpecifications<T> specs)
            => await ApplySpecification(specs).CountAsync();

        public async Task Add(T entity)
            => await context.Set<T>().AddAsync(entity);

        public void Delete(T entity)
            => context.Set<T>().Remove(entity);

        public void Update(T entity)
            => context.Set<T>().Update(entity);
    }
}
