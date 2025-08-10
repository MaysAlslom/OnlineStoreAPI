namespace OnlineStoreAPI.Models
{
    public class CartItem
    {
        public int Id { get; set; } // معرّف العنصر في السلة
        public int ProductId { get; set; } // معرّف المنتج
        public int Quantity { get; set; } // الكمية المضافة للسلة
        public int CartId { get; set; }  // Foreign Key for Cart


        // العلاقة مع المنتج
        public Product Product { get; set; }
        public Cart Cart { get; set; } // Related cart
    }

}
