using Microsoft.EntityFrameworkCore;
using Products.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Products.Context
{
    public class ProductsDbContext : DbContext
    {
        public ProductsDbContext()
        {
        }

        public ProductsDbContext(DbContextOptions<ProductsDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Product { get; set; }
        public DbSet<Category> Category { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Product>(entity => {
                entity.Property(x => x.Name).IsRequired();
                entity.Property(x => x.Size).IsRequired();
                entity.Property(x => x.Color).IsRequired();
                entity.Property(x => x.Price).IsRequired().HasColumnType("decimal(18,2)");
                entity.Property(x => x.Quantity).IsRequired();
                entity.Property(x => x.Image).HasDefaultValue("https://i.pinimg.com/originals/21/ff/a1/21ffa154e3d8639299017ab5683e55cc.jpg");
                entity.Property(x => x.Details).HasDefaultValue("The product information is missing ...");

            });
            builder.Entity<Category>().HasData(
              new Category
              {
                  Id = 1,
                  Name = "Electronics",
                  Type = "Electronic Items",
              },
              new Category
              {
                  Id = 2,
                  Name = "Clothes",
                  Type = "Dresses",
              },
               new Category
               {
                   Id = 3,
                   Name = "Shoes",
                   Type = "Shoes Types",
               },
              new Category
              {
                  Id = 4,
                  Name = "Jewlery",
                  Type = "Jewlery Items",
              });
        }
    }
}