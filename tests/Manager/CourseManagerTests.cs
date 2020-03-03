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
        [Theory]
        [InlineData(CourseUserRole.Administrator)]
        [InlineData(CourseUserRole.Manager)]
        public async Task Can_Create_Course_With_Valid_Data(CourseUserRole role)
        {
            var fakeCourseStore = new Mock<ICourseStore>();
            fakeCourseStore.Setup(s => s.CreateAsync(It.IsAny<Course>()))
            .Returns<Course>(course =>
            {
                return Task.FromResult(course);
            });

            ICourseUserManager userManager = new FakeCourseUserManager();

            ICourseManager courseManager = new CourseManager(
                userManager,
                fakeCourseStore.Object);

            ICourseUser author = await userManager.CreateAsync("admin", "admin@test.com", role);
            const string title = "A Course";
            ICourseUser speaker = await userManager.CreateAsync("user", "user@test.com", CourseUserRole.User);
            const string description = "A Description";
            ILocation location = new OnlineLocation
            {
                Url = "http://localhost",
                Note = "",
            };
            Visibility visibility = Visibility.Public;
            Session[] sessions = new Session[] { };

            Course course = await courseManager.CreateAsync(
                author,
                title,
                speaker,
                description,
                location,
                visibility,
                sessions
            );

            Assert.Equal(author.Id, course.Author.Id);
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