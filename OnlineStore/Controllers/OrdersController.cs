using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineStoreAPI.Dto;
using OnlineStoreAPI.Models;
using OnlineStoreAPI.Services;
using System.Security.Claims;

namespace OnlineStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        // Declare a private readonly field for OrderService
        private readonly IOrderService _orderService;
        private readonly AppDbContext _context;


        // Inject the OrderService via the constructor
        public OrdersController(AppDbContext context, IOrderService orderService)
        {
            _context = context;
            _orderService = orderService;
        }


        // Method to get the current user's ID from JWT token

        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (int.TryParse(userIdClaim, out var userId))
            {
                return userId;  // Successfully parsed the user ID as int
            }

            // Return an invalid user ID if parsing fails (this should not happen if token is valid)
            throw new UnauthorizedAccessException("Invalid user ID in token.");
        }



        // Checkout API endpoint
        [HttpPost("checkout")]
        public async Task<ActionResult> Checkout(int userId, [FromBody] CheckoutDto checkoutDto)
        {
            // Call PlaceOrder method on the service
            var order = await _orderService.PlaceOrder(userId, checkoutDto);
            return Ok(order);
        }



        [HttpGet("order/history")]
        public async Task<ActionResult<IEnumerable<Orders>>> GetOrderHistory()
        {
            var userId = GetCurrentUserId();
            var orders = await _orderService.GetOrdersByUser(userId);
            return Ok(orders.OrderByDescending(o => o.OrderDate)); // Sorted by date desc
        }



        [HttpGet("admin/orders")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<Orders>>> GetAllOrders([FromQuery] OrderFilterDto filterDto)
        {
            var orders = await _orderService.GetAllOrders(filterDto);
            return Ok(orders);
        }




    }
}
