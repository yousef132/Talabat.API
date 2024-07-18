using StackExchange.Redis;
using System.Text.Json;
using Talabat.Core.Entities.Cart;
using Talabat.Core.Repositories.Contract;

namespace Talabat.Infrastructure.Cart_Repository
{
    public class CartRepository : ICartRepository
    {
        private readonly IDatabase _database;

        public CartRepository(IConnectionMultiplexer redis)
        {
            this._database = redis.GetDatabase();
        }
        public async Task<bool> DeleteCartAsync(string CartId)
        {
            return await _database.KeyDeleteAsync(CartId);
        }

        public async Task<CustomerCart?> GetCartAsync(string CartId)
        {
            var cart = await _database.StringGetAsync(CartId);

            return cart.IsNullOrEmpty ? null : JsonSerializer.Deserialize<CustomerCart>(cart);
        }

        public async Task<CustomerCart?> UpdateCartAsync(CustomerCart cart)
        {
            var createOrUpdated = await _database.StringSetAsync(cart.Id, JsonSerializer.Serialize(cart), TimeSpan.FromDays(30));

            if (!createOrUpdated)
                return null;

            return await GetCartAsync(cart.Id);

        }
    }
}
