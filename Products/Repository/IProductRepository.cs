using Products.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Products.Repository
{
    public interface IProductRepository
    {
        Task<Product> CreateProductAsync(Product product);
        Task<Product> UpdateProduct(Product product);
        Task<Product> DeleteProductById(Guid productId);
        Task<Product> GetProductById(Guid productId);
        Task <IList<Product>> GetProductByCategory(string searchCategory);
        Task<IEnumerable<Product>> GetAllProducts();
        Task<bool> UpdateProductsInStockAsync(Dictionary<Guid, int> products);
    }
}
