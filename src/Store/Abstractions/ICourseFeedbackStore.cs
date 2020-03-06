using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KnowledgeShare.Store.Core;

namespace KnowledgeShare.Store.Abstractions
{
    public interface ICourseFeedbackStore
    {
        Task AddFeedbackToAsync(
            Course course,
            CourseUser user,
            FeedbackRate rate,
            string message,
            CancellationToken token = default);

        IQueryable<Feedback> GetFeedbacks(Course course);
    }
}
