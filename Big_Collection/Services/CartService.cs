using Big_Collection.Common;
using Big_Collection.Models;
using Big_Collection.ViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Big_Collection.Services
{
    public class CartService
    {
        private readonly IHttpContextAccessor _context;

        public CartService(IHttpContextAccessor httpContext)
        {
            this._context = httpContext;
        }

        public CartViewModel ViewCart()
        {
            var cart = GetCartContent();
            var cartViewModel = new CartViewModel();

            if (cart != null)
            {
                cartViewModel.CartItems = cart;
                cartViewModel.TotalPrice = CalculateTotalPrice();
            }

            return cartViewModel;
        }

        public List<CartItem> GetCartContent()
        {
            return SessionHandler.GetObjectFromJson<List<CartItem>>(_context.HttpContext.Session, Cookies.CART_SESSION_COOKIE);
        }

        //public void SetPayPalUrl(Payment payment)
        //{
        //    SessionHandler.SetObjectAsJson(_context.HttpContext.Session, Cookies.CART_SESSION_COOKIE, payment);
        //}

        //public Payment GetPayPalUrl()
        //{
        //    return SessionHandler.GetObjectFromJson<Payment>(_context.HttpContext.Session, Cookies.PAYPAL_URL_SESSION_COOKIE);
        //}

        public void SaveCartChanges(List<CartItem> cartItems)
        {
            SessionHandler.SetObjectAsJson(_context.HttpContext.Session, Cookies.CART_SESSION_COOKIE, cartItems);
        }

        public decimal CalculateTotalPrice()
        {
            var cart = GetCartContent();

            if (cart == null)
                return 0;

            return cart.Sum(cartItem => cartItem.Product.Price * cartItem.Quantity);
        }

        public int CountProductsInCart()
        {
            var cart = GetCartContent();

            if (cart == null)
                return 0;

            return cart.Sum(cartItem => cartItem.Quantity);
        }

        public int AddNewProductToCart(Product product)
        {
            var cart = GetCartContent();

            if (cart == null)
                cart = new List<CartItem>();

            cart.Add(new CartItem { Product = product, Quantity = 1 });

            SaveCartChanges(cart);
            return CountProductsInCart();
        }

        public int IncreaseExistingProductQuantity(Guid product)
        {
            var cart = GetCartContent();
            int index = FindIndexOfCartItem(cart, product);

            if (index != -1)
                cart[index].Quantity++;

            SaveCartChanges(cart);
            return CountProductsInCart();
        }

        public int ReduceExistingProductQuantity(Guid productId)
        {
            var cart = GetCartContent();

            if (cart != null)
            {
                var index = FindIndexOfCartItem(cart, productId);

                if (index != -1)
                    cart[index].Quantity--;

                if (cart[index].Quantity < 1)
                    cart.RemoveAt(index);
            }

            SaveCartChanges(cart);
            return CountProductsInCart();
        }

        public int RemoveProductFromCart(Guid productId)
        {
            var cart = GetCartContent();

            if (cart != null)
            {
                int index = FindIndexOfCartItem(cart, productId);

                if (index != -1)
                    cart.RemoveAt(index);

                SaveCartChanges(cart);
            }

            SaveCartChanges(cart);
            return CountProductsInCart();
        }

        private int FindIndexOfCartItem(List<CartItem> cart, Guid productId)
        {
            return cart.FindIndex(x => x.Product.Id == productId);
        }

        public bool ProductExistInCart(Guid productId)
        {
            var cart = GetCartContent();

            if (cart == null)
                return false;

            return GetCartContent().Any(x => x.Product.Id == productId);
        }

        public void EmptyCart()
        {
            _context.HttpContext.Session.Remove(Cookies.CART_SESSION_COOKIE);
        }
    }
}
