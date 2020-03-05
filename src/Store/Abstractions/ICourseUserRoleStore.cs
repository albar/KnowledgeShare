using System.Threading;
using System.Threading.Tasks;
using KnowledgeShare.Store.Core;

namespace KnowledgeShare.Store.Abstractions
{
    public interface ICourseUserRoleStore
    {
        Task SetCourseUserRoleAsync(
            CourseUser user,
            CourseUserRole role,
            CancellationToken token = default);
    }
}
