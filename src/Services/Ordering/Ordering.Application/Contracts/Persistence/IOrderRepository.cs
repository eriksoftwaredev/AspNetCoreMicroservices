using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ordering.Domain.Entities;

namespace Ordering.Application.Contracts.Persistence
{
    public interface IOrderRepository : IAsyncRepository<Order>
    {
        Task<IEnumerable<Order>> GetOrdersByUsername(string username);
    }
}
