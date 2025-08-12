using OnlineStoreAPI.Models;
using OnlineStoreAPI.Dto;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStoreAPI.Services
{
    public class OrderService : IOrderService
    {
        private readonly AppDbContext _context;

        public OrderService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Orders> PlaceOrder(int userId, CheckoutDto checkoutDto)
        {
            var user = await _context.Users.FindAsync(userId); // Get user for email
            if (user == null)
                return null; // Or handle error as needed

            var order = new Orders
            {
                UserId = userId,
                OrderDate = DateTime.UtcNow,
                ShippingAddress = checkoutDto.ShippingAddress,
                TotalAmount = checkoutDto.TotalAmount,
                CustomerEmail = user.Email // Set the CustomerEmail field here
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // Add order items
            foreach (var item in checkoutDto.OrderItems)
            {
                var orderItem = new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    OrderId = order.Id,
                    Price = item.Price
                };
                _context.OrderItems.Add(orderItem);
            }

            await _context.SaveChangesAsync();
            return order;
        }


        // Implementing GetOrdersByUser logic
        public async Task<IEnumerable<Orders>> GetOrdersByUser(int userId)
        {
            return await _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        // Implementing GetAllOrders for Admin
        public async Task<IEnumerable<Orders>> GetAllOrders(OrderFilterDto filterDto)
        {
            var ordersQuery = _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .AsQueryable();

            if (!string.IsNullOrEmpty(filterDto.CustomerEmail))
            {
                ordersQuery = ordersQuery.Where(o => o.CustomerEmail.Contains(filterDto.CustomerEmail));
            }

            if (filterDto.StartDate.HasValue && filterDto.EndDate.HasValue)
            {
                ordersQuery = ordersQuery.Where(o => o.OrderDate >= filterDto.StartDate && o.OrderDate <= filterDto.EndDate);
            }

            return await ordersQuery.OrderByDescending(o => o.OrderDate).ToListAsync();
        }
    }
}
