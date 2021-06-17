using APIGateway.Models;
using APIGateway.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AggregateController : ControllerBase
    {
        private ClientService _clientService;

        public AggregateController()
        {
            this._clientService = new ClientService();
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrderAsync(Order order)
        {
            var jwtToken = Request.Headers["Authorization"];

            var productsToDecreaseFromStock = new Dictionary<Guid, int>();
            foreach (var item in order.OrderProduct)
                productsToDecreaseFromStock.Add(item.ProductId, item.Quantity);

            var productResponse = await _clientService.PostRequestAsync("https://localhost:44391/api/product/updatestock", productsToDecreaseFromStock, jwtToken);

            if (productResponse.IsSuccessStatusCode)
            {
                var orderResponse = await _clientService.PostRequestAsync("https://localhost:44369/api/Order/create", order, jwtToken);

                if (orderResponse.IsSuccessStatusCode)
                {
                    var json = await orderResponse.Content.ReadAsStringAsync();
                    var newOrder = JsonConvert.DeserializeObject<Order>(json);

                    return Ok(newOrder);
                }
            }

            return null;
        }
    }
}
