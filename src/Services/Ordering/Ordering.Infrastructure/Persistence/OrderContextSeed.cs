using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Ordering.Domain.Entities;

namespace Ordering.Infrastructure.Persistence
{
    public class OrderContextSeed
    {
        public static async Task SeedAsync(OrderContext orderContext, ILogger<OrderContextSeed> logger)
        {
            if (!orderContext.Orders.Any())
            {
                orderContext.Orders.AddRange(GetPreconfiguredOrders());
                await orderContext.SaveChangesAsync();
                logger.LogInformation("Seed database associated with context {DbContextName}", typeof(OrderContext).Name);
            }
        }

        public static IEnumerable<Order> GetPreconfiguredOrders()
        {
            return new List<Order>()
            {
                new Order()
                {
                    UserName="msn",
                    FirstName="Mohsen",
                    LastName = "Pourali",
                    EmailAddress = "m.pourali1982@gmail.com",
                    AddressLine = "Azadi street",
                    Country = "Iran",
                    TotalPrice = 360,
                },
            };
        }
    }
}
