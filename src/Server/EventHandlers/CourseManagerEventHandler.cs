using System.Linq;
using System.Threading.Tasks;
using KnowledgeShare.Manager.Abstractions;
using KnowledgeShare.Server.Hubs.Course;
using KnowledgeShare.Server.Hubs.Notification;
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
        private readonly IHubClients<IKnowledgeShareNotification> _notificationClients;
        private readonly IHubClients<ICourseHub> _courseClients;
        private readonly ILogger<CourseManagerEventHandler> _logger;

        public CourseManagerEventHandler(
            ICourseFeedbackAggregationQueue queue,
            IHubContext<KnowledgeShareNotificationHub, IKnowledgeShareNotification> notificationHubContext,
            IHubContext<CourseHub, ICourseHub> courseHubContext,
            ILogger<CourseManagerEventHandler> logger)
        {
            _feedbackAggregationQueue = queue;
            _notificationClients = notificationHubContext.Clients;
            _courseClients = courseHubContext.Clients;
            _logger = logger;
        }

        public async Task CreatedAsync(Course course)
        {
            await _courseClients.All.CourseCreated(course);
        }

        public async Task UpdatedAsync(Course course)
        {
            await _courseClients.All.CourseUpdated(course);
        }

        public async Task UserRegisteredAsync(Course course, params CourseUser[] users)
        {
            _logger.LogInformation($"Registered User Count: {users.Length}");
            await _notificationClients
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
