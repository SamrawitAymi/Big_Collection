using Microsoft.VisualStudio.TestTools.UnitTesting;
using Orders.Repositories;
using Orders.UnitTests.Context;
using Orders.UnitTests.DummyOrder;

namespace Orders.UnitTests
{
    [TestClass]
    public class OrdersTest
    {
        public static IOrderRepository OrderRepository { get; set; }
        public static TestOrdersDbContext OrderTestContext { get; set; }


        [ClassInitialize]
        public static void TestFixtureSetup(TestContext context)
        {
            OrderTestContext = new TestOrdersDbContext();
            OrderRepository = new OrderRepository(OrderTestContext.DbContext);
        }

        [TestMethod]
        public void TestMethod1()
        {
            // Arrange           
            var dummyOrder = DummyTestOrder.TestOrder();

            // Act
            var newOrder = OrderRepository.CreateOrderAsync(dummyOrder).Result;

            // Assert
            Assert.AreEqual(dummyOrder, newOrder);

            DeleteDummyOrderFromDatabase(dummyOrder);
        }

        private static void DeleteDummyOrderFromDatabase(Order dummyOrder)
        {
            OrderTestContext.DbContext.Remove(dummyOrder);
            OrderTestContext.DbContext.SaveChanges();
        }
    }
}
