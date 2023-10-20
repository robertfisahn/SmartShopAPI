using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;

namespace SmartShopAPI.Models.Dtos.User
{
    public class RegisterUserDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string PostalCode { get; set; }
        public int RoleId { get; set; }
    }
}
