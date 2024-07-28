using StackExchange.Redis;
using System.Text.Json;
using Talabat.Core.Services.Contract;

namespace Talabat.Service.Cache_Service
{
    public class ResponseCacheService : IResponseCacheService
    {
        private readonly IDatabase _Database;

        public ResponseCacheService(IConnectionMultiplexer multiplexer)
        {
            _Database = multiplexer.GetDatabase();
        }
        public async Task CacheResponseAsync(string key, object Response, TimeSpan timeToiLive)
        {
            if (Response is null)
                return;

            var serializeOptions = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var serializedResponse = JsonSerializer.Serialize(Response, serializeOptions);
            await _Database.StringSetAsync(key, serializedResponse, timeToiLive);
        }

        public async Task<string?> GetCachedResponseAsync(string key)
            => await _Database.StringGetAsync(key);
    }
}
