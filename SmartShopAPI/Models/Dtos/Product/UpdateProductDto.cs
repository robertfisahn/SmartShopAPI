using System.ComponentModel.DataAnnotations;

namespace SmartShopAPI.Models.Dtos.Product
{
    public class UpdateProductDto
    {
        [Required]
        [MaxLength(25)]
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
    }
}
