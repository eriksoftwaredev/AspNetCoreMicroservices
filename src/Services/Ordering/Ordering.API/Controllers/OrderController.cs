using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Features.Orders.Queries.GetOrderList;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Ordering.Application.Features.Orders.Commands.CheckoutOrder;
using Ordering.Application.Features.Orders.Commands.DeleteOrder;
using Ordering.Application.Features.Orders.Commands.UpdateOrder;

namespace Ordering.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrderController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet("{username}", Name = "GetOrdersByUsername")]
        [ProducesResponseType(typeof(IEnumerable<OrdersVm>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<OrdersVm>>> GetOrdersByUsername(string username)
        {
            var query = new GetOrderListQuery(username);
            var orders = await _mediator.Send(query);

            return Ok(orders);
        }

        // testing method
        [HttpPost(Name = "CheckoutOrder")]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<int>> CheckoutOrder([FromBody] CheckoutOrderCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        // testing method
        [HttpPut(Name = "UpdateOrder")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> UpdateOrder([FromBody] UpdateOrderCommand command)
        {
            var result = await _mediator.Send(command);
            return NoContent();
        }

        // testing method
        [HttpDelete("{id}", Name = "DeleteOrder")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> DeleteOrder(int id)
        {
            var command = new DeleteOrderCommand(id);
            var result = await _mediator.Send(command);
            return NoContent();
        }
    }
}
