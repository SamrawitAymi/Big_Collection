using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.UnitTests.DummyOrder
{
    public class DummyTestOrder
    {
        public static Order TestOrder()
        {

            var dummyOrder = new Order
            {
                Id = Guid.NewGuid(),
                StatusId = 1,
                TotalPrice = 3000.99M,
                PaymentId = 1,
                UserId = Guid.NewGuid(),
                Date = DateTime.Now,

                OrderProduct = new List<OrderProducts>()
                {
                    new OrderProducts { Quantity = 3, ProductId = Guid.NewGuid(), Name = "Shirt" },
                    new OrderProducts { Quantity = 2, ProductId = Guid.NewGuid(), Name = "Shoe" }
                }
            };
            return dummyOrder;
        }

        public static Order TestOrderWithNullOrderProduct()
        {
            var dummyOrder = new Order
            {
                Id = Guid.NewGuid(),
                StatusId = 1,
                TotalPrice = 100.99M,
                PaymentId = 1,
                UserId = Guid.NewGuid(),
                Date = DateTime.Now   
            };
            return dummyOrder;
        }
    }
}
