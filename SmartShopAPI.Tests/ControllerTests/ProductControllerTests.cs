using Microsoft.AspNetCore.Mvc;
using Moq;
using SmartShopAPI.Controllers;
using SmartShopAPI.Interfaces;
using SmartShopAPI.Models.Dtos;
using SmartShopAPI.Models.Dtos.Product;

namespace SmartShopAPI.Tests.ControllerTests
{
    public class ProductControllerTests
    {
        private readonly Mock<IProductService> _productServiceMock;
        private readonly ProductController _productController;

        public ProductControllerTests()
        {
            _productServiceMock = new Mock<IProductService>();
            _productController = new ProductController(_productServiceMock.Object);
        }

        [Fact]
        public void Get_ReturnsOk()
        {
            List<ProductDto> products = new List<ProductDto>()
            {
                new ProductDto() { Name = "Product 1"},
                new ProductDto() { Name = "Product 2" }
            };

            var queryParams = new QueryParams
            {
                PageNumber = 1,
                PageSize = 10,
                SearchPhrase = "op",
                SortBy = "Price",
                SortOrder = SortOrder.Descending
            };
            _productServiceMock.Setup(service => service.Get(1, queryParams))
                .Returns(new PagedResult<ProductDto>(products, 2, 1, 10)); ;


            var result = _productController.Get(1, queryParams);

            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public void GetById_ReturnsOk()
        {
            var productDto = new ProductDto { Id = 1, Name = "Product1", Price = 20 };
            _productServiceMock.Setup(service => service.GetById(1, 1)).Returns(productDto);

            var result = _productController.GetById(1, 1);

            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public void CreateProduct_ReturnsCreated()
        {
            var createdProductId = 8;
            var productDto = new CreateProductDto { Name = "Product1", Price = 20 };
            _productServiceMock.Setup(service => service.Create(1, productDto)).Returns(createdProductId);

            var result = _productController.Create(1, productDto);

            Assert.IsType<CreatedResult>(result);
            _productServiceMock.Verify(service => service.Create(1, productDto), Times.Once);
        }

        [Fact]
        public void DeleteProduct_ReturnsNoContent()
        {
            _productServiceMock.Setup(service => service.Delete(1, 1));

            var result = _productController.Delete(1, 1);

            Assert.IsType<NoContentResult>(result);
            _productServiceMock.Verify(service => service.Delete(1, 1), Times.Once);
        }

        [Fact]
        public void UpdateProduct_ReturnsNoContent()
        {
            var productDto = new UpdateProductDto { Name = "ProductUpdate", Price = 30 };
            _productServiceMock.Setup(service => service.Update(1, productDto));

            var result = _productController.Update(1, productDto);

            Assert.IsType<NoContentResult>(result);
            _productServiceMock.Verify(service => service.Update(1, productDto), Times.Once);
        }

    }
}
