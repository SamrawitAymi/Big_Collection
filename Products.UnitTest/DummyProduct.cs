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
            Product product = new Product()
            {
                Id = Guid.NewGuid(),
                Name = "Emma Willis Wrap Dress",
                Color = "Pink",
                Size = "M",
                Price = 100,
                Quantity = 1,
                CategoryId = 2,
                Image = "https://xcdn.next.co.uk/common/Items/Default/Default/Publications/G68/shotview-315x472/267/225-425s4.jpg",
                Details = "Brighten up your wardrobe with this head-turning maxi-length dress, part of our exclusive Emma Willis collection. With a wrapover detail at the front, it's designed to flatter with elbow-length puffed sleeves, a soft collar and a ruffle-trim skirt. Wear yours with everything from sneakers to strappy sandals. Other colour available"
            };
            return product;
        }
    }
}
