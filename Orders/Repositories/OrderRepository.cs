using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Orders.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orders.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrdersDbContext _dbContext;
        public OrderRepository(OrdersDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Order> CreateOrderAsync(Order order)
        {
            if (order != null && order.OrderProduct != null)
            {
                bool isOrderExistInDb = await CheckIfOrderExistInDBAsync(order.Id);
                if (!isOrderExistInDb)
                {
                    _dbContext.Order.Add(order);
                    var result = await _dbContext.SaveChangesAsync();

                    if (result > 0)
                        return order;
                    else
                        return null;
                }
                else
                    return null;
            }
            else
                return null;
        }
      

        public async Task<Order> DeleteOrderByOrderIdAsync(Guid orderId)
        {
            var order = await _dbContext.Order.FindAsync(orderId);

            if (order == null)
                return null;

            _dbContext.Order.Remove(order);
            await _dbContext.SaveChangesAsync();
            return order;
        }

        public async Task<List<Order>> GetAllOrdersAsync()
        {
            return await _dbContext.Order.OrderByDescending(x => x.Date).Include(x => x.Status).ToListAsync();
        }

        public async Task<Order> GetOrderByOrderIdAsync(Guid orderId)
        {
            if (orderId == Guid.Empty)
                return null;
            return await _dbContext.Order.Where(x => x.Id == orderId)
               .Include(x => x.OrderProduct)
               .Include(x => x.Status)
               .OrderByDescending(x => x.Date)
               .FirstOrDefaultAsync();
        }

        public async Task<List<Order>> GetOrdersByUserIdAsync(Guid userId)
        {
            bool isUserwithOrderExistInDb = await CheckIfUserWithOrderExistInDBAsync(userId);

            if (userId == Guid.Empty)
                return null;
            return await _dbContext.Order.Where(x => x.UserId == userId)
               .Include(x => x.Status)
               .OrderByDescending(x => x.Date)
               .ToListAsync();
        }

        public async Task<Order> UpdateOrderAsync(Order order)
        {
            if (order.Id != Guid.Empty && await CheckIfOrderExistInDBAsync(order.Id))
            {
                try
                {                   
                    _dbContext.Entry(order).State = EntityState.Modified;
                    await _dbContext.SaveChangesAsync();
                    return order;
                }
                catch (SqlException)        // using Microsoft.Data.SqlClient;
                {
                    return null;
                }
            }

            return null;
        }

        public async Task<bool> UpdateOrderStatusAsync(int statusId, Guid orderId)
        {
            var listOfStatusId = await _dbContext.Status.ToListAsync();
            var statusIdExist = listOfStatusId.Any(x => x.Id == statusId);
            var orderToUpdate = await _dbContext.Order.FindAsync(orderId);

            if (statusIdExist && orderToUpdate != null)
            {
                orderToUpdate.StatusId = statusId;
                await _dbContext.SaveChangesAsync();
                return true;
            }

            return false;
        }


        /// <summary>
        /// Check if an order exist by its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>bool</returns>
        public async Task<bool> CheckIfOrderExistInDBAsync(Guid id)
        {
           return await _dbContext.Order.AnyAsync(x => x.Id == id);
        }


        /// <summary>
        /// Check if user with order exist by its id
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns>bool</returns>
        public async Task<bool> CheckIfUserWithOrderExistInDBAsync(Guid userId)
        {
            return await _dbContext.Order.AnyAsync(x => x.UserId == userId);
        }
    }
}
