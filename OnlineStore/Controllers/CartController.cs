using Microsoft.AspNetCore.Mvc;
using OnlineStoreAPI.Services;
using OnlineStoreAPI.Dto;
using OnlineStoreAPI.Models;


using Microsoft.AspNetCore.Mvc;
using OnlineStoreAPI.Services;
using OnlineStoreAPI.Dto; // Make sure this is included for CartItemDto

namespace OnlineStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        // Declare the private readonly _cartService field
        private readonly ICartService _cartService;

        // Constructor to inject ICartService
        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        // Example endpoint to add an item to the cart
        [HttpPost("add")]
        public async Task<ActionResult> AddItemToCart(int userId, CartItemDto cartItemDto)
        {
            // Use _cartService to add item to the cart
            var result = await _cartService.AddToCart(userId, cartItemDto);

            if (result == null)
            {
                return BadRequest("Unable to add item to cart.");
            }

            return Ok(result);  // Return the updated cart
        }

        // You can also add other cart methods like Update, Remove, Get Cart, etc.
    }
}
