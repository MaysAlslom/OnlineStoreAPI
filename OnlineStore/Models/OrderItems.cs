namespace OnlineStoreAPI.Models
{
    public class OrderItem
    {
        public int Id { get; set; } // معرّف العنصر
        public int ProductId { get; set; } // معرّف المنتج
        public int Quantity { get; set; } // كمية المنتج في الطلب
        public decimal Price { get; set; } // سعر المنتج (decimal للحد من الدقة المفقودة في الأرقام)

        // العلاقة مع الطلب
        public int OrderId { get; set; } // معرّف الطلب (يجب أن يكون int)
        public Orders Order { get; set; } // العلاقة مع الطلب

        // العلاقة مع المنتج
        public Product Product { get; set; } // العلاقة مع المنتج
    }
}

