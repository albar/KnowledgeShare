using System;
using System.Linq;
using System.Threading.Tasks;
using KnowledgeShare.Store.Abstractions;
using KnowledgeShare.Store.Core;
using Xunit;

namespace KnowledgeShare.Store.Test
{
    public abstract class CourseRegistrantStoreSpecificationTestBase
    {
        protected abstract ICourseRegistrantStore GetRegistrantCourseStore();

        [Fact]
        public async Task Can_Register_A_User_To_A_Course()
        {
            Course course = new Course();
            CourseUser user = new CourseUser();
            await GetRegistrantCourseStore().RegisterUserToAsync(course, user);

            Assert.Contains(user, course.Registrants.Select(registrant => registrant.User));
        }

        [Fact]
        public async Task Throws_When_Registering_A_User_To_A_Course_With_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await GetRegistrantCourseStore()
                    .RegisterUserToAsync(null, new CourseUser()));

            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await GetRegistrantCourseStore()
                    .RegisterUserToAsync(new Course(), null));

            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await GetRegistrantCourseStore()
                    .RegisterUserToAsync(null, null));
        }
    }
}
