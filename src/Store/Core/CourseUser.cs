using Microsoft.AspNetCore.Identity;

namespace KnowledgeShare.Store.Core
{
    public class CourseUser : IdentityUser
    {
        public CourseUserRole Role { get; set; }
    }
}
