using Talabat.Core.Entities;
using Talabat.Core.Spepcifications;

namespace Talabat.Core.Repositories.Contract
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<T?> GetAsync(int id);

        Task<IReadOnlyList<T>> GetAllAsync();
        Task<T?> GetWithSpecificationAsync(ISpecifications<T> specs);

        Task<IReadOnlyList<T>> GetAllWithSpecificationAsync(ISpecifications<T> specs);

        Task<int> GetCountAsync(ISpecifications<T> specs);
    }
}
