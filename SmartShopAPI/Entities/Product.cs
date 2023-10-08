using System.ComponentModel;

namespace SmartShopAPI.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set;}
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
        public DateTime? UpdatedDate { get; set; }

        public int StockQuantity { get; set; }
        public int CategoryId { get; set; }
        public virtual Category? Category { get; set; }
    }
}
