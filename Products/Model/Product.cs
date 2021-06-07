using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Products.Model
{
    
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Size { get; set; }
        public string Sex { get; set; }
        public string Color { get; set; }
        public decimal Price { get; set; }
        public string Details { get; set; }
        public int Quantity { get; set; }
        public string Image { get; set; }
        public int CategoryId { get; set; }

        public Category Category { get; set; }

    }
}
