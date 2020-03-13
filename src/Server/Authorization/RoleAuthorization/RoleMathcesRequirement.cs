using KnowledgeShare.Store.Core;

namespace KnowledgeShare.Server.Authorization.RoleAuthorization
{
    public class RoleMathcesRequirement : IRoleAuthorizationRequirement
    {
        private readonly CourseUserRole _role;

        public RoleMathcesRequirement(CourseUserRole role)
        {
            _role = role;
        }

        public bool Matches(CourseUserRole role)
        {
            return _role == role;
        }
    }
}
