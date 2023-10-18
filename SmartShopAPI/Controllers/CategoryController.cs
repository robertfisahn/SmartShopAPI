using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartShopAPI.Models.Dtos;
using SmartShopAPI.Services;

namespace SmartShopAPI.Controllers
{
    [Route("api/category")]
    [ApiController]
    [Authorize]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [AllowAnonymous]
        public ActionResult<IEnumerable<CategoryDto>> GetAll()
        {
            var categories = _categoryService.GetAll();
            return Ok(categories);
        }

        [HttpGet("{categoryId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [Authorize(Roles = "CUSTOM")]
        public ActionResult<CategoryDto> GetCategory([FromRoute]int categoryId)
        {
            var category = _categoryService.GetCategory(categoryId);
            return Ok(category);
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public ActionResult Create([FromBody]CreateCategoryDto dto) 
        {
            var categoryId = _categoryService.Create(dto);
            return Created($"category/{categoryId}", null);
        }

        [HttpDelete("{categoryId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public ActionResult Delete([FromRoute]int categoryId)
        {
            _categoryService.Delete(categoryId);
            return NoContent();
        }

        [HttpPut("{categoryId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public ActionResult Update([FromRoute]int categoryId, [FromBody]UpdateCategoryDto dto)
        {
            _categoryService.Update(categoryId, dto);
            return Ok();
        }
    }
}
