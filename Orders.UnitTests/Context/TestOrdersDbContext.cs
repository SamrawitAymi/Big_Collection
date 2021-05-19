using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Orders.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.UnitTests.Context
{
    public class TestOrdersDbContext
    {
        public AppSettings AppsettingsConfig { get; set; }
        public OrdersDbContext DbContext { get; set; }

        public TestOrdersDbContext()
        {
            AppsettingsConfig = new AppSettings();
            DbContext = InitializeContext();
        }

        public OrdersDbContext InitializeContext()
        {
            var config = AppsettingsConfig.Config.GetConnectionString("SqlDatabase");
            var dbOption = new DbContextOptionsBuilder<OrdersDbContext>()
                .UseSqlServer(config).Options;

            return new OrdersDbContext(dbOption);
        }

        public void Dispose()
        {
            DbContext?.Dispose();
        }
    }
}
