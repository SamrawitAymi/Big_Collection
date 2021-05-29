using Big_Collection.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Big_Collection.ViewModels
{
    public class ProductsCatagoryViewModel
    {
        public List<Product>Products { get; set; }
        public Category  Catagories { get; set; }
        public string ProductCategory { get; set; }
        public int SearchingString { get; set; }
    }
}
