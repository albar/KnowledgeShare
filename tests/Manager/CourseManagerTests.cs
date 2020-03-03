using System.Linq;
using System.Threading.Tasks;
using KnowledgeShare.Entity;
using KnowledgeShare.Manager.Abstractions;
using KnowledgeShare.Manager.Test.Fakers;
using KnowledgeShare.Store.Abstractions;
using Moq;
using Xunit;

namespace KnowledgeShare.Manager.Test
{
    public class CourseManagerTests
    {
        [Fact]
        public async Task Admin_Can_Create_Course()
        {
            var fakeCourseStore = new Mock<ICourseStore>();
            fakeCourseStore.Setup(s => s.CreateAsync(It.IsAny<Course>()))
            .Returns<Course>(course =>
            {
                return Task.FromResult(course);
            });

            ICourseUserManager userManager = new FakeCourseUserManager();
            ICourseRoleManager roleManager = new FakeCourseRoleManager();

            ICourseRole adminRole = await roleManager.CreateAsync("Administrator");
            ICourseRole userRole = await roleManager.CreateAsync("User");

            ICourseManager courseManager = new CourseManager(
                userManager,
                fakeCourseStore.Object);

            ICourseUser admin = await userManager.CreateAsync("admin", "admin@test.com", adminRole);
            const string title = "A Course";
            ICourseUser speaker = await userManager.CreateAsync("user", "user@test.com", userRole);
            const string description = "A Description";
            ILocation location = new OnlineLocation
            {
                Url = "http://localhost",
                Note = "",
            };
            Visibility visibility = Visibility.Public;
            Session[] sessions = new Session[] { };

            Course course = await courseManager.CreateAsync(
                admin,
                title,
                speaker,
                description,
                location,
                visibility,
                sessions
            );

            Assert.Equal(adminRole.Id, course.Author.Id);
            Assert.Equal(title, course.Title);
            Assert.Equal(speaker.Id, course.Speaker.Id);
            Assert.Equal(description, course.Description);
            Assert.Equal(location, course.Location);
            Assert.True(sessions.SequenceEqual(course.Sessions));

            fakeCourseStore.Verify(
                s => s.CreateAsync(It.IsAny<Course>()),
                Times.Once());
        }
    }
}