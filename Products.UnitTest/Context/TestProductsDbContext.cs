using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Products.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Products.UnitTest.Context
{
    public class TestProductsDbContext
    {
        public AppSettings AppsettingsConfig { get; set; }
        public ProductsDbContext DbContext { get; set; }

        public TestProductsDbContext()
        {
            AppsettingsConfig = new AppSettings();
            DbContext = InitializeContext();
        }

        public ProductsDbContext InitializeContext()
        {
            var config = AppsettingsConfig.Config.GetConnectionString("SqlDatabase");
            var dbOption = new DbContextOptionsBuilder<ProductsDbContext>()
                .UseSqlServer(config).Options;

            return new ProductsDbContext(dbOption);
        }

        public void Dispose()
        {
            DbContext?.Dispose();
        }
    }
}
