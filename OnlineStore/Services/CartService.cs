using Microsoft.EntityFrameworkCore;
using OnlineStoreAPI.Dto;
using OnlineStoreAPI.Models;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStoreAPI.Services
{
    public class CartService : ICartService
    {
        private readonly AppDbContext _context;

        public CartService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Cart> AddToCart(int userId, CartItemDto cartItemDto)
        {
            var cart = await _context.Carts.FirstOrDefaultAsync(c => c.UserId == userId);
            if (cart == null)
            {
                cart = new Cart { UserId = userId };
                _context.Carts.Add(cart);
            }

            var product = await _context.Products.FindAsync(cartItemDto.ProductId);
            if (product == null || product.StockQuantity < cartItemDto.Quantity)
                return null;

            var cartItem = new CartItem
            {
                ProductId = cartItemDto.ProductId,
                Quantity = cartItemDto.Quantity,
                CartId = cart.Id
            };

            _context.CartItems.Add(cartItem);
            await _context.SaveChangesAsync();

            return cart;
        }

        public async Task<Cart> UpdateCart(int userId, int cartItemId, int quantity)
        {
            var cartItem = await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.Cart.UserId == userId && ci.Id == cartItemId);

            if (cartItem == null || quantity < 1)
                return null;

            var product = await _context.Products.FindAsync(cartItem.ProductId);
            if (product == null || quantity > product.StockQuantity)
                return null;

            cartItem.Quantity = quantity;
            await _context.SaveChangesAsync();

            return await GetCart(userId);  // Return the updated cart
        }


        public async Task<Cart> RemoveFromCart(int userId, int cartItemId)
        {
            var cartItem = await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.Cart.UserId == userId && ci.Id == cartItemId);

            if (cartItem == null)
                return null;

            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();

            return await GetCart(userId);  // Return the updated cart
        }


        public async Task<Cart> GetCart(int userId)
        {
            var cart = await _context.Carts.Include(c => c.CartItems)
                                           .ThenInclude(ci => ci.Product)
                                           .FirstOrDefaultAsync(c => c.UserId == userId);
            return cart;
        }
    }
}
