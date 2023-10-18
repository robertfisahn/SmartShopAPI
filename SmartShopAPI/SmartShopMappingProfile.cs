using AutoMapper;
using SmartShopAPI.Entities;
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
            CreateMap<User, UserDto>()
                .ForMember(r=>r.RoleName, d=>d.MapFrom(u=>u.Role.Name))
                .ForMember(r => r.City, d=> d.MapFrom(u=> u.Address.City))
                .ForMember(r => r.Street, d => d.MapFrom(u => u.Address.Street))
                .ForMember(r => r.PostalCode, d => d.MapFrom(u => u.Address.PostalCode));
            CreateMap<RegisterUserDto, User>()
                .ForPath(u => u.Address.City, d => d.MapFrom(r => r.City))
                .ForPath(u => u.Address.Street, d => d.MapFrom(r => r.Street))
                .ForPath(u => u.Address.PostalCode, d => d.MapFrom(r => r.PostalCode));
        }
    }
}
