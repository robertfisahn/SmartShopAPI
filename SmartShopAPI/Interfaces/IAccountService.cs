using SmartShopAPI.Models.Dtos.User;

namespace SmartShopAPI.Interfaces
{
    public interface IAccountService
    {
        void RegisterUser(RegisterUserDto dto);
        public string GenerateJwt(LoginDto dto);
    }
}