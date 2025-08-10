namespace OnlineStoreAPI.Models
{
    public class Categories
    {
        public int Id { get; set; } // معرّف الفئة
        public string Name { get; set; } // اسم الفئة

        // العلاقة مع المنتجات
        public List<Product> Products { get; set; }
    }

}
