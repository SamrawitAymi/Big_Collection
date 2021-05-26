using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        [HttpGet("id")]
        public async Task<ActionResult<Product>> GetProductByIdAsync(Guid id)
        {
            try
            {
                if (_dbContext.Product.Any(p => p.Id == id))
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
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<Product>>> GetAllProductsAsync()
        {
            var result = await _productRepo.GetAllProducts();
            return Ok(result);
        }


        //PUT: ​/api​/Product​/edit​/id
        [Authorize(Roles = "Admin")]
        [HttpPut("edit/id")]
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
            catch (Exception)
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

        //DELETE: ​/api​/Product​/delete
        [HttpDelete ("delete")]
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
