using KnowledgeShare.Store.Core;
using Microsoft.AspNetCore.Authorization;

namespace KnowledgeShare.Server.Authorization.RoleAuthorization
{
    public interface IRoleAuthorizationRequirement : IAuthorizationRequirement
    {
        bool Matches(CourseUserRole role);
    }
}
