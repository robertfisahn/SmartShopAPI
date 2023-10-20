using System.ComponentModel.DataAnnotations;

namespace SmartShopAPI.Entities
{
    public class Address
    {
        public int Id { get; set; }
        [MaxLength(50)]
        public string? City { get; set; }
        [MaxLength(50)]
        public string? Street { get; set; }
        public string? PostalCode { get; set; }
        public virtual User? User { get; set; }
        public virtual List<Order>? Order { get; set; }
    }
}
