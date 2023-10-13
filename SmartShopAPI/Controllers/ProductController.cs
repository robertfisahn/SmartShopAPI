using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using SmartShopAPI.Data;
using SmartShopAPI.Models;
using SmartShopAPI.Models.Dtos;

namespace SmartShopAPI.Controller
{
    [Route("category/{categoryId}/product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly SmartShopDbContext _context;
        private readonly IMapper _mapper;
        
        public ProductController(SmartShopDbContext dbContext, IMapper mapper)
        {
            _context = dbContext;
            _mapper = mapper;
        }
        
        [HttpGet]
        public ActionResult<List<ProductDto>> Get([FromRoute]int categoryId)
        {
            var category = _context.Categories
                .Include(x => x.Products)
                .FirstOrDefault(c => c.Id == categoryId);
            var products = _mapper.Map<List<ProductDto>>(category.Products.ToList());
            
            return Ok(products);
        }

        [HttpGet("{productId}")]
        public ActionResult<ProductDto> GetById([FromRoute]int categoryId, [FromRoute]int productId)
        {
            var category = _context.Categories
                .Include(x => x.Products)
                .FirstOrDefault (c => c.Id == categoryId);
            var product = category.Products.FirstOrDefault(x => x.Id == productId);
            var dto = _mapper.Map<ProductDto>(product);
            return Ok(dto);
        }

        [HttpPost(Name="CreateProduct")]
        public ActionResult Create([FromRoute]int categoryId, [FromBody]CreateProductDto dto)
        {
            var product = _mapper.Map<Product>(dto);
            product.CategoryId = categoryId;
            _context.Products.Add(product);
            _context.SaveChanges();
            return Created($"category/{categoryId}/product/{product.Id}", null);
            //return CreatedAtRoute("CreateProduct", new { id = product.Id }, product);
        }

        [HttpDelete("{productId}")]
        public ActionResult Delete([FromRoute]int categoryId, [FromRoute]int productId)
        {
            var category = _context.Categories.FirstOrDefault(x => x.Id == categoryId);
            var product = category.Products.FirstOrDefault(p  => p.Id == productId);
            _context.Products.Remove(product);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpPatch("{productId}")]
        public ActionResult Update([FromRoute]int productId, JsonPatchDocument<ProductDto> jsonDto)
        {
            var product = _context.Products.FirstOrDefault(x => x.Id == productId);
            var dto = _mapper.Map<ProductDto>(product);
            jsonDto.ApplyTo(dto, ModelState);
            _mapper.Map(dto, product);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
