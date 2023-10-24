using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartShopAPI.Interfaces;
using SmartShopAPI.Models.Dtos;
using SmartShopAPI.Models.Dtos.Product;

namespace SmartShopAPI.Controllers
{
    [Route("api/category/{categoryId}/product")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public ActionResult<IEnumerable<ProductDto>> Get([FromRoute]int categoryId, [FromQuery]QueryParams query)
        {
            var products = _productService.Get(categoryId, query);
            return Ok(products);
        }

        [HttpGet("{productId}")]
        [AllowAnonymous]
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
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public ActionResult Create([FromRoute]int categoryId, [FromBody]CreateProductDto dto)
        {
            var productId = _productService.Create(categoryId, dto);
            return Created($"category/{categoryId}/product/{productId}", null);
            //return CreatedAtRoute("CreateProduct", new { id = product.Id }, product);
        }

        [HttpDelete("{productId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public ActionResult Delete([FromRoute]int categoryId, [FromRoute]int productId)
        {
            _productService.Delete(categoryId, productId);
            return NoContent();
        }

        [HttpPut("{productId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public ActionResult Update([FromRoute]int productId, UpdateProductDto dto)
        {
            _productService.Update(productId, dto);
            return NoContent();
        }
    }
}
