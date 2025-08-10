using OnlineStoreAPI.Dto;
using OnlineStoreAPI.Models;

namespace OnlineStoreAPI.Services
{
    public interface IOrderService
    {
        Task<Orders> PlaceOrder(int userId, CheckoutDto checkoutDto);
        Task<IEnumerable<Orders>> GetOrdersByUser(int userId);
    }
}
