using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Orders.Context;
using Orders.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orders.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : Controller
    {
        private readonly OrdersDbContext _orderDbContext;
        private readonly IOrderRepository _orderRepository;

        public OrdersController(OrdersDbContext orderDbContext, IOrderRepository orderRepository)
        {
            this._orderDbContext = orderDbContext;
            this._orderRepository = orderRepository;
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<ActionResult<Order>> CreateOrder (Order order)
        {
            if (order != null)
            {
                try
                {
                    var newOrder = await _orderRepository.CreateOrderAsync(order);
                    if (newOrder != null)
                        return Ok(newOrder);
                }
                catch (Exception)
                {
                    return BadRequest();
                }
            }
            return BadRequest();
        }


        [Authorize(Roles ="Admin")]
        [HttpGet("getall")]
        public async Task<ActionResult<Order>> GetAllOrders()
        {
            var orders = await _orderRepository.GetAllOrdersAsync();
            return Ok(orders.OrderByDescending(o => o.Date));
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrderById(Guid id)
        {
            if (_orderDbContext.Order.Any(x => x.Id != id))
                return NotFound();
            var order = await _orderRepository.GetOrderByOrderIdAsync(id);
            if (order != null)
                return Ok(order);

            return BadRequest();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Order>> DeleteOrder(Guid id)
        {
            if (id != Guid.Empty)
            {
                var result = await _orderRepository.DeleteOrderByOrderIdAsync(id);

                if (result != null)
                    return Ok(result);
            }

            return BadRequest();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{status}/{id}")]
        public async Task<ActionResult<Order>> CreateOrderStatus(int statusId, Guid id)
        {
            var order = _orderDbContext.Order.Any(x => x.Id == id);
            if (!order)
                return NotFound();

            var result = await _orderRepository.UpdateOrderStatusAsync(statusId, id);

            if (result)
                return Ok(true);
            return BadRequest();
        }

        // PUT: api/Orders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(Order order)
        {
            if (order.Id == Guid.Empty)
                return BadRequest();

            try
            {
                var result = await _orderRepository.UpdateOrderAsync(order);
                if (result != null)
                    return Ok(result);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (_orderDbContext.Order.Any(x => x.Id != order.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [Authorize]
        [HttpGet("user/{id}")]
        public async Task<ActionResult<List<Order>>> GetOrdersByUserId(Guid id)
        {
            var orderExist = _orderDbContext.Order.Any(x => x.UserId != id);

            if (!orderExist)
                return NotFound();

            var result = await _orderRepository.GetOrdersByUserIdAsync(id);

            if (result != null)
                return Ok(result);

            return BadRequest();

        }
    }
}