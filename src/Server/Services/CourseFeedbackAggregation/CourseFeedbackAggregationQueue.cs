using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using KnowledgeShare.Store.Core;

namespace KnowledgeShare.Server.Services.CourseFeedbackAggregation
{
    public class CourseFeedbackAggregationQueue : ICourseFeedbackAggregationQueue
    {
        private readonly ConcurrentQueue<Course> _queue = new ConcurrentQueue<Course>();
        private readonly SemaphoreSlim _lock = new SemaphoreSlim(0);

        public void Enqueue(Course course)
        {
            _queue.Enqueue(course);
            _lock.Release();
        }

        public async Task<Course> DequeueAsync(CancellationToken token)
        {
            await _lock.WaitAsync(token);
            _queue.TryDequeue(out var course);
            return course;
        }
    }
}
