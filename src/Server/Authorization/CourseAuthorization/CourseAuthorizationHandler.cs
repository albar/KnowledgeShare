using System.Threading.Tasks;
using KnowledgeShare.Store.Core;
using Microsoft.AspNetCore.Authorization;

namespace KnowledgeShare.Server.Authorization.CourseAuthorization
{
    public class CourseAuthorizationHandler : AuthorizationHandler<ICourseAuthorizationRequirement, Course>
    {
        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            ICourseAuthorizationRequirement requirement,
            Course resource)
        {
            if (await requirement.CheckAsync(resource))
            {
                context.Succeed(requirement);
            }
        }
    }
}
