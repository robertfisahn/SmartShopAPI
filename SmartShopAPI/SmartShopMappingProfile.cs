using AutoMapper;
using SmartShopAPI.Models;
using SmartShopAPI.Models.Dtos;

namespace SmartShopAPI
{
    public class SmartShopMappingProfile : Profile
    {
        public SmartShopMappingProfile()
        {
            CreateMap<Category, CategoryDto>();
            CreateMap<UpdateCategoryDto, Category>();

            CreateMap<CreateCategoryDto, Category>();
            CreateMap<CreateProductDto, Product>();
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name));
            CreateMap<ProductDto, Product>();
            CreateMap<UpdateProductDto, Product>();
        }
    }
}
