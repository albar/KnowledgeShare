using System.Threading.Tasks;
using KnowledgeShare.Store.Core;

namespace KnowledgeShare.Server.Authorization.CourseAuthorization
{
    public class RegisterCourseRequirement : ICourseAuthorizationRequirement
    {
        public Task<bool> CheckAsync(Course course)
        {
            return Task.FromResult(course.Visibility == CourseVisibility.Public);
        }
    }
}
