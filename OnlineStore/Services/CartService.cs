using Microsoft.EntityFrameworkCore;  // This is required for `FirstOrDefaultAsync` and `Include` methods
using OnlineStoreAPI.Models;
using System.Linq;  // This is also needed for LINQ methods like `FirstOrDefault`
using System.Threading.Tasks;
using OnlineStoreAPI.Services;  // Add this reference in your controller or service file
using OnlineStoreAPI.Dto;  // Ensure this namespace is included


public class CartService : ICartService
{
    private readonly AppDbContext _context;

    public CartService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Cart> AddToCart(int userId, CartItemDto cartItemDto)
    {
        // Retrieve or create cart for the user from the database
        var cart = await _context.Carts.FirstOrDefaultAsync(c => c.UserId == userId);
        if (cart == null)
        {
            cart = new Cart { UserId = userId };
            _context.Carts.Add(cart);
        }

        var product = await _context.Products.FindAsync(cartItemDto.ProductId);
        if (product == null || product.StockQuantity < cartItemDto.Quantity)
            return null;  // Product not available or insufficient stock

        var cartItem = new CartItem
        {
            ProductId = cartItemDto.ProductId,
            Quantity = cartItemDto.Quantity,
            CartId = cart.Id
        };

        _context.CartItems.Add(cartItem);
        await _context.SaveChangesAsync();  // Save changes to the database

        return cart;
    }

    public async Task<Cart> UpdateCart(int userId, int cartItemId, int quantity)
    {
        var cart = await _context.Carts.Include(c => c.CartItems).FirstOrDefaultAsync(c => c.UserId == userId);
        if (cart == null)
            return null;  // Cart not found

        var cartItem = cart.CartItems.FirstOrDefault(ci => ci.Id == cartItemId);
        if (cartItem == null)
            return null;  // Cart item not found

        var product = await _context.Products.FindAsync(cartItem.ProductId);
        if (product == null || quantity > product.StockQuantity)
            return null;  // Insufficient stock

        cartItem.Quantity = quantity;
        await _context.SaveChangesAsync();  // Save changes to the database

        return cart;  // Return the updated cart
    }

    public async Task<Cart> RemoveFromCart(int userId, int cartItemId)
    {
        var cart = await _context.Carts.Include(c => c.CartItems).FirstOrDefaultAsync(c => c.UserId == userId);
        if (cart == null)
            return null;  // Cart not found

        var cartItem = cart.CartItems.FirstOrDefault(ci => ci.Id == cartItemId);
        if (cartItem == null)
            return null;  // Cart item not found

        _context.CartItems.Remove(cartItem);  // Remove item from the cart
        await _context.SaveChangesAsync();  // Save changes to the database

        return cart;  // Return the updated cart
    }

    public async Task<Cart> GetCart(int userId)
    {
        var cart = await _context.Carts.Include(c => c.CartItems).ThenInclude(ci => ci.Product).FirstOrDefaultAsync(c => c.UserId == userId);
        return cart;  // Return the cart for the user
    }
}
