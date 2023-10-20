using Microsoft.AspNetCore.Mvc;
using SmartShopAPI.Interfaces;
using SmartShopAPI.Models.Dtos.Product;

namespace SmartShopAPI.Controllers
{
    [Route("api/category/{categoryId}/product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public ActionResult<List<ProductDto>> Get([FromRoute]int categoryId)
        {
            var products = _productService.Get(categoryId);
            return Ok(products);
        }

        [HttpGet("{productId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public ActionResult<ProductDto> GetById([FromRoute]int categoryId, [FromRoute]int productId)
        {
            var product = _productService.GetById(categoryId, productId);
            return Ok(product);
        }

        [HttpPost(Name="CreateProduct")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public ActionResult Create([FromRoute]int categoryId, [FromBody]CreateProductDto dto)
        {
            var productId = _productService.Create(categoryId, dto);
            return Created($"category/{categoryId}/product/{productId}", null);
            //return CreatedAtRoute("CreateProduct", new { id = product.Id }, product);
        }

        [HttpDelete("{productId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public ActionResult Delete([FromRoute]int categoryId, [FromRoute]int productId)
        {
            _productService.Delete(categoryId, productId);
            return NoContent();
        }

        [HttpPut("{productId}")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public ActionResult Update([FromRoute]int productId, UpdateProductDto dto)
        {
            _productService.Update(productId, dto);
            return NoContent();
        }
    }
}
