using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineStoreAPI.Dto;
using OnlineStoreAPI.Models;
using OnlineStoreAPI.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStoreAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ProductService _productService;

        private readonly AppDbContext _context;


        // Inject ProductService into the controller
        public ProductsController(ProductService productService)
        {
            _productService = productService;
        }

        // Fetch paginated products with stock > 0
        [HttpGet("products")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts([FromQuery] PaginationDto paginationDto)
        {
            var products = await _productService.GetPagedProducts(paginationDto);
            return Ok(products);
        }

        // Fetch products by category
        [HttpGet("products/category/{categoryId}")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsByCategory(int categoryId)
        {
            // Get products by category from ProductService
            var products = await _productService.GetProductsByCategory(categoryId);
            if (products == null || !products.Any())
                return NotFound("No products found in this category.");

            return Ok(products);
        }

        // Search products by keyword
        [HttpGet("products/search")]
        public async Task<ActionResult<IEnumerable<Product>>> SearchProducts([FromQuery] string query)
        {
            var sanitizedQuery = query.Trim().ToLower(); // sanitize query
            var products = await _productService.SearchProducts(sanitizedQuery);
            return Ok(products);
        }

        // 1. Add a new product (Admin only)
        [HttpPost("admin/products/add")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Product>> AddProduct([FromBody] ProductDto productDto)
        {
            var category = await _context.Categories.FindAsync(productDto.CategoryId);
            if (category == null)
                return BadRequest("Category not found.");

            var existingProduct = await _context.Products
                .FirstOrDefaultAsync(p => p.Name == productDto.Name);
            if (existingProduct != null)
                return BadRequest("Product name must be unique.");

            var product = new Product
            {
                Name = productDto.Name,
                Price = productDto.Price,
                StockQuantity = productDto.StockQuantity,
                ImageUrl = productDto.ImageUrl,
                CategoryId = productDto.CategoryId
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return Ok(product);
        }






        [HttpPut("admin/products/edit/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Product>> EditProduct(int id, [FromBody] ProductDto productDto)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound("Product not found.");

            var orderItems = await _context.OrderItems
                .Where(oi => oi.ProductId == id)
                .SumAsync(oi => oi.Quantity);

            if (productDto.StockQuantity < orderItems)
                return BadRequest("Cannot reduce stock below number already ordered.");

            product.Name = productDto.Name;
            product.Price = productDto.Price;
            product.StockQuantity = productDto.StockQuantity;
            product.ImageUrl = productDto.ImageUrl;
            product.CategoryId = productDto.CategoryId;

            _context.Products.Update(product);
            await _context.SaveChangesAsync();

            return Ok(product);
        }




        // 3. Delete a product (Admin only)
        [HttpDelete("admin/products/delete/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound("Product not found.");

            var orderItems = await _context.OrderItems
                .AnyAsync(oi => oi.ProductId == id);
            if (orderItems)
                return BadRequest("Cannot delete product because it's part of an existing order.");

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent(); // Product successfully deleted
        }





        // 4. Get all products (Admin view)
        [HttpGet("admin/products")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<Product>>> GetAllProducts()
        {
            var products = await _productService.GetAllProducts();
            return Ok(products);
        }
    }
}
