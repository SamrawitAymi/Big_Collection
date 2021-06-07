using Microsoft.EntityFrameworkCore;
using Products.Context;
using Products.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace Products.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductsDbContext _context;

        public ProductRepository(ProductsDbContext context)
        {
            this._context = context;
        }

        public async Task<Product> CreateProductAsync(Product product)
        {
            try
            {
                if(IsProductModelValid(product))
                {
                    bool isProductExistInDb = await IsProductExistInDb(product.Id);
                    if (!isProductExistInDb)
                    {
                        _context.Product.Add(product);
                        var result = await _context.SaveChangesAsync();
                        if (result > 0)
                            return product;
                        else
                            return null;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {

                return null;
            }
        }

        public async Task <Product> UpdateProduct(Product product)
        {
            try
            {
                if (product != null)
                {
                    bool isProductExistInDb = await IsProductExistInDb(product.Id);
                    if (isProductExistInDb && product.Id != Guid.Empty)
                    {
                        _context.Entry(product).State = EntityState.Modified;
                        await _context.SaveChangesAsync();
                        return product;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception)
            {

                return null;
            }
            return null;
        }

        public async Task<Product> GetProductById(Guid productId)
        {
            if (productId == Guid.Empty)
                return null;
            var result = await _context.Product.FindAsync(productId);
            return result;
        }

        public async Task<Product> DeleteProductById(Guid productId)
        {
            try
            {
                var product = await _context.Product.FindAsync(productId);

                if (product != null)
                {
                    _context.Product.Remove(product);
                    await _context.SaveChangesAsync();
                    return product;
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<IEnumerable<Product>> GetAllProducts()
        {         
            var result = await _context.Product.OrderBy(x => x.Name).ToListAsync();
            return result;
        }

        public async Task<IList<Product>> GetProductByCategory(string searchCategory)
        {
            var categoryId = GetCategoryId(searchCategory);

            var result = await _context.Product.Where(p => p.CategoryId == categoryId).ToListAsync();
           
            return result;
                       
        }

        private int GetCategoryId(string name)
        {
            if (String.IsNullOrEmpty(name))
                return 0;
            var result = _context.Category.SingleOrDefault(x => x.Name.ToLower() == name.ToLower());
            return result.Id;
        }


        public async Task<bool> UpdateProductsInStockAsync(Dictionary<Guid, int> products)
        {
            try
            {    
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    foreach (var item in products)
                    {
                        var product = await GetProductById(item.Key);
                        product.Quantity -= item.Value;

                        if (product.Quantity < 0)
                            product.Quantity = 0;

                        await UpdateProduct(product);
                    }

                    transaction.Complete();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        private async Task<bool> IsProductExistInDb(Guid productId)
        {
            return await _context.Product.AnyAsync(x => x.Id == productId);
        }

        private static bool IsProductModelValid(Product product)
        {
            if (product == null || product.Name == null || product.Price == 0 || product.Image == null)
            {
                return false;
            }
            return true;
        }
    }

}
