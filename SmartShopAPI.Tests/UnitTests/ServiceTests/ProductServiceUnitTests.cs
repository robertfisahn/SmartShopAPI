using SmartShopAPI.Models.Dtos.Product;
using SmartShopAPI.Tests.Helpers;
using SmartShopAPI.Exceptions;
using SmartShopAPI.Models.Dtos;

namespace SmartShopAPI.Tests.UnitTests.ServiceTests
{
    public class ProductServiceUnitTests : UnitTestBase, IDisposable
    {
        public ProductServiceUnitTests() : base() { }

        [Fact]
        public async Task GetProducts_Succesfully()
        {
            var queryParams = new QueryParams()
            {
                PageNumber = 1,
                PageSize = 10,
                SearchPhrase = "Olej",
                SortBy = "Price",
                SortOrder = SortOrder.Descending
            };
            var products = await _service.GetAsync(1, queryParams);
            Assert.Equal(2, products.TotalCount);
            Assert.Equal("Olej Motul 5w30", products.Items[0].Name);
        }

        [Fact]
        public async Task CheckCategory_ExistingCategory_DoesNotThrowException()
        {
            var existingCategory = 1;
            var exception = await Record.ExceptionAsync(() => _service.CheckCategory(existingCategory));
            Assert.Null(exception);
        }

        [Fact]
        public async Task CheckCategory_NonExistingCategory_ThrowsNotFoundException()
        {
            var nonExistingCategory = 11;
            await Assert.ThrowsAsync<NotFoundException>(() => _service.CheckCategory(nonExistingCategory));
        }

        [Fact]
        public async Task GetById_ExistingProduct()
        {
            var existingProduct = await _service.GetByIdAsync(1, 1);
            Assert.Equal(1, existingProduct.Id);
        }

        [Theory]
        [InlineData(1, 88)]
        [InlineData(88, 1)]
        public async Task GetById_ThrowsNotFoundException(int categoryId, int productId)
        {
            await Assert.ThrowsAsync<NotFoundException>(() => _service.GetByIdAsync(categoryId, productId));
        }

        [Fact]
        public async Task Create_Product_Successfully()
        {
            var newProductId = await _service.CreateAsync(1, ProductTestData.CreateProductDto);
            Assert.NotNull(await _service.GetByIdAsync(1, newProductId));
        }

        [Fact]
        public async Task Create_Product_NonExistingCategory_ThrowsNotFoundException()
        {
            await Assert.ThrowsAsync<NotFoundException>(() => _service.CreateAsync(0, ProductTestData.CreateProductDto));
        }

        [Fact]
        public async Task Delete_Product_Successfully()
        {
            await _service.DeleteAsync(1, 1);
            var productExists = _context.Products.Any(p => p.Id == 1);
            Assert.False(productExists);
        }

        [Fact]
        public async Task Delete_Product_NonExistingProduct_ThrowsNotFoundException()
        {
            var nonExistingProductId = 99;
            await Assert.ThrowsAsync<NotFoundException>(() => _service.DeleteAsync(0, nonExistingProductId));
        }

        [Fact]
        public async Task Delete_Product_NonExistingCategory_ThrowsNotFoundException()
        {
            var nonExistingCategorytId = 99;
            await Assert.ThrowsAsync<NotFoundException>(() => _service.DeleteAsync(nonExistingCategorytId, 1));
        }

        [Fact]
        public async Task Update_Product_Successfully()
        {
            var dto = new UpdateProductDto { Name = "update product" };
            await _service.UpdateAsync(1, dto);
            var updateProduct = _context.Products.FirstOrDefault(p => p.Id == 1);
            Assert.Equal("update product", updateProduct.Name);
        }

        [Fact]
        public async Task Update_Product_NonExistingProduct_ThrowsNotFoundException()
        {
            var nonExistingProductId = 99;
            var dto = new UpdateProductDto { Name = "Update Product" };
            await Assert.ThrowsAsync<NotFoundException>(() => _service.UpdateAsync(nonExistingProductId, dto));
        }

        [Fact]
        public async Task FilterProducts_ReturnsFilteredProducts()
        {
            var result = await _service.FilterProducts(1, "5w30");
            Assert.Single(result);
            Assert.Equal("Olej Motul 5w30", result[0].Name);
        }

        [Fact]
        public void PaginateProducts_PaginatesCorrectly()
        {
            int pageNumber = 2;
            int pageSize = 10;
            var additionalProducts = ProductTestData.Products;
            _context.Products.AddRange(additionalProducts);
            _context.SaveChanges();
            var paginated = _service.PaginateProducts(_context.Products.ToList(), pageSize, pageNumber);
            Assert.Single(paginated);
        }

        [Theory]
        [InlineData("Name", SortOrder.Ascending, 0, "Klocki hamulcowe Brembo")]
        [InlineData("Name", SortOrder.Descending, 0, "Tłumik końcowy Akrapovic")]
        [InlineData("Price", SortOrder.Ascending, 0, "Świeca zapłonowa NGK")]
        [InlineData("Price", SortOrder.Descending, 0, "Tłumik końcowy Akrapovic")]
        public void SortProducts(string sortBy, SortOrder sortOrder, int expectedPosition, string productName)
        {
            var sort = _service.SortProducts(_context.Products.ToList(), sortOrder, sortBy).ToList();
            Assert.Equal(productName, sort[expectedPosition].Name);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
