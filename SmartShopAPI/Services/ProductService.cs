﻿using AutoMapper;
using SmartShopAPI.Data;
using SmartShopAPI.Models;
using SmartShopAPI.Exceptions;
using SmartShopAPI.Models.Dtos.Product;
using SmartShopAPI.Interfaces;
using SmartShopAPI.Models.Dtos;
using Microsoft.EntityFrameworkCore;

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

        public async Task<PagedResult<ProductDto>> GetAsync(int categoryId, QueryParams query)
        {
            await CheckCategory(categoryId);

            var filteredProducts = await FilterProducts(categoryId, query.SearchPhrase);
            var paginatedAndSortedProducts = PaginateProducts(SortProducts(filteredProducts, query.SortOrder, query.SortBy),
                query.PageSize, query.PageNumber);

            var dtos = _mapper.Map<List<ProductDto>>(paginatedAndSortedProducts);
            return new PagedResult<ProductDto>(dtos, filteredProducts.Count(), query.PageSize, query.PageNumber);
        }

        public async Task<ProductDto> GetByIdAsync(int categoryId, int productId)
        {
            await CheckCategory(categoryId);
            var product = await _context.Products
                .Where(x => x.CategoryId == categoryId)
                .FirstOrDefaultAsync(x => x.Id == productId) ?? throw new NotFoundException("Product not found");

            return _mapper.Map<ProductDto>(product);
        }

        public async Task<int> CreateAsync(int categoryId, CreateProductDto dto)
        {
            await CheckCategory(categoryId);
            var product = _mapper.Map<Product>(dto);
            product.CategoryId = categoryId;
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return product.Id;
        }

        public async Task DeleteAsync(int categoryId, int productId)
        {
            await CheckCategory(categoryId);
            var product = await _context.Products
                .Where(x => x.CategoryId == categoryId)
                .FirstOrDefaultAsync(p => p.Id == productId) ?? throw new NotFoundException("Product not found");
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(int productId, UpdateProductDto dto)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(x => x.Id == productId) ?? throw new NotFoundException("Product not found");
            _mapper.Map(dto, product);
            await _context.SaveChangesAsync();
        }

        public async Task CheckCategory(int categoryId)
        {
            if(!await _context.Categories.AnyAsync(x => x.Id == categoryId))
            {
                throw new NotFoundException("Category not found");
            }
        }

        public async Task<List<Product>> FilterProducts(int categoryId, string? searchPhrase)
        {
            var products = await _context.Products
                .Where(x => x.CategoryId == categoryId && (searchPhrase == null || x.Name.ToLower().Contains(searchPhrase.ToLower())))
                .ToListAsync();
            return products;
        }

        public List<Product> SortProducts(List<Product> products, SortOrder sortOrder, string sortBy)
        {
            if (!string.IsNullOrEmpty(sortBy))
            {
                var columnSelector = new Dictionary<string, Func<Product, object>>
        {
            { nameof(Product.Name), r => r.Name },
            { nameof(Product.Price), r => r.Price }
        };

                var selectedColumn = columnSelector[sortBy];

                products = sortOrder == SortOrder.Ascending
                    ? products.OrderBy(selectedColumn).ToList()
                    : products.OrderByDescending(selectedColumn).ToList();
            }
            return products;
        }

        public List<Product> PaginateProducts(List<Product> products, int pageSize, int pageNumber)
        {
            var result = products
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize).ToList();
            return result;
        }
    }
}
