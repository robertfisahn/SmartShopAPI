using Moq;
using AutoMapper;
using SmartShopAPI.Models;
using SmartShopAPI.Models.Dtos.Product;
using SmartShopAPI.Services;
using SmartShopAPI.Tests.Helpers;
using SmartShopAPI.Data;
using SmartShopAPI.Exceptions;
using SmartShopAPI.Models.Dtos;

namespace SmartShopAPI.Tests.UnitTests.ServiceTests
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
                .Callback((UpdateProductDto dto, Product product) =>
                {
                    product.Name = dto.Name;
                });
            _mockMapper.Setup(m => m.Map<List<ProductDto>>(It.IsAny<IEnumerable<Product>>()))
                .Returns((IEnumerable<Product> source) => source.Select(p => new ProductDto { Id = p.Id, Name = p.Name }).ToList());
        }

        [Fact]
        public void GetProducts_Succesfully()
        {
            var queryParams = new QueryParams()
            {
                PageNumber = 1,
                PageSize = 10,
                SearchPhrase = "op",
                SortBy = "Price",
                SortOrder = SortOrder.Descending
            };
            var products = _service.Get(1, queryParams);
            Assert.Equal(2, products.TotalCount);
            Assert.Equal("Desktop", products.Items[0].Name);
        }

        [Fact]
        public void CheckCategory_ExistingCategory_DoesNotThrowException()
        {
            var existingCategory = 1;
            var exception = Record.Exception(() => _service.CheckCategory(existingCategory));
            Assert.Null(exception);
        }

        [Fact]
        public void CheckCategory_NonExistingCategory_ThrowsNotFoundException()
        {
            var nonExistingCategory = 11;
            Assert.Throws<NotFoundException>(() => _service.CheckCategory(nonExistingCategory));
        }

        [Fact]
        public void GetById_ExistingProduct()
        {
            var existingProduct = _service.GetById(1, 0);
            Assert.Equal(0, existingProduct.Id);
        }

        [Fact]
        public void GetById_NonExistingProductId_ThrowsNotFoundException()
        {
            var nonExistingProductId = 99;
            Assert.Throws<NotFoundException>(() => _service.GetById(1, nonExistingProductId));
        }

        [Fact]
        public void GetById_NonExistingCategoryId()
        {
            var nonExistingCategoryId = 99;
            Assert.Throws<NotFoundException>(() => _service.GetById(nonExistingCategoryId, 0));
        }

        [Fact]
        public void Create_Product_Successfully()
        {
            var newProduct = new CreateProductDto { Name = "New Laptop" };
            var newProductId = _service.Create(1, newProduct);
            Assert.NotNull(_service.GetById(1, newProductId));
            _mockContext.Verify(c => c.Products.Add(It.IsAny<Product>()), Times.Once);
            _mockContext.Verify(c => c.SaveChanges(), Times.Once());
        }

        [Fact]
        public void Create_Product_NonExistingCategory_ThrowsNotFoundException()
        {
            var dto = new CreateProductDto { Name = "exc" };
            Assert.Throws<NotFoundException>(() => _service.Create(0, dto));
            _mockContext.Verify(c => c.Products.Add(It.IsAny<Product>()), Times.Never);
            _mockContext.Verify(c => c.SaveChanges(), Times.Never());
        }

        [Fact]
        public void Delete_Product_Successfully()
        {
            _service.Delete(1, 0);
            var productExists = _mockContext.Object.Products.Any(p => p.Id == 0);
            Assert.False(productExists);
            _mockContext.Verify(c => c.Products.Remove(It.Is<Product>(p => p.Id == 0)), Times.Once());
            _mockContext.Verify(c => c.SaveChanges(), Times.Once());
        }

        [Fact]
        public void Delete_Product_NonExistingProduct_ThrowsNotFoundException()
        {
            var nonExistingProductId = 99;
            Assert.Throws<NotFoundException>(() => _service.Delete(0, nonExistingProductId));
            _mockContext.Verify(c => c.Products.Remove(It.Is<Product>(p => p.Id == 99)), Times.Never());
            _mockContext.Verify(c => c.SaveChanges(), Times.Never());
        }

        [Fact]
        public void Delete_Product_NonExistingCategory_ThrowsNotFoundException()
        {
            var nonExistingCategorytId = 99;
            Assert.Throws<NotFoundException>(() => _service.Delete(nonExistingCategorytId, 0));
            _mockContext.Verify(c => c.Products.Remove(It.Is<Product>(p => p.Id == 99)), Times.Never());
            _mockContext.Verify(c => c.SaveChanges(), Times.Never());
        }

        [Fact]
        public void Update_Product_Successfully()
        {
            var dto = new UpdateProductDto { Name = "update product" };
            _service.Update(0, dto);
            var updateProduct = _mockContext.Object.Products.FirstOrDefault(p => p.Id == 0);
            Assert.Equal("update product", updateProduct.Name);
            _mockContext.Verify(c => c.SaveChanges(), Times.Once());
        }

        [Fact]
        public void Update_Product_NonExistingProduct_ThrowsNotFoundException()
        {
            var nonExistingProductId = 99;
            var dto = new UpdateProductDto { Name = "Update Product" };
            Assert.Throws<NotFoundException>(() => _service.Update(nonExistingProductId, dto));
            _mockContext.Verify(c => c.SaveChanges(), Times.Never());
        }

        [Fact]
        public void FilterProducts_ReturnsFilteredProducts()
        {
            var result = _service.FilterProducts(1, "Mob").ToList();
            Assert.Equal("Mobile", result[0].Name);
        }

        [Fact]
        public void FilterProducts_NotExistingCategory_ThrowsNotFoundException()
        {
            var nonExistingCategory = 99;
            Assert.Throws<NotFoundException>(() => _service.FilterProducts(nonExistingCategory, "Mob").ToList());
        }

        [Fact]
        public void PaginateProducts_PaginatesCorrectly()
        {
            int pageNumber = 2;
            int pageSize = 10;
            var additionalProducts = new List<Product>
            {
                new Product { Name = "Product1" },
                new Product { Name = "Product2" },
                new Product { Name = "Product3" },
                new Product { Name = "Product4" },
                new Product { Name = "Product5" },
                new Product { Name = "Product6" },
                new Product { Name = "Product7" }
            };
            _products.AddRange(additionalProducts);
            var paginated = _service.PaginateProducts(_products.AsQueryable(), pageSize, pageNumber);
            Assert.Equal("Product7", paginated[0].Name);
        }

        [Fact]
        public void SortProducts_ByNameAscending()
        {
            var sortBy = "Name";
            var sortOrder = SortOrder.Ascending;

            var sort = _service.SortProducts(_products.AsQueryable(), sortOrder, sortBy).ToList();
            Assert.Equal("Desktop", sort[0].Name);
        }

        [Fact]
        public void SortProducts_ByPriceAscending()
        {
            var sortBy = "Price";
            var sortOrder = SortOrder.Ascending;

            var sort = _service.SortProducts(_products.AsQueryable(), sortOrder, sortBy).ToList();
            Assert.Equal("Desktop", sort[3].Name);
        }

        [Fact]
        public void SortProducts_ByPriceDescending()
        {
            var sortBy = "Price";
            var sortOrder = SortOrder.Descending;

            var sort = _service.SortProducts(_products.AsQueryable(), sortOrder, sortBy).ToList();
            Assert.Equal("Desktop", sort[0].Name);
        }

        [Fact]
        public void SortProducts_ByNameDescending()
        {
            var sortBy = "Name";
            var sortOrder = SortOrder.Descending;

            var sort = _service.SortProducts(_products.AsQueryable(), sortOrder, sortBy).ToList();
            Assert.Equal("Desktop", sort[3].Name);
        }
    }
}
