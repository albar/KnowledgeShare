using System.Threading;
using System.Threading.Tasks;
using KnowledgeShare.Entity;

namespace KnowledgeShare.Store.Abstractions
{
    public interface ICourseUserRoleStore
    {
        Task SetCourseUserRoleAsync(
            ICourseUser user,
            CourseUserRole role,
            CancellationToken token = default);
    }
}
