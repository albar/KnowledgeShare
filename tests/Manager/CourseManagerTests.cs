using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KnowledgeShare.Entity;
using KnowledgeShare.Manager.Abstractions;
using KnowledgeShare.Manager.Exceptions;
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

            ICourseUser author = await CreateUserAsync(userManager, role);
            const string title = "A Course";
            ICourseUser speaker = await CreateUserAsync(userManager, CourseUserRole.User);
            const string description = "A Description";
            ILocation location = new OnlineLocation
            {
                Url = "http://localhost",
                Note = "",
            };
            Visibility visibility = Visibility.Public;
            Session[] sessions = new Session[] { new Session() };

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

        [Theory]
        [InlineData(CourseUserRole.User, null, null, null, false, Visibility.Public, 0, new string[] { "author", "title", "speaker", "location", "sessions" })]
        [InlineData(CourseUserRole.Administrator, null, null, null, false, Visibility.Public, 0, new string[] { "title", "speaker", "location", "sessions" })]
        [InlineData(CourseUserRole.Manager, null, null, null, false, Visibility.Public, 0, new string[] { "title", "speaker", "location", "sessions" })]
        public async Task Can_Not_Create_Course_With_Invalid_Data(
            CourseUserRole role,
            string title,
            string description,
            ICourseUser speaker,
            bool withLocation,
            Visibility visibility,
            int sessionCount,
            string[] errorKeys)
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

            ICourseUser author = await userManager.CreateAsync(
                "admin", "admin@test.com", role);
            ILocation location = null;
            if (withLocation)
            {
                location = new OnlineLocation
                {
                    Url = "http://localhost",
                    Note = "",
                };
            }
            Session[] sessions = Enumerable.Range(0, sessionCount)
                .Select(_ => new Session())
                .ToArray();

            ValidationException exception = await Assert
                .ThrowsAsync<ValidationException>(async () =>
                    await courseManager.CreateAsync(
                    author,
                    title,
                    speaker,
                    description,
                    location,
                    visibility,
                    sessions
                ));

            int capturedErrorsCount = exception.ErrorsBag.Keys
                .Intersect(errorKeys)
                .Count();

            Assert.Equal(errorKeys.Length, capturedErrorsCount);

            fakeCourseStore.Verify<Task<Course>>(
                s => s.CreateAsync(It.IsAny<Course>()),
                Times.Never);
        }

        [Theory]
        [InlineData(CourseUserRole.Administrator, 2)]
        [InlineData(CourseUserRole.Manager, 1)]
        [InlineData(CourseUserRole.Manager, 2, true)]
        [InlineData(CourseUserRole.User, 1)]
        public async Task Can_List_Courses_With_Respecting_User_Role(
            CourseUserRole role,
            int accessibleCount,
            bool selfAuthor = false)
        {
            ICourseUserManager userManager = new FakeCourseUserManager();
            ICourseUser accessor = await CreateUserAsync(userManager, role);
            ICourseUser author;
            if (selfAuthor)
            {
                author = accessor;
            }
            else
            {
                author = await CreateUserAsync(userManager, CourseUserRole.Manager);
            }

            List<Course> courses = new List<Course>
            {
                new Course {Author = author, Visibility = Visibility.Public},
                new Course {Author = author, Visibility = Visibility.Private},
            };

            var fakeCourseStore = new Mock<ICourseStore>();
            fakeCourseStore.SetupGet(s => s.Query).Returns(Queryable.AsQueryable(courses));

            ICourseManager courseManager = new CourseManager(userManager, fakeCourseStore.Object);
            Abstractions.ICollection<Course> collection = courseManager.GetAllAccessibleToUser(accessor);
            List<Course> accessibleCourses = await collection.ToListAsync();

            Assert.Equal(accessibleCount, accessibleCourses.Count);

            fakeCourseStore.Verify<IQueryable<Course>>(s => s.Query, Times.Once());
        }

        private static async Task<ICourseUser> CreateUserAsync(
            ICourseUserManager manager,
            CourseUserRole role)
        {
            return await manager.CreateAsync("fakename", "fake@mail.com", role);
        }
    }
}