using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Big_Collection.Models
{
    public class Product
    {
        public Guid Id { get; set; }
        [DisplayName("Products")]
        public string Name { get; set; }
        public string Size { get; set; }
        public string Sex { get; set; }
        public string Color { get; set; }
        public decimal Price { get; set; }
        public string Details { get; set; }
        public int Quantity { get; set; }
        public string Image { get; set; }
        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }
    }

    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public ICollection<Product> Product { get; set; }
    }
}
