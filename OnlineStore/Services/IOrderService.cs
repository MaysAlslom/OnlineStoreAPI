using OnlineStoreAPI.Models;
using OnlineStoreAPI.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineStoreAPI.Services
{
    public interface IOrderService
    {
        Task<Orders> PlaceOrder(int userId, CheckoutDto checkoutDto);
        Task<IEnumerable<Orders>> GetOrdersByUser(int userId);
        Task<IEnumerable<Orders>> GetAllOrders(OrderFilterDto filterDto);  // Make sure this is defined
    }
}

