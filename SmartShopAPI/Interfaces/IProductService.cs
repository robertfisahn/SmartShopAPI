using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SmartShopAPI.Models.Dtos;
using SmartShopAPI.Models.Dtos.Product;

namespace SmartShopAPI.Interfaces
{
    public interface IProductService
    {
        int Create(int categoryId, CreateProductDto dto);
        void Delete(int categoryId, int productId);
        PagedResult<ProductDto> Get(int categoryId, QueryParams query);
        ProductDto GetById(int categoryId, int productId);
        void Update(int productId, UpdateProductDto dto);
    }
}