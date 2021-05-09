using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orders.Context
{
    public class OrdersDbContext : DbContext
    {
        public OrdersDbContext()
        {
        }

        public OrdersDbContext(DbContextOptions<OrdersDbContext> options) : base(options)
        {

        }

        public DbSet<Order> Order { get; set; }
        public DbSet<OrderProducts> OrderProduct { get; set; }
        public DbSet<Status> Status { get; set; }

    }
}

