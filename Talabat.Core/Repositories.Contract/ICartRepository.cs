using Talabat.Core.Entities.Cart;

namespace Talabat.Core.Repositories.Contract
{
    public interface ICartRepository
    {
        Task<CustomerCart?> GetCartAsync(string CartId);

        Task<CustomerCart?> UpdateCartAsync(CustomerCart cart);

        Task<bool> DeleteCartAsync(string CartId);
    }
}
