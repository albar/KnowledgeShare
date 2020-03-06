using System;
using System.Threading.Tasks;
using KnowledgeShare.Store.Abstractions;
using KnowledgeShare.Store.Core;
using Xunit;

namespace KnowledgeShare.Store.Test
{
    public abstract class CourseStoreSpecificationTestBase
    {
        protected abstract ICourseStore GetCourseStore();

        [Fact]
        public async Task Can_Create__And_Find_A_Course()
        {
            const string title = "The Title";

            Course course = new Course
            {
                Title = title,
            };
            await GetCourseStore().CreateAsync(course);
            Course found = await GetCourseStore().FindByIdAsync(course.Id);

            Assert.Equal(title, found.Title);
        }

        [Fact]
        public async Task Throw_When_Creating_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await GetCourseStore().CreateAsync(null));
        }

        [Fact]
        public async Task Can_Update_A_Course()
        {
            const string initialTitle = "Initial Title";
            const string updatedTitle = "Updated Title";

            Course course = new Course
            {
                Title = initialTitle,
            };
            await GetCourseStore().CreateAsync(course);
            course.Title = updatedTitle;
            await GetCourseStore().UpdateAsync(course);

            Course updated = await GetCourseStore().FindByIdAsync(course.Id);

            Assert.Equal(updatedTitle, updated.Title);
        }

        [Fact]
        public async Task Throw_When_Updating_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await GetCourseStore().UpdateAsync(null));
        }

        [Fact]
        public async Task Can_Remove_A_Course()
        {
            Course course = new Course();
            await GetCourseStore().CreateAsync(course);
            await GetCourseStore().RemoveAsync(course);

            Course found = await GetCourseStore().FindByIdAsync(course.Id);

            Assert.Null(found);
        }

        [Fact]
        public async Task Throw_When_Removing_Null()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await GetCourseStore().RemoveAsync(null));
        }
    }
}
