namespace OnlineStoreAPI.Dto
{
    public class UpdateCartItemDto
    {
        public int UserId { get; set; }  // Added UserId
        public int CartItemId { get; set; }
        public int Quantity { get; set; }
    }
}

