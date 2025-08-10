namespace OnlineStoreAPI.Models
{
    public class Orders
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public DateTime OrderDate { get; set; }
        public string ShippingAddress { get; set; }

        // Change from int to decimal
        public decimal TotalAmount { get; set; }

        public int UserId { get; set; }
        public Users User { get; set; }
        public List<OrderItem> OrderItems { get; set; }
    }
}

