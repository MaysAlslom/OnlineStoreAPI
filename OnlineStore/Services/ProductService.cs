using OnlineStoreAPI.Dto;
using OnlineStoreAPI.Models;
using OnlineStoreAPI.Services;
using Microsoft.EntityFrameworkCore;






public class ProductService : IProductService
{
    private readonly AppDbContext _context;

    public ProductService(AppDbContext context)
    {
        _context = context;
    }


    // 1. Method to fetch paginated products
    public async Task<IEnumerable<Product>> GetPagedProducts(PaginationDto paginationDto)
    {
        // Use LINQ to fetch products with pagination logic
        return await _context.Products
            .Skip((paginationDto.PageNumber - 1) * paginationDto.PageSize)  // Skip records based on page number
            .Take(paginationDto.PageSize)  // Take the specified number of products per page
            .ToListAsync();
    }


    public async Task<Product> AddProduct(ProductDto productDto)
    {
        // Validate category exists
        var category = await _context.Categories.FindAsync(productDto.CategoryId);
        if (category == null) return null;

        // Check for unique product name
        var existingProduct = await _context.Products
            .AnyAsync(p => p.Name == productDto.Name);
        if (existingProduct) return null;

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
        await _context.SaveChangesAsync();

        return product; // Return the created product
    }

    public async Task<Product> EditProduct(int id, ProductDto productDto)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null) return null;

        product.Name = productDto.Name;
        product.Price = productDto.Price;
        product.StockQuantity = productDto.StockQuantity;
        product.Description = productDto.Description;
        product.ImageUrl = productDto.ImageUrl;
        product.CategoryId = productDto.CategoryId;

        _context.Products.Update(product);
        await _context.SaveChangesAsync();

        return product; // Return updated product
    }

    public async Task<bool> DeleteProduct(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null) return false;

        // Check if product is part of an order
        var orderItems = await _context.OrderItems
            .AnyAsync(oi => oi.ProductId == product.Id);
        if (orderItems) return false;

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();

        return true; // Return true if deleted
    }

    public async Task<IEnumerable<Product>> GetAllProducts()
    {
        return await _context.Products.ToListAsync();
    }
}
