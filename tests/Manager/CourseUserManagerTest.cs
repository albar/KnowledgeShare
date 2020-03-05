using System;
using System.Threading;
using System.Threading.Tasks;
using KnowledgeShare.Entity;
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
                    It.IsAny<ICourseUser>(),
                    It.IsAny<CourseUserRole>(),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            CourseUserManager<FakeCourseUser> manager = new CourseUserManager<FakeCourseUser>(
                fakeCourseUserStore.As<IUserStore<FakeCourseUser>>().Object,
                null, null, null, null, null, null, null, null);

            var user = CreateUser();
            await manager.SetCourseUserRoleAsync(user, CourseUserRole.Manager);

            fakeCourseUserStore.Verify(store => store.SetCourseUserRoleAsync(
                    It.IsAny<ICourseUser>(),
                    It.IsAny<CourseUserRole>(),
                    It.IsAny<CancellationToken>()),
                Times.Once());
        }

        [Fact]
        public async Task Throw_When_Changing_Role_With_Null()
        {
            var fakeUserStore = new Mock<IUserStore<FakeCourseUser>>();

            CourseUserManager<FakeCourseUser> manager = new CourseUserManager<FakeCourseUser>(
                fakeUserStore.Object,
                null, null, null, null, null, null, null, null);

            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await manager.SetCourseUserRoleAsync(null, CourseUserRole.Manager));
        }

        private static FakeCourseUser CreateUser(CourseUserRole role = CourseUserRole.User)
        {
            return new FakeCourseUser
            {
                Username = "fake",
                Email = "fake@mail.com",
                Role = role,
            };
        }

        public class FakeCourseUser : ICourseUser
        {
            public string Id { get; } = Guid.NewGuid().ToString();

            public string Username { get; set; }
            public string Email { get; set; }
            public CourseUserRole Role { get; set; }
        }
    }
}
