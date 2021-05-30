using Big_Collection.Common;
using Big_Collection.Models;
using Big_Collection.Services;
using Big_Collection.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Big_Collection.Controllers
{
    public class OrderController : Controller
    {
        private readonly IClientService _clientService;
        private readonly ICookieHandler _cookieHandler;
        private readonly CartService _cartService;

        public OrderController(CartService cartService, IClientService clientService, ICookieHandler cookieHandler)
        {
            _cartService = cartService;
            _clientService = clientService;
            _cookieHandler = cookieHandler;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var result = await _clientService.SendRequestToGatewayAsync(ApiGateways.ApiGateway.ALL_ORDERS, HttpMethod.Get);

            if (result.IsSuccessStatusCode)
            {
                var orders = await _clientService.ReadResponseAsync<List<Order>>(result.Content);
                return View(orders);
            }

            return View();
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> OrderUpdate(Guid id)
        {
            if (id == Guid.Empty)
                return RedirectToAction(nameof(Index));

            var result = await _clientService.SendRequestToGatewayAsync(ApiGateways.ApiGateway.ORDERS_GATEWAY_BASEURL + id, HttpMethod.Get);

            if (result.IsSuccessStatusCode)
            {
                var order = await _clientService.ReadResponseAsync<Order>(result.Content);
                return View(order);
            }

            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> OrderUpdate(int statusId, Guid id)
        {
            var apiLocation = ApiGateways.ApiGateway.ORDERS_GATEWAY_BASEURL + $"{statusId}/{id}";
            var response = await _clientService.SendRequestToGatewayAsync(apiLocation, HttpMethod.Put);

            var updateSuccessFlag = false;

            if (response.IsSuccessStatusCode)
                updateSuccessFlag = true;

            TempData["StatusUpdated"] = updateSuccessFlag;
            TempData["OrderID"] = id;
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> OrderPage()
        {
            var userId = await _cookieHandler.GetClaimFromAuthenticationCookieAsync("UserId");
            var cart = _cartService.GetCartContent();
            var userResult = await _clientService.SendRequestToGatewayAsync(ApiGateways.ApiGateway.GET_ORDER_BY_USERID + userId, HttpMethod.Get);
            var user = await _clientService.ReadResponseAsync<User>(userResult.Content);

            OrderViewModel vm = new OrderViewModel
            {
                User = user,
                Cart = cart
            };
            ViewBag.TotalPrice = _cartService.CalculateTotalPrice();
            return View(vm);
        }
        [HttpGet]
        public IActionResult OrderRegistrationPage()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<List<Order>> GetOrdersAsync()
        {
            var response = await _clientService.SendRequestToGatewayAsync(ApiGateways.ApiGateway.ALL_ORDERS, HttpMethod.Get);
            return await ReturnOrders(response);
        }

        [HttpGet]
        public async Task<Order> GetOrderByIdAsync(Guid id)
        {
            var response = await _clientService.SendRequestToGatewayAsync(ApiGateways.ApiGateway.ORDERS_GATEWAY_BASEURL + id, HttpMethod.Get);
            return await ReturnOrder(response);
        }

        [HttpPost]
        public async Task<Order> PostOrderAsync(Order order)
        {
            var response = await _clientService.SendRequestToGatewayAsync(ApiGateways.ApiGateway.CREATE_ORDER, HttpMethod.Post, order);
            return await ReturnOrder(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<Order> UpdateOrderAsync(Guid id, Order order)
        {
            var response = await _clientService.SendRequestToGatewayAsync(ApiGateways.ApiGateway.ORDERS_GATEWAY_BASEURL + id, HttpMethod.Put, order);
            return await ReturnOrder(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public async Task<Order> DeleteOrderAsync(Guid id)
        {
            var response = await _clientService.SendRequestToGatewayAsync(ApiGateways.ApiGateway.ORDERS_GATEWAY_BASEURL + id, HttpMethod.Delete);
            return await ReturnOrder(response);

        }

        [HttpGet]
        public async Task<List<Order>> GetOrdersByUserIdAsync(Guid id)
        {

            var response = await _clientService.SendRequestToGatewayAsync(ApiGateways.ApiGateway.GET_ORDER_BY_USERID + id, HttpMethod.Get);
            return (response.IsSuccessStatusCode)
                ? await _clientService.ReadResponseAsync<List<Order>>(response.Content) : null;
        }
        [HttpGet]
        public async Task<IActionResult> GetUserOrders()
        {
            var id = await _cookieHandler.GetClaimFromAuthenticationCookieAsync("UserId");

            var response = await _clientService.SendRequestToGatewayAsync(ApiGateways.ApiGateway.GET_ORDER_BY_USERID + id, HttpMethod.Get);

            if (response.IsSuccessStatusCode)
            {
                var orders = await _clientService.ReadResponseAsync<List<Order>>(response.Content);
                return View(orders);
            }
            return View();
        }
        

        private async Task<List<Order>> ReturnOrders(HttpResponseMessage response)
        {
            return (response.IsSuccessStatusCode)
                ? await _clientService.ReadResponseAsync<List<Order>>(response.Content) : null;
        }

        async Task<Order> ReturnOrder(HttpResponseMessage response)
        {
            return (response.IsSuccessStatusCode)
                ? await _clientService.ReadResponseAsync<Order>(response.Content) : null;
        }
        public async Task<ActionResult> OrderConfirmationPage(Guid orderId)
        {
            var userId = await _cookieHandler.GetClaimFromAuthenticationCookieAsync("UserId");
            var userResult = await _clientService.SendRequestToGatewayAsync(ApiGateways.ApiGateway.GET_USER + userId, HttpMethod.Get);
            var user = await _clientService.ReadResponseAsync<User>(userResult.Content);

            var orderResult = await _clientService.SendRequestToGatewayAsync(ApiGateways.ApiGateway.ORDERS_GATEWAY_BASEURL + orderId, HttpMethod.Get);
            var order = await _clientService.ReadResponseAsync<Order>(orderResult.Content);

            OrderSuccessViewModel Model = new OrderSuccessViewModel
            {
                Order = order,
                User = user
            };
            return View(Model);
        }
    }
}
