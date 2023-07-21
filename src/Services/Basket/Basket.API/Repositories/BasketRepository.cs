using Basket.API.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Basket.API.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDistributedCache _cache;

        public BasketRepository(IDistributedCache cache)
        {
            _cache = cache ?? throw new ArgumentException(nameof(cache));
        }

        public async Task<ShoppingCart> GetBasket(string userName)
        {
            var basket = await _cache.GetStringAsync(userName);
            if (String.IsNullOrEmpty(basket)) return null;
            return JsonConvert.DeserializeObject<ShoppingCart>(basket);
        }

        public async Task<ShoppingCart> UpdateBasket(ShoppingCart cart)
        {
            await _cache.SetStringAsync(cart.UserName, JsonConvert.SerializeObject(cart));
            return await GetBasket(cart.UserName);
        }

        public async Task DeleteBasket(string userName)
        {
            await _cache.RemoveAsync(userName);
        }

    
    }
}
