using System;
using System.Threading;
using System.Threading.Tasks;
using KnowledgeShare.Store.Core;
using KnowledgeShare.Store.Abstractions;
using Microsoft.AspNetCore.Identity;
using Moq;
using Xunit;

namespace KnowledgeShare.Manager.Test
{
    public class CourseUserManagerTest
    {
        [Fact]
        public async Task Can_Change_User_Role()
        {
            var fakeCourseUserStore = new Mock<ICourseUserRoleStore>();
            fakeCourseUserStore.Setup(store => store.SetCourseUserRoleAsync(
                    It.IsAny<Store.Core.CourseUser>(),
                    It.IsAny<CourseUserRole>(),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            CourseUserManager<CourseUser> manager = new CourseUserManager<CourseUser>(
                fakeCourseUserStore.As<IUserStore<CourseUser>>().Object,
                null, null, null, null, null, null, null, null);

            await manager.SetCourseUserRoleAsync(CreateUser(), CourseUserRole.Manager);

            fakeCourseUserStore.Verify(store => store.SetCourseUserRoleAsync(
                    It.IsAny<Store.Core.CourseUser>(),
                    It.IsAny<CourseUserRole>(),
                    It.IsAny<CancellationToken>()),
                Times.Once());
        }

        [Fact]
        public async Task Throw_When_Changing_Role_With_Null_User()
        {
            var fakeUserStore = new Mock<IUserStore<CourseUser>>();

            CourseUserManager<CourseUser> manager = new CourseUserManager<CourseUser>(
                fakeUserStore.Object,
                null, null, null, null, null, null, null, null);

            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await manager.SetCourseUserRoleAsync(null, CourseUserRole.Manager));
        }

        private static CourseUser CreateUser(CourseUserRole role = CourseUserRole.User)
        {
            return new CourseUser
            {
                Username = "fake",
                Email = "fake@mail.com",
                Role = role,
            };
        }
    }
}
