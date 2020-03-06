using System;
using System.Threading.Tasks;
using KnowledgeShare.Store.Abstractions;
using KnowledgeShare.Store.Core;
using Xunit;

namespace KnowledgeShare.Store.Test
{
    public abstract class CourseUserRoleStoreSpecificationTestBase
    {
        protected abstract ICourseUserRoleStore GetCourseUserRoleStore();

        [Fact]
        public async Task Can_Set_User_Role()
        {
            const CourseUserRole role = CourseUserRole.Manager;

            CourseUser user = new CourseUser
            {
                Role = CourseUserRole.User,
            };

            await GetCourseUserRoleStore().SetCourseUserRoleAsync(user, role);

            Assert.Equal(role, user.Role);
        }

        [Fact]
        public async Task Throws_When_Set_Null_User_A_Role()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await GetCourseUserRoleStore()
                    .SetCourseUserRoleAsync(null, CourseUserRole.Administrator));
        }
    }
}
