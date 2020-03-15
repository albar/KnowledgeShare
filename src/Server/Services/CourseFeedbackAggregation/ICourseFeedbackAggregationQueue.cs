using System.Threading;
using System.Threading.Tasks;
using KnowledgeShare.Store.Core;

namespace KnowledgeShare.Server.Services.CourseFeedbackAggregation
{
    public interface ICourseFeedbackAggregationQueue
    {
        void Enqueue(Course course);
        Task<Course> DequeueAsync(CancellationToken token);
    }
}
