using OnlineStoreAPI.Dto;
using OnlineStoreAPI.Models;

namespace OnlineStoreAPI.Services
{
    public interface ICategoriesService
    {
        Task<Categories> CreateCategory(CategoriesDto categoryDto);
        Task<Categories> EditCategory(int id, CategoriesDto categoryDto);
        Task<bool> DeleteCategory(int id);
    }

}
