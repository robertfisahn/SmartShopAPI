using SmartShopAPI.Entities;

namespace SmartShopAPI.Interfaces
{
    public interface IOrderService
    {
        int Create();
        Order GetById(int id);
    }
}