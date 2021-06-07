using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Products.Context;
using Products.Model;
using Products.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Products.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProductsDbContext _dbContext;
        private readonly IProductRepository _productRepo;

        public ProductController(ProductsDbContext productsDbContext, IProductRepository productRepository)
        {
            _dbContext = productsDbContext;
            _productRepo = productRepository;
        }


        // POST: /api​/Product​/create
        [HttpPost ("create")]
        [Authorize (Roles = "Admin")]
        public async Task<ActionResult<Product>> PostProductAsync(Product product)
        {
            if (product != null)
            {
                try
                {
                    var result = await _productRepo.CreateProductAsync(product);
                    if (result != null)
                    {
                        return Ok(result);
                    }
                }

                catch (Exception)
                {
                    return BadRequest();
                }
            }
            return BadRequest();
        }

        // GET: /api/Product/id
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProductByIdAsync(Guid id)
        {
            try
            {
                if (_dbContext.Product.Any(p => p.Id == id) == true)
                {
                    var result = await _productRepo.GetProductById(id);
                    if (result != null)
                    {
                        return Ok(result);
                    }
                }
            }
            catch (Exception)
            {
                return BadRequest();
            }
            return NotFound();
        }

        // GET: /api/Product/all
        [HttpGet("getall")]
        public async Task<ActionResult<IEnumerable<Product>>> GetAllProductsAsync()
        {
            var result = await _productRepo.GetAllProducts();
            return Ok(result);
        }

        // GET: /api/Product/all
        [HttpGet("category")]
        public async Task<IList<Product>> GetProductsbyCatagoryAsync( string searchProduct)
        {
            var result = await _productRepo.GetProductByCategory(searchProduct);
            if (result != null)
            {
                return result; 
            }

            return null;
        }

    
        //PUT: ​/api​/Product​/edit​/id
        [Authorize(Roles = "Admin")]
        [HttpPut("edit/{id}")]
        public async Task<ActionResult<Product>> UpdateProductAsync(Product product, Guid id)
        {
            if (id != product.Id)
            { return BadRequest(); }
            try
            {
                var result = await _productRepo.UpdateProduct(product);
                if (result != null)
                    return Ok(result);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (_dbContext.Product.Any(x => x.Id != id))
                {
                    return NotFound();
                }
                else
                {
                    return BadRequest();
                }
            }
            return NoContent();
        }

        [Authorize]
        [HttpPost("updatestock")]
        public async Task<ActionResult> UpdateProductQuantity(Dictionary<Guid, int> products)
        {
            var result = await _productRepo.UpdateProductsInStockAsync(products);

            if (result)
                return Ok();
            else
                return BadRequest();
        }

        //DELETE: ​/api​/Product​/delete
        [HttpDelete ("delete/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Product>> DeleteProductAsync(Guid id)
        {
            if (_dbContext.Product.Any(x => x.Id == id))
            {
                var result = await _productRepo.DeleteProductById(id);
                if (result != null)
                
                    return Ok(result);
            }
            return BadRequest();
        }
    }
}
