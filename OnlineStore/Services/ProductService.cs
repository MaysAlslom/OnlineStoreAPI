using OnlineStoreAPI.Dto;
using OnlineStoreAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStoreAPI.Services
{
    public class ProductService
    {
        private readonly AppDbContext _context;

        // Constructor with dependency injection for AppDbContext
        public ProductService(AppDbContext context)
        {
            _context = context;
        }

        // Fetch products with stock > 0, with pagination
        public async Task<IEnumerable<Product>> GetPagedProducts(PaginationDto paginationDto)
        {
            return await _context.Products
                                 .Where(p => p.StockQuantity > 0)  // Only show products with stock > 0
                                 .Skip((paginationDto.PageNumber - 1) * paginationDto.PageSize)
                                 .Take(paginationDto.PageSize)
                                 .ToListAsync();
        }

        // Get products by category
        public async Task<IEnumerable<Product>> GetProductsByCategory(int categoryId)
        {
            return await _context.Products
                                 .Where(p => p.CategoryId == categoryId && p.StockQuantity > 0)
                                 .ToListAsync();
        }

        // Search products by keyword
        public async Task<IEnumerable<Product>> SearchProducts(string query)
        {
            // Sanitize query to ensure it's case-insensitive
            var sanitizedQuery = query.Trim().ToLower();
            return await _context.Products
                                 .Where(p => p.Name.ToLower().Contains(sanitizedQuery))  // Case-insensitive search
                                 .ToListAsync();
        }

        // Add new product
        public async Task<Product> AddProduct(ProductDto productDto)
        {
            // Validate category exists
            var category = await _context.Categories.FindAsync(productDto.CategoryId);
            if (category == null)
                return null;  // Category does not exist

            // Check for unique product name
            var existingProduct = await _context.Products
                                                 .AnyAsync(p => p.Name == productDto.Name);
            if (existingProduct)
                return null;  // Product name already exists

            var product = new Product
            {
                Name = productDto.Name,
                Price = productDto.Price,
                StockQuantity = productDto.StockQuantity,
                Description = productDto.Description,
                ImageUrl = productDto.ImageUrl,
                CategoryId = productDto.CategoryId
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();  // Save to the database

            return product;  // Return the created product
        }

        // Edit an existing product
        public async Task<Product> EditProduct(int id, ProductDto productDto)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return null;  // Product not found

            // Update product details
            product.Name = productDto.Name;
            product.Price = productDto.Price;
            product.StockQuantity = productDto.StockQuantity;
            product.Description = productDto.Description;
            product.ImageUrl = productDto.ImageUrl;
            product.CategoryId = productDto.CategoryId;

            _context.Products.Update(product);
            await _context.SaveChangesAsync();  // Save changes to the database

            return product;  // Return the updated product
        }

        // Delete a product
        public async Task<bool> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return false;  // Product not found

            // Check if product is part of an order (prevents deleting products that are already ordered)
            var orderItems = await _context.OrderItems
                                            .AnyAsync(oi => oi.ProductId == product.Id);
            if (orderItems)
                return false;  // Product is linked to an order, so it cannot be deleted

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();  // Save the deletion to the database

            return true;  // Return true if successfully deleted
        }

        // Get all products (for admin use)
        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            return await _context.Products.ToListAsync();  // Retrieve all products
        }
    }
}
