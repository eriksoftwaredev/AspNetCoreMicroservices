using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shopping.Aggregator.Models;

namespace Shopping.Aggregator.Services
{
    public interface IBasketService
    {
        Task<BasketModel> GetBasket(string username);
    }
}
