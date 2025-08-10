using Microsoft.AspNetCore.Mvc;
using OnlineStoreAPI.Dto;
using OnlineStoreAPI.Services;

namespace OnlineStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        // Declare a private readonly field for OrderService
        private readonly IOrderService _orderService;

        // Inject the OrderService via the constructor
        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService; // Assign the injected service to the field
        }

        // Checkout API endpoint
        [HttpPost("checkout")]
        public async Task<ActionResult> Checkout(int userId, [FromBody] CheckoutDto checkoutDto)
        {
            // Call PlaceOrder method on the service
            var order = await _orderService.PlaceOrder(userId, checkoutDto);
            return Ok(order);
        }
    }
}
