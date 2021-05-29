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
    public class ProductsController : Controller
    {
        private readonly IClientService _clientService;

        public ProductsController(IClientService clientService)
        {
            _clientService = clientService;
        }

        public async Task<IActionResult> ProductsPage()
        {
            var products = await GetProductsAsync();
            return View(products);
        }

        public async Task<IActionResult> ProductsCategoryPage(string productCategory, string stringSearch)
        {
            var products = await GetProductByCategoryNameAsync(productCategory, stringSearch);
            //var vm = new ProductsCatagoryViewModel
            //{
            //    Catagories = new Category(productCategory.Distinct().ToList()),
            //    Products = products.ToList()
            //};
            return View(products);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ManageProductsPage()
        {
            var products = await GetProductsAsync();
            return View(products);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditProductPage(Guid Id)
        {
            var product = await GetProductByIdAsync(Id);
            return View(product);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> EditProductPageAsync(Product product)
        {
            if (ModelState.IsValid)
            {
                var responseProduct = await UpdateProductAsync(product.Id, product);
                if (responseProduct != null)
                {
                    TempData["AlertMessage"] = "Product updated!";
                    return RedirectToAction("ManageProductsPage");
                }
            }
            return View(product);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProduct(Guid Id)
        {
            var response = await DeleteProductAsync(Id);
            if (response != null)
            {
                TempData["AlertMessage"] = "Product removed!";
                return RedirectToAction("ManageProductsPage");
            }
            TempData["AlertMessage"] = "Could not remove product!";
            return RedirectToAction("ManageProductsPage");
        }

        public async Task<IActionResult> ProductDetails(Guid id)
        {
            var product = await GetProductByIdAsync(id);
            return View(product);
        }

        [HttpGet]
        public async Task<List<Product>> GetProductsAsync()
        {
            var response = await _clientService.SendRequestToGatewayAsync(ApiGateways.ApiGateway.ALL_PRODUCTS, HttpMethod.Get);

            return (response.IsSuccessStatusCode)
                ? await _clientService.ReadResponseAsync<List<Product>>(response.Content) : null;
        }

        [HttpGet]
        public async Task<Product> GetProductByIdAsync(Guid id)
        {
            var response = await _clientService.SendRequestToGatewayAsync(ApiGateways.ApiGateway.PRODUCTS_API_BASEURL + id, HttpMethod.Get);

            return (response.IsSuccessStatusCode)
                ? await _clientService.ReadResponseAsync<Product>(response.Content) : null;
        }


        [HttpGet]
        public async Task<List<Product>> GetProductByCategoryNameAsync(string productCategory, string searchString)
        {
            var response = await _clientService.SendRequestToGatewayAsync(ApiGateways.ApiGateway.ALL_PRODUCTS + productCategory, HttpMethod.Get);
            
            return (response.IsSuccessStatusCode)
                ? await _clientService.ReadResponseAsync<List<Product>>(response.Content) : null;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<Product> PostProductAsync(Product product)
        {
            var response = await _clientService.SendRequestToGatewayAsync(ApiGateways.ApiGateway.CREATE_PRODUCT, HttpMethod.Post, product);

            return (response.IsSuccessStatusCode)
                ? await _clientService.ReadResponseAsync<Product>(response.Content) : null;
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<Product> UpdateProductAsync(Guid id, Product product)
        {
            var response = await _clientService.SendRequestToGatewayAsync(ApiGateways.ApiGateway.EDIT_PRODUCT + id, HttpMethod.Put, product);

            return (response.IsSuccessStatusCode)
                ? await _clientService.ReadResponseAsync<Product>(response.Content) : null;
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public async Task<Product> DeleteProductAsync(Guid id)
        {
            var response = await _clientService.SendRequestToGatewayAsync(ApiGateways.ApiGateway.DELETE_PRODUCT + id, HttpMethod.Delete);

            return (response.IsSuccessStatusCode)
                ? await _clientService.ReadResponseAsync<Product>(response.Content) : null;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult CreateProductPage()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateProductPage(Product product)
        {
            if (ModelState.IsValid)
            {
                var result = await PostProductAsync(product);
                if (result != null)
                {
                    TempData["AlertMessage"] = "New product added!";
                    return RedirectToAction("ManageProductsPage");
                }
            }
            return View();
        }
    }
}