using Microsoft.AspNetCore.Mvc;
using SmartShopAPI.Models;
using SmartShopAPI.Models.Dtos;

namespace SmartShopAPI.Services
{
    public interface ICategoryService
    {
        int Create(CreateCategoryDto dto);
        void Delete(int categoryId);
        IEnumerable<CategoryDto> GetAll();
        CategoryDto GetCategory(int categoryId);
        void Update(int categoryId, UpdateCategoryDto dto);
    }
}