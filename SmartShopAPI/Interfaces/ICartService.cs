using SmartShopAPI.Entities;
using SmartShopAPI.Models.Dtos.CartItem;

namespace SmartShopAPI.Interfaces
{
    public interface ICartService
    {
        IEnumerable<CartItemDto> GetById(int userId);
        int Create(int id, CreateCartDto item);
        void Delete(int cartItemId);
        void Update(int cartItemId, CreateCartDto dto);
    }
}