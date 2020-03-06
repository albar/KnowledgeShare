using System;
using System.Linq;
using System.Threading.Tasks;
using KnowledgeShare.Store.Abstractions;
using KnowledgeShare.Store.Core;
using Xunit;

namespace KnowledgeShare.Store.Test
{
    public abstract class CourseFeedbackStoreSpecificationTest
    {
        protected abstract ICourseFeedbackStore GetCourseFeedbackStore();

        [Fact]
        public async Task Can_Add_Feedback_To_A_Course()
        {
            Course course = new Course();
            CourseUser user = new CourseUser();
            await GetCourseFeedbackStore().AddFeedbackToAsync(
                course,
                user,
                FeedbackRate.NotBad,
                null);

            Assert.Contains(user, course.Feedbacks.Select(attendant => attendant.User));
        }

        [Fact]
        public async Task Throws_When_Registering_A_User_To_A_Course_With_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await GetCourseFeedbackStore()
                    .AddFeedbackToAsync(null, new CourseUser(), FeedbackRate.NotBad, null));

            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await GetCourseFeedbackStore()
                    .AddFeedbackToAsync(new Course(), null, FeedbackRate.NotBad, null));

            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await GetCourseFeedbackStore()
                    .AddFeedbackToAsync(null, null, FeedbackRate.NotBad, null));
        }
    }
}
