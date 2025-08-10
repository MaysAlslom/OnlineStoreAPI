namespace OnlineStoreAPI.Models
{
    public class Users
    {

        public int Id { get; set; } // معرّف المستخدم
        public string UserName { get; set; } // اسم المستخدم
        public string Email { get; set; } // البريد الإلكتروني
        public string PasswordHash { get; set; } // كلمة المرور المشفرة
        public string Role { get; set; } // دور المستخدم (مثل "Admin" أو "Customer")

        // العلاقة مع الطلبات
        public List<Orders> Orders { get; set; }
    }
}
