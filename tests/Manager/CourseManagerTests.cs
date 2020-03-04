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
            List<Session> sessions = new List<Session> { new Session() };

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
            List<Session> sessions = Enumerable.Range(0, sessionCount)
                .Select(_ => new Session())
                .ToList();

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

        [Theory]
        [InlineData(8, 1, 5, 5)]
        [InlineData(8, 2, 5, 3)]
        [InlineData(8, 3, 5, 0)]
        public async Task Can_List_And_Paginate_Courses(
            int total,
            int page,
            int limit,
            int resultCount)
        {
            ICourseUserManager userManager = new FakeCourseUserManager();
            ICourseUser author = await CreateUserAsync(userManager, CourseUserRole.Manager);

            List<Course> courses = Enumerable.Range(0, total).Select(_ =>
                    new Course { Author = author, Visibility = Visibility.Public })
                .ToList();

            var fakeCourseStore = new Mock<ICourseStore>();
            fakeCourseStore.SetupGet(s => s.Query).Returns(Queryable.AsQueryable(courses));

            ICourseManager courseManager = new CourseManager(userManager, fakeCourseStore.Object);
            Abstractions.ICollection<Course> collection = courseManager.GetAllAccessibleToUser(author);
            IPaginatedCollection<Course> paginatedCourses = await collection.PaginateAsync(page, limit);

            Assert.Equal(page, paginatedCourses.Page);
            Assert.Equal(limit, paginatedCourses.Limit);
            Assert.Equal(resultCount, paginatedCourses.Items.Count());
        }

        [Theory]
        [InlineData(CourseUserRole.Administrator, Visibility.Public)]
        [InlineData(CourseUserRole.Administrator, Visibility.Private)]
        [InlineData(CourseUserRole.Manager, Visibility.Public)]
        [InlineData(CourseUserRole.Manager, Visibility.Private, true)]
        [InlineData(CourseUserRole.User, Visibility.Public)]
        public async Task Can_Find_A_Course_By_id(
            CourseUserRole role,
            Visibility visibility,
            bool selfAuthor = false)
        {
            const string title = "A Course";

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

            IDictionary<string, Course> courses = new Course[]
            {
                new Course { Author = author},
                new Course { Author = author, Title = title},
            }.ToDictionary(c => c.Id, c => c);

            Course course = courses.Last().Value;

            var fakeCourseStore = new Mock<ICourseStore>();
            fakeCourseStore.Setup(s => s.FindByIdAsync(It.IsAny<string>()))
                .Returns<string>(id =>
                    Task.FromResult(courses.ContainsKey(id) ? courses[id] : null));

            ICourseManager courseManager = new CourseManager(userManager, fakeCourseStore.Object);
            Course foundCourse = await courseManager.FindAccessibleToUserByIdAsync(accessor, course.Id);

            Assert.Equal(title, foundCourse.Title);
            fakeCourseStore.Verify<Task<Course>>(s =>
                s.FindByIdAsync(It.IsAny<string>()),
                Times.Once());
        }

        [Theory]
        [InlineData(CourseUserRole.Administrator)]
        [InlineData(CourseUserRole.Administrator, true)]
        [InlineData(CourseUserRole.Manager, true)]
        public async Task Administrator_Or_Course_Author_Can_Update_A_Course(
            CourseUserRole accessorRole,
            bool selfAuthor = false)
        {
            ICourseUserManager userManager = new FakeCourseUserManager();
            ICourseUser accessor = await CreateUserAsync(userManager, accessorRole);
            ICourseUser author;
            if (selfAuthor)
            {
                author = accessor;
            }
            else
            {
                author = await CreateUserAsync(userManager, CourseUserRole.Manager);
            }

            const string initialTitle = "Initial Title";
            const string updatedTitle = "Updated Title";

            Course course = new Course
            {
                Author = author,
                Title = initialTitle,
            };

            var fakeCourseStore = new Mock<ICourseStore>();
            fakeCourseStore.Setup(s => s.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(course);
            fakeCourseStore.Setup(s => s.UpdateAsync(It.IsAny<Course>()))
                .Returns<Course>(c =>
                {
                    course.Title = c.Title;
                    return Task.FromResult(course);
                });

            ICourseManager courseManager = new CourseManager(userManager, fakeCourseStore.Object);

            Course updatedCourse = await courseManager.UpdateAccessibleToUserAsync(
                accessor,
                course.Id,
                new UpdatableCourse
                {
                    Title = updatedTitle,
                });

            Assert.Equal(updatedTitle, updatedCourse.Title);
            fakeCourseStore.Verify(s => s.UpdateAsync(It.IsAny<Course>()), Times.Once());
        }

        private static async Task<ICourseUser> CreateUserAsync(
            ICourseUserManager manager,
            CourseUserRole role)
        {
            return await manager.CreateAsync("fakename", "fake@mail.com", role);
        }
    }
}