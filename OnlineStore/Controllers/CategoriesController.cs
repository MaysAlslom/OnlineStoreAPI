using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineStoreAPI.Dto;
using OnlineStoreAPI.Models;
using OnlineStoreAPI.Services;

namespace OnlineStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly AppDbContext _context;  // Add this line
        private readonly ICategoriesService _categoryService;

        public CategoriesController(AppDbContext context, ICategoriesService categoryService)
        {
            _context = context;  // Initialize _context
            _categoryService = categoryService;
        }


        // 1. Create a category
        [HttpPost("admin/categories/add")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Categories>> CreateCategory([FromBody] CategoriesDto CategoriesDto)
        {
            if (await _context.Categories.AnyAsync(c => c.Name == CategoriesDto.Name))
                return BadRequest("Category name must be unique.");

            var category = new Categories
            {
                Name = CategoriesDto.Name
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return Ok(category);
        }




        // 2. Edit a category
        [HttpPut("admin/categories/edit/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Categories>> EditCategory(int id, [FromBody] CategoriesDto CategoriesDto)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return NotFound("Category not found.");

            category.Name = CategoriesDto.Name;

            _context.Categories.Update(category);
            await _context.SaveChangesAsync();

            return Ok(category);
        }


        // 3. Delete a category
        [HttpDelete("admin/categories/delete/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return NotFound("Category not found.");

            // Prevent deletion if the category has products linked to it
            var products = await _context.Products.AnyAsync(p => p.CategoryId == id);
            if (products)
                return BadRequest("Cannot delete category because it is linked to products.");

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return NoContent(); // Category successfully deleted
        }

    }

}

