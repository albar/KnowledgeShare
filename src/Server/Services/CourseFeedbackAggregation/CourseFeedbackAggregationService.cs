using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KnowledgeShare.Manager;
using KnowledgeShare.Store.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace KnowledgeShare.Server.Services.CourseFeedbackAggregation
{
    public class CourseFeedbackAggregationService : BackgroundService
    {
        private static readonly int _defaultDelay = 60 * 1000; // 1 minute

        private readonly ICourseFeedbackAggregationQueue _queue;
        private readonly IServiceScopeFactory _scopedService;

        public CourseFeedbackAggregationService(
            ICourseFeedbackAggregationQueue queue,
            IServiceScopeFactory factory)
        {
            _queue = queue;
            _scopedService = factory;
        }

        protected override async Task ExecuteAsync(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            var courseAggregationDelay = new Dictionary<Course, Task>();

            while (!token.IsCancellationRequested)
            {
                var course = await _queue.DequeueAsync(token);
                if (courseAggregationDelay.ContainsKey(course))
                {
                    continue;
                }

                var task = DelayedAggregationAsync(course, token).ContinueWith(_ =>
                {
                    courseAggregationDelay.Remove(course);
                });

                courseAggregationDelay.Add(course, task);
            }

            await Task.WhenAll(courseAggregationDelay.Values);
        }

        private async Task DelayedAggregationAsync(Course course, CancellationToken token)
        {
            await Task.Delay(_defaultDelay, token);
            if (token.IsCancellationRequested)
            {
                return;
            }
            using var scope = _scopedService.CreateScope();
            using var manager = scope.ServiceProvider.GetRequiredService<CourseManager>();

            var feedbackRates = await manager.GetFeedbacks(course)
                .Select(feedback => (int)feedback.Rate)
                .ToListAsync();

            course.FeedbackCount = feedbackRates.Count;
            course.FeedbackRateAverage = feedbackRates.Average();
            var rounded = (int)Math.Round(course.FeedbackRateAverage, MidpointRounding.AwayFromZero);
            course.FeedbackRate = (FeedbackRate)rounded;

            await manager.UpdateAsync(course, token);
        }
    }
}
