using System.ComponentModel.DataAnnotations;

namespace SmartShopAPI.Entities
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int RoleId { get; set; }
        public virtual Role Role { get; set; }
        public int? AddressId { get; set; }
        public virtual Address? Address { get; set; }
        public virtual List<CartItem>? CartItems { get; set; }
        public virtual List<Order>? Order { get; set; }
    }
}
