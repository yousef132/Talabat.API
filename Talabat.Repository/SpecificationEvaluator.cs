using Microsoft.EntityFrameworkCore;
using Talabat.Core.Entities;
using Talabat.Core.Spepcifications;

namespace Talabat.Infrastructure
{
    public static class SpecificationEvaluator<TEntity> where TEntity : BaseEntity
    {
        public static IQueryable<TEntity> BuildQuery(IQueryable<TEntity> baseQuery
            , ISpecifications<TEntity> specs)
        {
            var query = baseQuery;

            if (specs?.Criteria is not null)
                query = query.Where(specs.Criteria);

            if (specs?.OrderBy is not null)
                query = query.OrderBy(specs.OrderBy);

            else if (specs?.OrderByDesc is not null)
                query = query.OrderByDescending(specs.OrderByDesc);

            if (specs.IsPaginated)
                query = query.Skip(specs.Skip).Take(specs.Take);

            query = specs?.Includes.Aggregate(query,
                (current, includeExpression) => current.Include(includeExpression));


            return query;
        }

    }
}
