using Moq;
using AutoMapper;
using SmartShopAPI.Models;
using SmartShopAPI.Models.Dtos.Product;
using SmartShopAPI.Services;
using SmartShopAPI.Tests.Helpers;
using SmartShopAPI.Data;
using SmartShopAPI.Exceptions;
using SmartShopAPI.Models.Dtos;

namespace SmartShopAPI.Tests
{
    public class ProductServiceUnitTests
    {
        private readonly Mock<SmartShopDbContext> _mockContext;
        private readonly Mock<IMapper> _mockMapper;
        private readonly ProductService _service;
        private readonly List<Product> _products;

        public ProductServiceUnitTests()
        {
            _mockContext = DbContextMocking.CreateMockDbContext();
            _mockMapper = new Mock<IMapper>();
            _service = new ProductService(_mockContext.Object, _mockMapper.Object);

            var categories = new List<Category> { new Category { Id = 1, Name = "Electronics" } };

            _products = new List<Product>
            {
                new Product { Id = 0, Name = "Test Product", CategoryId = 1, Price = 100 },
                new Product { Id = 1, Name = "Laptop", CategoryId = 1, Price = 200 },
                new Product { Id = 2, Name = "Desktop", CategoryId = 1, Price = 1000 },
                new Product { Id = 3, Name = "Mobile", CategoryId = 1, Price = 400}
            };

            var mockProducts = DbContextMocking.CreateMockDbSet(_products);
            var mockCategories = DbContextMocking.CreateMockDbSet(categories);

            _mockContext.Setup(c => c.Products).Returns(mockProducts);
            _mockContext.Setup(c => c.Categories).Returns(mockCategories);

            _mockMapper.Setup(m => m.Map<ProductDto>(It.IsAny<Product>()))
                .Returns((Product source) => new ProductDto { Id = source.Id, Name = source.Name });

            _mockMapper.Setup(m => m.Map<Product>(It.IsAny<CreateProductDto>()))
                .Returns((CreateProductDto dto) => new Product { Name = dto.Name, CategoryId = 1 });

            _mockMapper.Setup(m => m.Map(It.IsAny<UpdateProductDto>(), It.IsAny<Product>()))
                .Callback((UpdateProductDto dto, Product product) => {
                    product.Name = dto.Name;
                });
        }

        [Fact]
        public void CheckCategory_ExistingCategory_DoesNotThrowException()
        {
            // arrange
            var existingCategory = 1;

            // act
            var exception = Record.Exception(() => _service.CheckCategory(existingCategory));

            // assert
            Assert.Null(exception);
        }

        [Fact]
        public void CheckCategory_NonExistingCategory_ThrowsNotFoundException()
        {
            // arrange
            var nonExistingCategory = 11;

            // act & assert
            Assert.Throws<NotFoundException>(() => _service.CheckCategory(nonExistingCategory));
        }

        [Fact]
        public void GetById_ExistingProduct()
        {
            var existingProduct = _service.GetById(1, 0);
            Assert.NotNull(existingProduct);
            Assert.Equal(0, existingProduct.Id);
        }

        [Fact]
        public void GetById_NonExistingProduct_ThrowsNotFoundException()
        {
            var nonExistingProductId = 99;
            Assert.Throws<NotFoundException>(() => _service.GetById(1, nonExistingProductId));
        }

        [Fact]
        public void Create_Product_Successfully()
        {
            var newProduct = new CreateProductDto { Name = "New Laptop" };
            var newProductId = _service.Create(1, newProduct);
            Assert.NotNull(_service.GetById(1, newProductId));
        }

        [Fact]
        public void Create_Product_NonExistingCategory_ThrowsNotFoundException()
        {
            var dto = new CreateProductDto { Name = "exc" };
            Assert.Throws<NotFoundException>(() => _service.Create(0, dto));
        }

        [Fact]
        public void Delete_Product_Successfully()
        {
            _service.Delete(1, 0);
            var productExists = _mockContext.Object.Products.Any(p => p.Id == 0);
            Assert.False(productExists);
        }

        [Fact]
        public void Delete_Product_NonExistingProduct_ThrowsNotFoundException()
        {
            var nonExistingProductId = 99;
            Assert.Throws<NotFoundException>(() => _service.Delete(0, nonExistingProductId));
        }

        [Fact]
        public void Update_Product_Successfully()
        {
            var dto = new UpdateProductDto { Name = "update product" };
            _service.Update(0, dto);
            var updateProduct = _mockContext.Object.Products.FirstOrDefault(p => p.Id == 0);
            Assert.Equal("update product", updateProduct.Name);
        }

        [Fact]
        public void Update_Product_NonExistingProduct_ThrowsNotFoundException()
        {
            // arrange
            var nonExistingProductId = 99;
            var dto = new UpdateProductDto { Name = "Update Product" };

            // act & assert
            Assert.Throws<NotFoundException>(() => _service.Update(nonExistingProductId, dto));
        }

        [Fact]
        public void FilterProducts_ReturnsFilteredProducts()
        {
            var result = _service.FilterProducts(1, "Mob").ToList();

            Assert.Single(result);
            Assert.Equal("Mobile", result[0].Name);
        }

        [Fact]
        public void PaginateProducts_PaginatesCorrectly()
        {
            int pageNumber = 2;
            int pageSize = 2;

            var paginated = _service.PaginateProducts(_products.AsQueryable(), pageNumber, pageSize);

            Assert.Equal(2, paginated.Count); 
            Assert.Equal("Desktop", paginated[0].Name);
            Assert.Equal("Mobile", paginated[1].Name); 
        }

        [Fact]
        public void SortProducts_ByNameAscending()
        {
            var sortBy = "Name";
            var sortOrder = SortOrder.Ascending;

            var sort = _service.SortProducts(_products.AsQueryable(), sortOrder, sortBy).ToList();
            Assert.Equal("Desktop", sort[0].Name);
            Assert.Equal("Test Product", sort[3].Name);
        }

        [Fact]
        public void SortProducts_ByPriceAscending()
        {
            var sortBy = "Price";
            var sortOrder = SortOrder.Ascending;

            var sort = _service.SortProducts(_products.AsQueryable(), sortOrder, sortBy).ToList();
            Assert.Equal("Test Product", sort[0].Name);
            Assert.Equal("Desktop", sort[3].Name);
        }

        [Fact]
        public void SortProducts_ByPriceDescending()
        {
            var sortBy = "Price";
            var sortOrder = SortOrder.Descending;

            var sort = _service.SortProducts(_products.AsQueryable(), sortOrder, sortBy).ToList();
            Assert.Equal("Desktop", sort[0].Name);
            Assert.Equal("Test Product", sort[3].Name);
        }

        [Fact]
        public void SortProducts_ByNameDescending()
        {
            var sortBy = "Name";
            var sortOrder = SortOrder.Descending;

            var sort = _service.SortProducts(_products.AsQueryable(), sortOrder, sortBy).ToList();
            Assert.Equal("Test Product", sort[0].Name);
            Assert.Equal("Mobile", sort[1].Name);
        }

        [Fact]
        public void SortProducts_NonExistingAttribute_ThrowsNotFoundException()
        {
            var nonExistingSortBy = "xxx";
            var sortOrder = SortOrder.Ascending;
            Assert.Throws<NotFoundException>(() => _service.SortProducts(_products.AsQueryable(), sortOrder, nonExistingSortBy));
        }
    }
}
