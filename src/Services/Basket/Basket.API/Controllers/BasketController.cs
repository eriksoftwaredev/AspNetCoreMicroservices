using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Basket.API.Entities;
using Basket.API.gRPCServices;
using Basket.API.Repositoies;
using Microsoft.Extensions.Logging;

namespace Basket.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BasketController : Controller
    {
        private readonly IBasketRepository _repository;
        private readonly DiscountGRPCService _discountGrpcService;
        private readonly ILogger<BasketController> _logger;

        public BasketController(IBasketRepository repository, DiscountGRPCService discountGrpcService, ILogger<BasketController> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _discountGrpcService = discountGrpcService ?? throw new ArgumentNullException(nameof(discountGrpcService));
            _logger = logger;
        }

        [HttpGet("{username}", Name = "GetBasket")]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> GetBasket(string username)
        {
            var basket = await _repository.GetBasket(username);
            return Ok(basket ?? new ShoppingCart(username));
        }

        [HttpPost]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> UpdateBasket([FromBody] ShoppingCart basket)
        {
            // TODO : Cammunicate with Discount.GRPC and calculate latest price of products

            foreach (var item in basket.Items)
            {
                _logger.LogInformation($"Basket.API calls GetDiscount({item.ProductName})");
                var coupon = await _discountGrpcService.GetDiscount(item.ProductName);
                _logger.LogInformation($"Basket.API gets the coupon: productName = {coupon.ProductName}, amount = {coupon.Amount}");

                item.Price -= coupon.Amount;
            }

            return Ok(await _repository.UpdateBasket(basket));
        }


        [HttpDelete("{username}", Name = "DeleteBasket")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteBasket(string username)
        {
            await _repository.DeleteBasket(username);
            return Ok();
        }
    }
}
