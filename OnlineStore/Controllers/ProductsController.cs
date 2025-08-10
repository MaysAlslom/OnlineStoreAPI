using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStoreAPI.Models;
using OnlineStoreAPI.Dto;


[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly ProductService _productService;

    public ProductsController(ProductService productService)
    {
        _productService = productService;
    }

    [HttpGet("products")]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts([FromQuery] PaginationDto paginationDto)
    {
        var products = await _productService.GetPagedProducts(paginationDto);
        return Ok(products);
    }



    // 1. Add a new product
    [HttpPost("admin/add")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<Product>> AddProduct([FromBody] ProductDto productDto)
    {
        // Validate product and category existence
        var result = await _productService.AddProduct(productDto);
        if (result == null)
            return BadRequest("Product could not be added. Check your data.");

        return Ok(result); // Returns the added product
    }

    // 2. Edit an existing product
    [HttpPut("admin/edit/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<Product>> EditProduct(int id, [FromBody] ProductDto productDto)
    {
        var result = await _productService.EditProduct(id, productDto);
        if (result == null)
            return NotFound("Product not found or data invalid.");

        return Ok(result); // Returns the updated product
    }

    // 3. Delete a product
    [HttpDelete("admin/delete/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var result = await _productService.DeleteProduct(id);
        if (!result)
            return NotFound("Product not found or product is linked to an order.");

        return NoContent(); // Success, product deleted
    }

    // Optionally: 4. Get all products (for admin view)
    [HttpGet("admin/products")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<Product>>> GetAllProducts()
    {
        var products = await _productService.GetAllProducts();
        return Ok(products);
    }


}
