using SmartShopAPI.Models.Dtos;

namespace SmartShopAPI.Services
{
    public interface IAccountService
    {
        void RegisterUser(RegisterUserDto dto);
        public string GenerateJwt(LoginDto dto);
    }
}