namespace SmartShopAPI.Models.Dtos
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public virtual List<ProductDto>? Products { get; set; }
    }
}
