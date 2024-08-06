using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SmartShopAPI.Models.Dtos;
using SmartShopAPI.Models.Dtos.Product;

namespace SmartShopAPI.Interfaces
{
    public interface IProductService
    {
        Task<int> CreateAsync(int categoryId, CreateProductDto dto);
        Task DeleteAsync(int categoryId, int productId);
        Task<PagedResult<ProductDto>> GetAsync(int categoryId, QueryParams query);
        Task<ProductDto> GetByIdAsync(int categoryId, int productId);
        Task UpdateAsync(int productId, UpdateProductDto dto);
    }
}