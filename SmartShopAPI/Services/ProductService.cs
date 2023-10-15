using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartShopAPI.Data;
using SmartShopAPI.Models.Dtos;
using SmartShopAPI.Models;
using SmartShopAPI.Exceptions;

namespace SmartShopAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly SmartShopDbContext _context;
        private readonly IMapper _mapper;

        public ProductService(SmartShopDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public List<ProductDto> Get(int categoryId)
        {
            var category = _context.Categories
                .Include(x => x.Products)
                .FirstOrDefault(c => c.Id == categoryId) ?? throw new NotFoundException("Category not found");
            var products = _mapper.Map<List<ProductDto>>(category.Products.ToList());
            return products;
        }
        public ProductDto GetById(int categoryId, int productId)
        {
            var category = _context.Categories
                .Include(x => x.Products)
                .FirstOrDefault(c => c.Id == categoryId) ?? throw new NotFoundException("Category not found");
            var product = category.Products
                .FirstOrDefault(x => x.Id == productId) ?? throw new NotFoundException("Product not found");
            var productDto = _mapper.Map<ProductDto>(product);
            return productDto;
        }
        public int Create(int categoryId, CreateProductDto dto)
        {
            var category = _context.Categories
                .FirstOrDefault(x => x.Id == categoryId) ?? throw new NotFoundException("Category not found");
            var product = _mapper.Map<Product>(dto);
            product.CategoryId = categoryId;
            _context.Products.Add(product);
            _context.SaveChanges();
            return product.Id;
        }
        public void Delete(int categoryId, int productId)
        {
            var category = _context.Categories
                .FirstOrDefault(c => c.Id == categoryId) ?? throw new NotFoundException("Category not found");
            var product = _context.Products
                .FirstOrDefault(p => p.Id == productId) ?? throw new NotFoundException("Product not found");
            _context.Products.Remove(product);
            _context.SaveChanges();
        }
        public void Update(int productId, UpdateProductDto dto)
        {
            var product = _context.Products
                .FirstOrDefault(x => x.Id == productId) ?? throw new NotFoundException("Product not found");
            _mapper.Map(dto, product);
            _context.SaveChanges();
        }
    }
}
