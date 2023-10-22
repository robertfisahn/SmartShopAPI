using Microsoft.AspNetCore.Authorization;
using SmartShopAPI.Entities;
using SmartShopAPI.Interfaces;
using System.Security.Claims;

namespace SmartShopAPI.Authorization
{
    public class ResourceOperationRequirementHandler : AuthorizationHandler<ResourceOperationRequirement, IUserVerification>
    {
        private readonly IUserContextService _userContextService;
        public ResourceOperationRequirementHandler(IUserContextService userContextService)
        {
           _userContextService = userContextService;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ResourceOperationRequirement requirement,
            IUserVerification resource)
        {
            if(requirement.ResourceOperation == ResourceOperation.Create)
            {
                context.Succeed(requirement);
            }

            var userId = _userContextService.GetUserId();
            
            if(resource is IEnumerable<IUserVerification> resourcesToVerify)
            {
                if(resourcesToVerify.First().UserId == userId)
                {
                    context.Succeed(requirement);
                }
            }
            else if(resource is IUserVerification resourceToVerify)
            {
                if (resourceToVerify.UserId == userId)
                {
                    context.Succeed(requirement);
                }
            }
            
            return Task.CompletedTask;
        }
    }
}
