using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;

namespace SmartShopAPI.Models.Dtos
{
    public class RegisterUserDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string PostalCode { get; set; }
        [Required]
        public int RoleId { get; set; }
    }
}
