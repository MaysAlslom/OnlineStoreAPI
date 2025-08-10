using OnlineStoreAPI.Dto;
using OnlineStoreAPI.Models;

using Microsoft.EntityFrameworkCore;

namespace OnlineStoreAPI.Services
{
    public class CategoryService : ICategoriesService
    {
        private readonly AppDbContext _context;

        public CategoryService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Categories> CreateCategory(CategoriesDto categoryDto)
        {
            var existingCategory = await _context.Categories
                .AnyAsync(c => c.Name == categoryDto.Name);
            if (existingCategory) return null;

            var category = new Categories
            {
                Name = categoryDto.Name
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return category; // Return created category
        }

        public async Task<Categories> EditCategory(int id, CategoriesDto categoryDto)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return null;

            category.Name = categoryDto.Name;
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();

            return category; // Return updated category
        }

        public async Task<bool> DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return false;

            // Check if category has products linked to it
            var products = await _context.Products
                .AnyAsync(p => p.CategoryId == category.Id);
            if (products) return false;

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return true; // Return true if deleted
        }
    }

}
