namespace OnlineStoreAPI.Models
{
    public class Product
    {
        public int Id { get; set; } // معرّف المنتج
        public string Name { get; set; } // اسم المنتج
        public decimal Price { get; set; } // سعر المنتج
        public int StockQuantity { get; set; } // الكمية المتوفرة من المنتج
        public string Description { get; set; } // وصف المنتج
        public string ImageUrl { get; set; } // رابط الصورة (يمكن استخدامه لتخزين صورة المنتج)
        public int CategoryId { get; set; } // معرّف الفئة التي ينتمي إليها المنتج

        // العلاقة مع الفئة
        public Categories Categories{ get; set; }
    }

}
