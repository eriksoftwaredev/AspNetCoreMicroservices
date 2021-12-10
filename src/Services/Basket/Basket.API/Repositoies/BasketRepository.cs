using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Basket.API.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Basket.API.Repositoies
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDistributedCache _redisCache;

        public BasketRepository(IDistributedCache redisCache)
        {
            _redisCache = redisCache ?? throw new ArgumentNullException(nameof(redisCache));
        }

        public async Task DeleteBasket(string username)
        {
            await _redisCache.RemoveAsync(username);
        }

        public async Task<ShoppingCart> GetBasket(string username)
        {
            var basket = await _redisCache.GetStringAsync(username);
            if (string.IsNullOrEmpty(basket))
                return null;

            return JsonConvert.DeserializeObject<ShoppingCart>(basket);
        }

        //public async Task<ShoppingCart> UpdateBasket(ShoppingCart basket)
        //{
        //    await _redisCache.SetStringAsync(basket.UserName, JsonConvert.SerializeObject(basket));
        //    return await GetBasket(basket.UserName);
        //}

        public async Task<ShoppingCart> UpdateBasket(ShoppingCart basket)
        {
            await _redisCache.SetStringAsync(basket.UserName, JsonConvert.SerializeObject(basket));

            return await GetBasket(basket.UserName);

            //var deser = JsonConvert.SerializeObject(basket);
            // //_redisCache.SetString("sss", "eeee");
            //await _redisCache.SetStringAsync(basket.UserName, deser);

            //return await GetBasket(basket.UserName);
        }
    }
}
