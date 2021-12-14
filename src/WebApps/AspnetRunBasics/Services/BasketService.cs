using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using AspnetRunBasics.Extensions;
using AspnetRunBasics.Models;

namespace AspnetRunBasics.Services
{
    public class BasketService:IBasketService
    {
        private readonly HttpClient _client;

        public BasketService(HttpClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }
        public async Task<BasketModel> GetBasket(string username)
        {
            var response = await _client.GetAsync($"/Basket/{username}");
            return await response.ReadContentAs<BasketModel>();
        }

        public async Task<BasketModel> UpdateBasket(BasketModel model)
        {
            var response = await _client.PostAsJsonAsync($"/Basket", model);

            if (response.IsSuccessStatusCode)
                return await response.ReadContentAs<BasketModel>();
            else
            {
                throw new Exception("Something went wrong when calling UpdateBasket api.");
            }
        }

        public async Task CheckoutBasket(BasketCheckoutModel model)
        {
            var response = await _client.PostAsJsonAsync($"/Basket/Checkout", model);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Something went wrong when calling CheckoutBasket api.");
            }
        }
    }
}
