using System.Collections.Generic;
using System.Linq;
using KnowledgeShare.Store.Core;

namespace KnowledgeShare.Server.Authorization.RoleAuthorization
{
    public class RoleMatchesAnyRequirement : IRoleAuthorizationRequirement
    {
        private readonly IEnumerable<CourseUserRole> _roles;

        public RoleMatchesAnyRequirement(IEnumerable<CourseUserRole> roles)
        {
            _roles = roles;
        }

        public bool Matches(CourseUserRole role)
        {
            return _roles.Any(r => r == role);
        }
    }
}
