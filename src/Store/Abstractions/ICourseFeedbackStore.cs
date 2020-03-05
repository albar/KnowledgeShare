using System.Threading;
using System.Threading.Tasks;
using KnowledgeShare.Store.Core;

namespace KnowledgeShare.Store.Abstractions
{
    public interface ICourseFeedbackStore
    {
        Task AddFeedbackToAsync(
            Course course,
            ICourseUser user,
            FeedbackRate rate,
            string message,
            CancellationToken token = default);
    }
}
