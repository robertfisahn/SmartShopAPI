﻿using SmartShopAPI.Exceptions;
using SmartShopAPI.Interfaces;
using System.Security.Claims;

namespace SmartShopAPI.Services
{
    public class UserContextService : IUserContextService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        
        public UserContextService(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public ClaimsPrincipal User => _contextAccessor.HttpContext?.User;

        public int GetUserId()
        {
            var claim = User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier);
            if(claim == null)
            {
                throw new NotFoundException("User not found");
            }
            return int.Parse(claim.Value);
        }
    }

}
