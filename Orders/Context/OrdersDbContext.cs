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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Order>(entity =>
            {
                entity.Property(x => x.Date).IsRequired();
                entity.Property(x => x.TotalPrice).IsRequired().HasColumnType("decimal(18,2)");
                entity.Property(x => x.StatusId).IsRequired().HasDefaultValue(1);
            });


            builder.Entity<OrderProducts>(entity =>
            {
                entity.Property(x => x.OrderId).IsRequired();
                entity.Property(x => x.ProductId).IsRequired();
                entity.Property(x => x.Name).IsRequired().HasMaxLength(50);
            });


            builder.Entity<Status>().HasData(
                new Status() { Id = 1, Name = "Accepted" },
                new Status() { Id = 2, Name = "Processing" },
                new Status() { Id = 3, Name = "Shipped" },
                new Status() { Id = 4, Name = "Delivered" },
                new Status() { Id = 5, Name = "Completed" });

        }
    }
}

