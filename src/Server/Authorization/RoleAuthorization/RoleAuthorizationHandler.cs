using System.Threading.Tasks;
using KnowledgeShare.Store.Core;
using Microsoft.AspNetCore.Authorization;

namespace KnowledgeShare.Server.Authorization.RoleAuthorization
{
    public class RoleAuthorizationHandler :
        AuthorizationHandler<IRoleAuthorizationRequirement, CourseUser>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            IRoleAuthorizationRequirement requirement,
            CourseUser resource)
        {
            if (requirement.Matches(resource.Role))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
