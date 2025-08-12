using OnlineStoreAPI.Dto;
using OnlineStoreAPI.Models;

namespace OnlineStoreAPI.Services
{
    public interface ICartService
    {
        Task<Cart> AddToCart(int userId, CartItemDto cartItemDto);
        Task<Cart> UpdateCart(int userId, int cartItemId, int quantity);
        Task<Cart> RemoveFromCart(int userId, int cartItemId);
        Task<Cart> GetCart(int userId);
    }

}
