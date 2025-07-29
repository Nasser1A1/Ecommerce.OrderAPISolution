using eCommerce.SharedLib.Logs;
using eCommerce.SharedLib.ResponseT;
using Microsoft.EntityFrameworkCore;
using OrderApi.Application.Interfaces;
using OrderApi.Domain.Entities;
using OrderApi.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OrderApi.Infrastructure.Repository
{
    public class OrderRepository(OrderDbContext context) : IOrder
    {
        public async Task<Response> CreateAsync(Order entity)
        {
            try
            {
                var OrderCreated = await context.orders.AddAsync(entity);
                await context.SaveChangesAsync();
                return OrderCreated.Entity.Id > 0
                    ? new Response(true, "Order created successfully.")
                    : new Response(false, "Failed to create order.");
            }
            catch (Exception ex)
            {
                LogExceptions.LogException(ex);
                return new Response(false, "An error occurred while creating the order: " + ex.Message);

            }
        }

        public async Task<Response> DeleteAsync(Order entity)
        {
            try
            {
               var orderToDelete = await FindByIdAsync(entity.Id);
                if (orderToDelete == null)
                {
                    return new Response(false, "Order not found.");
                }
                context.orders.Remove(entity);
                await context.SaveChangesAsync();
                return new Response(true, "Order deleted successfully.");

            }
            catch (Exception ex)
            {
                LogExceptions.LogException(ex);
                return new Response(false, "An error occurred while deleting the order: " + ex.Message);

            }
        }

        public async Task<Order> FindByIdAsync(int id)
        {
            try
            {
                var order = await context.orders.FindAsync(id);
             
                return order != null ? order : null!;
            }
            catch (Exception ex)
            {
                LogExceptions.LogException(ex);
                throw new Exception("An error occurred while retrieving the order: " + ex.Message);

            }
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            try
            {
                var orders = await context.orders.AsNoTracking().ToListAsync();
                return orders.Any() ? orders : new List<Order>();
            }
            catch (Exception ex)
            {
                LogExceptions.LogException(ex);
                throw new Exception("An error occurred while retrieving orders: " + ex.Message);

            }
        }

        public async Task<Order> GetByAsync(Expression<Func<Order, bool>> predicate)
        {
            try
            {
                var orders = await context.orders.Where(predicate).FirstOrDefaultAsync();
                return orders != null ? orders : null!;

            }
            catch (Exception ex)
            {
                LogExceptions.LogException(ex);
                throw new Exception("An error occurred while retrieving the order: " + ex.Message);

            }
        }

        public async Task<IEnumerable<Order>> GetOrdersAsync(Expression<Func<Order, bool>> predicate)
        {
            try
            {
                var orders = await context.orders.Where(predicate).ToListAsync();
                return orders != null ? orders : null!;

            }
            catch (Exception ex)
            {
                LogExceptions.LogException(ex);
                throw new Exception("An error occurred while retrieving orders : " + ex.Message);
            }
        }

        public async Task<Response> UpdateAsync(Order entity)
        {
             try
            {
                var existingOrder = await FindByIdAsync(entity.Id);
                if (existingOrder == null)
                {
                    return new Response(false, "Order not found.");
                }
                context.Entry(existingOrder).State = EntityState.Detached;
                context.orders.Update(entity);
                await context.SaveChangesAsync();
                return new Response(true, "Order updated Successfully.");
            }
            catch (Exception ex)
            {
                LogExceptions.LogException(ex);
                return new Response(false, "An error occurred while updating the order: " + ex.Message);

            }
        }
    }
}
