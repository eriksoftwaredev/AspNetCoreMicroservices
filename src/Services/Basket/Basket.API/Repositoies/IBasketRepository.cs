using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Basket.API.Entities;

namespace Basket.API.Repositoies
{
    public interface IBasketRepository
    {
        Task<ShoppingCart> GetBasket(string username);
        Task<ShoppingCart> UpdateBasket(ShoppingCart basket);
        Task DeleteBasket(string username);
    }
}
