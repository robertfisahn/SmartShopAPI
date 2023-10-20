using System.Security.Claims;

namespace SmartShopAPI.Interfaces
{
    public interface IUserContextService
    {
        ClaimsPrincipal User { get; }
        public int GetUserId();
    }
}