using Big_Collection.Common;
using Big_Collection.Models;
using Big_Collection.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Big_Collection.Controllers
{
    public class CartController : Controller
    {
        private readonly IClientService _clientService;
        private readonly CartService _cartService;

        public CartController(IClientService clientService, CartService cartService)
        {
            _clientService = clientService;
            _cartService = cartService;
        }

        public IActionResult Cart()
        {
            var cart = _cartService.ViewCart();

            if (cart.CartItems.Count == 0)
            {
                ViewBag.Message = "Your shopping cart is empty";
            }

            return View(cart);
        }

        public ActionResult CalculateTotalPrice()
        {
            return Ok(_cartService.CalculateTotalPrice());
        }

        public async Task<IActionResult> AddToCart(Guid productId)
        {
            if (!_cartService.ProductExistInCart(productId))
            {
                var response = await _clientService.SendRequestToGatewayAsync(ApiGateways.ApiGateway.PRODUCTS_API_BASEURL + productId, HttpMethod.Get);
                var product = await _clientService.ReadResponseAsync<Product>(response.Content);
                return Ok(_cartService.AddNewProductToCart(product));
            }

            return Ok(_cartService.IncreaseExistingProductQuantity(productId));
        }

        public int CountProductsInCart()
        {
            return _cartService.CountProductsInCart();
        }

        public IActionResult ReduceFromCart(Guid productId)
        {
            return Ok(_cartService.ReduceExistingProductQuantity(productId));
        }

        public ActionResult RemoveFromCart(Guid productId)
        {
            var productsInCart = _cartService.RemoveProductFromCart(productId);
            return Ok(productsInCart);
        }


    }
}
