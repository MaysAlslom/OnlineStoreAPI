namespace OnlineStoreAPI.Dto
{
    public class CheckoutDto
    {
        public string CustomerName { get; set; }
        public string ShippingAddress { get; set; }
        public decimal TotalAmount { get; set; }
        public List<OrderItemDto> OrderItems { get; set; }
    }


    public class OrderItemDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }

}
