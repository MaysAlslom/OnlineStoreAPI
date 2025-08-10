using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace OnlineStoreAPI.Models
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Categories> Categories { get; set; }
        public DbSet<Orders> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }

        public DbSet<Users> Users { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Specify precision and scale for the 'Price' in OrderItem
            modelBuilder.Entity<OrderItem>()
                .Property(oi => oi.Price)
                .HasColumnType("decimal(18,2)"); // 18 digits, 2 decimal places

            // Specify precision and scale for 'TotalAmount' in Orders
            modelBuilder.Entity<Orders>()
                .Property(o => o.TotalAmount)
                .HasColumnType("decimal(18,2)"); // 18 digits, 2 decimal places

            // You can also apply it for other decimal properties like 'Price' in Products
            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)");

            base.OnModelCreating(modelBuilder);

            // Configure the primary key for IdentityUserLogin
            modelBuilder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.HasKey(e => e.UserId);
            });

            // Configure the primary key for IdentityUserRole
            modelBuilder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });
            });

            // Configure the primary key for IdentityUserClaim
            modelBuilder.Entity<IdentityUserClaim<string>>(entity =>
            {
                entity.HasKey(e => e.Id);
            });

            // Optionally, configure other Identity tables if needed (e.g., IdentityUserToken)
            modelBuilder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });
            });
        }
    }
}




