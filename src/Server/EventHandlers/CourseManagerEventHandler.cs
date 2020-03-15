using System.Linq;
using System.Threading.Tasks;
using KnowledgeShare.Manager.Abstractions;
using KnowledgeShare.Server.Notification;
using KnowledgeShare.Server.Services.CourseFeedbackAggregation;
using KnowledgeShare.Store.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace KnowledgeShare.Server.EventHandlers
{
    public class CourseManagerEventHandler : ICourseManagerEventHandler
    {
        private readonly ICourseFeedbackAggregationQueue _feedbackAggregationQueue;
        private readonly IHubClients<IKnowledgeShareNotification> _hubClients;
        private readonly ILogger<CourseManagerEventHandler> _logger;

        public CourseManagerEventHandler(
            ICourseFeedbackAggregationQueue queue,
            IHubContext<KnowledgeShareNotificationHub, IKnowledgeShareNotification> hubContext,
            ILogger<CourseManagerEventHandler> logger)
        {
            _feedbackAggregationQueue = queue;
            _logger = logger;
            _hubClients = hubContext.Clients;
        }

        public Task CreatedAsync(Course course)
        {
            // notify subscriber for new public course
            return Task.CompletedTask;
        }

        public Task UpdatedAsync(Course course)
        {
            // notify registrant for course update
            return Task.CompletedTask;
        }

        public async Task UserRegisteredAsync(Course course, params CourseUser[] users)
        {
            await _hubClients
                .Users(users.Select(u => u.Id).ToList())
                .Notification(KnowledgeShareNotification.FromRaw(
                    "You are registered to a {0} course \"{1}\".\n",
                    new object[]
                    {
                        course.Visibility.ToString(),
                        course.Title
                    }));

            _logger.LogInformation("notify users registered");
        }

        public Task FeedbackGivenAsync(Course course, CourseUser user)
        {
            _feedbackAggregationQueue.Enqueue(course);
            return Task.CompletedTask;
        }

        public Task RemovedAsync(Course course)
        {
            // do not know what to do
            return Task.CompletedTask;
        }
    }
}
