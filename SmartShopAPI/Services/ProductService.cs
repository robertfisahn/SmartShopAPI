using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartShopAPI.Data;
using SmartShopAPI.Models;
using SmartShopAPI.Exceptions;
using SmartShopAPI.Models.Dtos.Product;
using SmartShopAPI.Interfaces;
using SmartShopAPI.Models.Dtos;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
        public PagedResult<ProductDto> Get(int categoryId, QueryParams query)
        {
            CheckCategory(categoryId);
            var filteredProducts = FilterProducts(categoryId, query.SearchPhrase);
            var totalItemsCount = filteredProducts.Count();
            var sortProducts = SortProducts(filteredProducts, query.SortOrder, query.SortBy);
            var paginateProducts = PaginateProducts(sortProducts, query.PageSize, query.PageNumber);
            var dtos = _mapper.Map<List<ProductDto>>(paginateProducts);
            var result =  new PagedResult<ProductDto>(dtos, totalItemsCount, query.PageSize, query.PageNumber);
            return result;
        }
        public ProductDto GetById(int categoryId, int productId)
        {
            CheckCategory(categoryId);
            var product = _context.Products
                .Where(x => x.CategoryId == categoryId)
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
            CheckCategory(categoryId);
            var product = _context.Products
                .Where(x => x.CategoryId == categoryId)
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
        public void CheckCategory(int categoryId)
        {
            if(!_context.Categories.Any(x => x.Id == categoryId))
            {
                throw new NotFoundException("Category not found");
            }
        }
        public IQueryable<Product> FilterProducts(int categoryId, string searchPhrase)
        {
            var products = _context.Products
                .Where(x => x.CategoryId == categoryId && (searchPhrase == null || x.Name
                .Contains(searchPhrase, StringComparison.OrdinalIgnoreCase)));
            return products;
        }
        public IQueryable<Product> SortProducts(IQueryable<Product> products, SortOrder sortOrder, string sortBy)
        {
            if (!string.IsNullOrEmpty(sortBy))
            {
                var columnSelector = new Dictionary<string, Expression<Func<Product, object>>>
                {
                    { nameof(Product.Name), r => r.Name },
                    { nameof(Product.Price), r => r.Price }
                };

                if (columnSelector.TryGetValue(sortBy, out var selectedColumn))
                {
                    products = sortOrder == SortOrder.Ascending
                        ? products.OrderBy(selectedColumn)
                        : products.OrderByDescending(selectedColumn);
                }
                else
                {
                    var availableAttributes = string.Join(", ", columnSelector.Keys);
                    throw new NotFoundException($"Attribute '{sortBy}' not found. Available attributes: {availableAttributes}.");
                }
            }
            return products;
        }
        public List<Product> PaginateProducts(IQueryable<Product> products, int pageSize, int pageNumber)
        {
            var result = products
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize).ToList();
            return result;
        }
    }
}
