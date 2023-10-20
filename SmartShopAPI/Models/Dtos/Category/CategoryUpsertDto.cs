using System.ComponentModel.DataAnnotations;

namespace SmartShopAPI.Models.Dtos.Category
{
    public class CategoryUpsertDto
    {
        [Required]
        [MaxLength(25)]
        public string Name { get; set; }
    }
}
