using Microsoft.AspNetCore.Mvc;
using OnlineStoreAPI.Dto;
using OnlineStoreAPI.Models;
using OnlineStoreAPI.Services;
using System.Threading.Tasks;

namespace OnlineStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpPost("cart/add")]
        public async Task<ActionResult> AddItemToCart([FromBody] CartItemDto cartItemDto)
        {
            var result = await _cartService.AddToCart(cartItemDto.UserId, cartItemDto);
            if (result == null)
                return BadRequest("Product not available or quantity exceeds stock.");

            return Ok(result);
        }




        [HttpPut("cart/update")]
        public async Task<ActionResult> UpdateCartItem([FromBody] UpdateCartItemDto updateCartItemDto)
        {
            var result = await _cartService.UpdateCart(updateCartItemDto.UserId, updateCartItemDto.CartItemId, updateCartItemDto.Quantity);
            if (result == null)
                return BadRequest("Invalid cart item ID or insufficient stock.");

            return Ok(result);
        }


        [HttpDelete("cart/remove/{cartItemId}")]
        public async Task<ActionResult> RemoveItemFromCart(int cartItemId, [FromQuery] int userId)
        {
            var result = await _cartService.RemoveFromCart(userId, cartItemId);
            if (result == null)
                return NotFound("Cart item not found.");

            return NoContent();
        }



        [HttpGet("cart")]
        public async Task<ActionResult<Cart>> GetCart(int userId)
        {
            var cart = await _cartService.GetCart(userId);
            if (cart == null)
                return NotFound("Cart not found.");

            return Ok(cart);
        }
    }
}
