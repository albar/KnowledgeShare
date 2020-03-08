using System;
using System.Threading;
using System.Threading.Tasks;
using KnowledgeShare.Store.Core;
using KnowledgeShare.Store.Abstractions;
using Microsoft.AspNetCore.Identity;
using Moq;
using Xunit;
using KnowledgeShare.Manager.Exceptions;

namespace KnowledgeShare.Manager.Test
{
    public class CourseUserManagerTest
    {
        [Fact]
        public async Task Can_Change_User_Role()
        {
            var fakeCourseUserStore = new Mock<ICourseUserRoleStore>();
            fakeCourseUserStore.Setup(store => store.SetCourseUserRoleAsync(
                    It.IsAny<CourseUser>(),
                    It.IsAny<CourseUserRole>(),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            CourseUserManager manager = new CourseUserManager(
                fakeCourseUserStore.As<IUserStore<CourseUser>>().Object,
                null, null, null, null, null, null, null, null);

            await manager.SetCourseUserRoleAsync(CreateUser(), CourseUserRole.Manager);

            fakeCourseUserStore.Verify(store => store.SetCourseUserRoleAsync(
                    It.IsAny<CourseUser>(),
                    It.IsAny<CourseUserRole>(),
                    It.IsAny<CancellationToken>()),
                Times.Once());
        }

        [Fact]
        public async Task Throw_When_Changing_Role_With_Null_User()
        {
            var fakeUserStore = new Mock<IUserStore<CourseUser>>();

            CourseUserManager manager = new CourseUserManager(
                fakeUserStore.Object,
                null, null, null, null, null, null, null, null);

            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await manager.SetCourseUserRoleAsync(null, CourseUserRole.Manager));
        }

        [Fact]
        public async Task Throw_When_Trying_To_Set_Role_But_Store_Not_Implement_Related_Interface()
        {
            var fakeStore = new Mock<IUserStore<CourseUser>>();
            CourseUserManager manager = new CourseUserManager(
                fakeStore.Object,
                null, null, null, null, null, null, null, null);

            await Assert.ThrowsAsync<NotSupportedStoreException>(async () =>
                await manager.SetCourseUserRoleAsync(
                    CreateUser(),
                    CourseUserRole.Manager));
        }

        private static CourseUser CreateUser(CourseUserRole role = CourseUserRole.User)
        {
            return new CourseUser
            {
                UserName = "fake",
                Email = "fake@mail.com",
                Role = role,
            };
        }
    }
}
