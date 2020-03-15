using System.Threading.Tasks;
using KnowledgeShare.Store.Core;

namespace KnowledgeShare.Manager.Abstractions
{
    public interface ICourseManagerEventHandler
    {
        Task CreatedAsync(Course course);
        Task UpdatedAsync(Course course);
        Task UserRegisteredAsync(Course course, params CourseUser[] users);
        Task FeedbackGivenAsync(Course course, CourseUser user);
        Task RemovedAsync(Course course);
    }
}
