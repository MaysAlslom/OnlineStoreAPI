
using OnlineStoreAPI.Dto;
using OnlineStoreAPI.Models;
using Microsoft.EntityFrameworkCore;

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
            // Create a new order
            var order = new Orders
            {
                CustomerName = checkoutDto.CustomerName,
                OrderDate = DateTime.UtcNow,
                ShippingAddress = checkoutDto.ShippingAddress,
                TotalAmount = checkoutDto.TotalAmount,
                UserId = userId
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // Save order items
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

            return order; // Return the created order
        }

        public async Task<IEnumerable<Orders>> GetOrdersByUser(int userId)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Where(o => o.UserId == userId)
                .ToListAsync();
        }
    }
}
