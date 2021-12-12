using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Shopping.Aggregator.Models;
using Shopping.Aggregator.Services;

namespace Shopping.Aggregator.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ShoppingController : ControllerBase
    {
        private readonly ICatalogService _catalogService;
        private readonly IBasketService _basketService;
        private readonly IOrderService _orderService;

        public ShoppingController(ICatalogService catalogService, IBasketService basketService, IOrderService orderService)
        {
            _catalogService = catalogService ?? throw new ArgumentNullException(nameof(catalogService));
            _basketService = basketService ?? throw new ArgumentNullException(nameof(basketService));
            _orderService = orderService ?? throw new ArgumentNullException(nameof(orderService));
        }

        [HttpGet("{username}", Name = "GetShopping")]
        [ProducesResponseType(typeof(ShoppingModel), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingModel>> GetShopping(string username)
        {
            // Get basket with username
            var basket = await _basketService.GetBasket(username);

            // Iterate basket items and consume products with basket item productId member
            foreach (var item in basket.Items)
            {
                var product = await _catalogService.GetCatalog(item.ProductId);

                // Set additional product fields onto basket item
                item.ProductName = product.Name;
                item.Category = product.Category;
                item.Summary = product.Summary;
                item.Description = product.Description;
                item.ImageFile = product.ImageFile;
            }

            // Consume ordering microservices in order to retrieve order list
            var orders = await _orderService.GetOrdersByUsername(username);

            // Return root ShoppingModel dto class which including all responses
            var shoppingModel=new ShoppingModel()
            {
                Username=username,
                BasketWithProducts=basket,
                Orders=orders
            };
            return Ok(shoppingModel);
        }
    }
}
