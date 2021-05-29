using Products.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Products.UnitTest
{
    public class DummyProduct
    {
        public static Product Product()
        {
            //var size = new List<string>()
            //        {
            //            "XXS", "XS", "S", "M", "XXL", "XL", "L", "F"
            //        };
            //var sex = new List<string>()
            //        {
            //            "M","F"
            //        };
            //var colors = new List<string>()
            //        {
            //            "White","Black", "Blue", "Red", "Navy", "Gray", "Purpule"
            //        };
            Product product = new Product()
            {
                Id = Guid.NewGuid(),
                Name = "Lucky Brand Turquoise Collar Necklace",
                Color = "Blue",
                Size = "No-Size",
                Sex = "F",
                Price = 499,
                Details = "Lucky Brand fashion jewelry with etched metal detail and colored stone",
                Quantity = 9,
                Image = "https://www.richandrare.com/image/cache/catalog/R26/4962-1-1000x1000.jpg",
                CategoryId = 4
            };
            return product;
        }
    }
}
