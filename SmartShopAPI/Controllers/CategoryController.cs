using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartShopAPI.Data;
using SmartShopAPI.Models;
using SmartShopAPI.Models.Dtos;

namespace SmartShopAPI.Controller
{

    [Route("category")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly SmartShopDbContext _context;
        private readonly IMapper _mapper;
        
        public CategoryController(SmartShopDbContext dbContext, IMapper mapper)
        {
            _context = dbContext;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CategoryDto>> GetAll()
        {
            var categories = _context.Categories
                .Include(x => x.Products);
            var dto = _mapper.Map<List<CategoryDto>>(categories);
            return Ok(dto);
        }

        [HttpGet("{id}")]
        public ActionResult<Category> GetCategory([FromRoute]int id)
        {
            var category = _context.Categories.FirstOrDefault(c => c.Id == id);
            return Ok(category);
        }

        [HttpPost]
        public ActionResult Create([FromBody]CreateCategoryDto dto) 
        {
            var entityCategory = _mapper.Map<Category>(dto);
            _context.Categories.Add(entityCategory);
            _context.SaveChanges();
            return Created($"category/{entityCategory.Id}", null);
        }

        [HttpDelete("{categoryId}")]
        public ActionResult Delete([FromRoute]int categoryId)
        {
            var category = _context.Categories.FirstOrDefault(c => c.Id == categoryId);
            _context.Categories.Remove(category);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpPut("{categoryId}")]
        public ActionResult Update([FromRoute]int categoryId, [FromBody]UpdateCategoryDto dto)
        {
            var category = _context.Categories.FirstOrDefault(x => x.Id == categoryId);
            _mapper.Map(dto, category);
            _context.SaveChanges();
            return Ok();
        }
    }
}
