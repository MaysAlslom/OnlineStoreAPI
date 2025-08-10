namespace OnlineStoreAPI.Models
{
    public class Cart
    {
        public int Id { get; set; } // معرّف السلة
        public int UserId { get; set; } // معرّف المستخدم

        // قائمة العناصر الموجودة في السلة (قد تكون في الذاكرة)
        public List<CartItem> CartItems { get; set; } = new List<CartItem>();
    }

}
