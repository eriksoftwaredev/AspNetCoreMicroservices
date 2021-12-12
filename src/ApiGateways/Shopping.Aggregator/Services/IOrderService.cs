using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shopping.Aggregator.Models;

namespace Shopping.Aggregator.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderResponseModel>> GetOrdersByUsername(string username);
    }
}
