using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStoreAPI.Dto;
using OnlineStoreAPI.Models;
using OnlineStoreAPI.Services;

namespace OnlineStoreAPI.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoriesService _categoryService;

        public CategoriesController(ICategoriesService categoryService)
        {
            _categoryService = categoryService;
        }

        // 1. Create a category
        [HttpPost("admin/create")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Categories>> CreateCategory([FromBody] CategoriesDto categoryDto)
        {
            var result = await _categoryService.CreateCategory(categoryDto);
            if (result == null)
                return BadRequest("Category could not be created.");

            return Ok(result); // Return created category
        }

        // 2. Edit a category
        [HttpPut("admin/edit/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Categories>> EditCategory(int id, [FromBody] CategoriesDto categoryDto)
        {
            var result = await _categoryService.EditCategory(id, categoryDto);
            if (result == null)
                return NotFound("Category not found.");

            return Ok(result); // Return updated category
        }

        // 3. Delete a category
        [HttpDelete("admin/delete/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var result = await _categoryService.DeleteCategory(id);
            if (!result)
                return BadRequest("Category cannot be deleted because it has products linked to it.");

            return NoContent(); // Success, category deleted
        }
    }

}

