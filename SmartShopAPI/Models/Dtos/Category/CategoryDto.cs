using SmartShopAPI.Models.Dtos.Product;

namespace SmartShopAPI.Models.Dtos.Category
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual List<ProductDto> Products { get; set; }
    }
}
