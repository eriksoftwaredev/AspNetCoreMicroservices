using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Basket.API.Entities;
using Basket.API.gRPCServices;
using Basket.API.Repositoies;
using EventBus.Messages.Events;
using MassTransit;
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
        private readonly IMapper _mapper;
        private readonly IPublishEndpoint _publishEndpoint;

        public BasketController(IBasketRepository repository, DiscountGRPCService discountGrpcService, ILogger<BasketController> logger, IMapper mapper, IPublishEndpoint publishEndpoint)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _discountGrpcService = discountGrpcService ?? throw new ArgumentNullException(nameof(discountGrpcService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
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

        [Route("[action]")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Checkout([FromBody] BasketCheckout basketCheckout)
        {
            // Get existing basket
            var basket = await _repository.GetBasket(basketCheckout.UserName);
            if (basket == null)
                return BadRequest();

            // Create basketCheckoutEvent -- Set TotalPrice on basketCheckout eventMessage
            var eventMessage = _mapper.Map<BasketCheckoutEvent>(basketCheckout);
            eventMessage.TotalPrice = basket.TotalPrice;

            // Send checkout event to rabbitMQ
            await _publishEndpoint.Publish(eventMessage);
          

            // Remove the basket
            await _repository.DeleteBasket(basket.UserName);

            return Accepted();
        }
    }
}
