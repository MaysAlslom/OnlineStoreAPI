using OnlineStoreAPI.Dto;
using OnlineStoreAPI.Models;

namespace OnlineStoreAPI.Services
{
    public interface IProductService
    {
        Task<Product> AddProduct(ProductDto productDto);
        Task<Product> EditProduct(int id, ProductDto productDto);
        Task<bool> DeleteProduct(int id);
        Task<IEnumerable<Product>> GetAllProducts();
    }

}
