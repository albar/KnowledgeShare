using System.Threading.Tasks;
using KnowledgeShare.Store.Core;

namespace KnowledgeShare.Server.Authorization.CourseAuthorization
{
    public class RemoveCourseRequirement : ICourseAuthorizationRequirement
    {
        private readonly CourseUser _accessor;

        public RemoveCourseRequirement(CourseUser accessor)
        {
            _accessor = accessor;
        }

        public Task<bool> CheckAsync(Course course)
        {
            var result = _accessor.Role == CourseUserRole.Administrator ||
                course.Author.Id == _accessor.Id;

            return Task.FromResult(result);
        }
    }
}
