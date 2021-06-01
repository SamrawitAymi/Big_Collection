using Microsoft.VisualStudio.TestTools.UnitTesting;
using Orders.Repositories;
using Orders.UnitTests.Context;
using Orders.UnitTests.DummyOrder;
using System;
using System.Collections.Generic;

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
        public void CreateNewOrder_CreatingOrder_RetunCretatedOrder()
        {
            // Arrange           
            var dummyOrder = DummyTestOrder.TestOrder();

            // Act
            var newOrder = OrderRepository.CreateOrderAsync(dummyOrder).Result;

            // Assert
            Assert.AreEqual(dummyOrder, newOrder);

            DeleteDummyOrderFromDatabase(dummyOrder);
        }

        [TestMethod]
        public void CreateNullOrder_CreatingOrder_RetunNulldOrder()
        {
            // Arrange           
            var dummyOrder = DummyTestOrder.TestOrder();

            // Act
            var newOrder = OrderRepository.CreateOrderAsync(dummyOrder).Result;
            newOrder = null;

            // Assert
            Assert.IsNull(newOrder);

        }


        [TestMethod]
        public void DeleteOrderByOrderIdAsync_DeleteOrderFromDatabse_ReturnDeletedOrderAreEqual()
        {

            // Arrange
            Order dummyOrder = CreateDummyOrderToDatabase();

            // Act
           
            var deletedProduct = OrderRepository.DeleteOrderByOrderIdAsync(dummyOrder.Id).Result;

            // Assert
            Assert.AreEqual(dummyOrder, deletedProduct);
        }


        [TestMethod]
        public void DeleteOrderByOrderIdAsync_DeleteEmtyOrder_ReturnNull()
        {

            // Arrange
            var orderId = Guid.Empty;

            // Act

            var deletedProduct = OrderRepository.DeleteOrderByOrderIdAsync(orderId).Result;

            // Assert
            Assert.IsNull(deletedProduct);
        }


        [TestMethod]
        public void GetAllOrdersAsync_GetAllOrdersFromDatbase_ReturnListOfOrders()
        {
            // Act             
            var orders = OrderRepository.GetAllOrdersAsync().Result;

            // Assert
            Assert.IsInstanceOfType(orders, typeof(List<Order>));
        }

        [TestMethod]
        public void UpdateOrderAsync_UpdateStatus_ReturnUpdatedOrder()
        {

            // Arrange
            Order dummyOrder = CreateDummyOrderToDatabase();
            var oldStatusId = 1;
            dummyOrder.StatusId = 2;

            // Act
            var order = OrderRepository.UpdateOrderAsync(dummyOrder).Result;

            // Assert
            Assert.AreNotEqual(oldStatusId, order.StatusId);
            Assert.AreEqual(dummyOrder.StatusId, order.StatusId);

            DeleteDummyOrderFromDatabase(dummyOrder);
        }

    
        [TestMethod]
        public void UpdateOrderStatusAsync_UpdateOrderStatusToSent_ReturnTrue()
        {
            // Arrange 
            Order dummyOrder = CreateDummyOrderToDatabase();

            // Act              
            var result = OrderRepository.UpdateOrderStatusAsync(2, dummyOrder.Id).Result;

            // Assert
            Assert.IsTrue(result);

            DeleteDummyOrderFromDatabase(dummyOrder);
        }

        [TestMethod]
        public void UpdateOrderStatusAsync_UpdateOrderStatusToNonExistingStatus_ReturnFalse()
        {
            // Arrange 
            Order dummyOrder = CreateDummyOrderToDatabase();

            // Act             
            var result = OrderRepository.UpdateOrderStatusAsync(10, dummyOrder.Id).Result;

            // Assert
            Assert.IsFalse(result);

            DeleteDummyOrderFromDatabase(dummyOrder);
        }



        private static void DeleteDummyOrderFromDatabase(Order dummyOrder)
        {
            OrderTestContext.DbContext.Remove(dummyOrder);
            OrderTestContext.DbContext.SaveChanges();
        }

        private static Order CreateDummyOrderToDatabase()
        {
            var dummyOrder = DummyTestOrder.TestOrder();
            OrderTestContext.DbContext.Order.Add(dummyOrder);
            OrderTestContext.DbContext.SaveChanges();
            return dummyOrder;
        }
    }
}
