using System.Threading.Tasks;
using KnowledgeShare.Store.Core;

namespace KnowledgeShare.Server.Authorization.CourseAuthorization
{
    public class CreateCourseRequirement : ICourseAuthorizationRequirement
    {
        private readonly CourseUser _accessor;

        public CreateCourseRequirement(CourseUser accessor)
        {
            _accessor = accessor;
        }

        public Task<bool> CheckAsync(Course course)
        {
            var result = _accessor.Role == CourseUserRole.Administrator ||
                _accessor.Role == CourseUserRole.Manager;

            return Task.FromResult(result);
        }
    }
}
