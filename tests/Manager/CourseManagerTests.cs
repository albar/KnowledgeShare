using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KnowledgeShare.Entity;
using KnowledgeShare.Manager.Validation;
using KnowledgeShare.Manager.Validation.CourseValidators;
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
        public async Task Can_Create_A_Course(CourseUserRole role)
        {
            const string title = "A title";
            const string description = "A description";
            ICourseUser author = CreateUser(role);
            ICourseUser speaker = CreateUser();
            ILocation location = CreateOnlineLocation();
            List<Session> sessions = new List<Session> { new Session() };
            Visibility visibility = Visibility.Public;

            var fakeStore = new Mock<ICourseStore>();
            fakeStore.Setup(s => s.CreateAsync(It.IsAny<Course>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            CourseManager manager = new CourseManager(fakeStore.Object, new ICourseValidator[] { });
            await manager.CreateAsync(new Course
            {
                Author = author,
                Title = title,
                Speaker = speaker,
                Description = description,
                Location = location,
                Sessions = sessions,
                Visibility = visibility,
            });

            fakeStore.VerifyAll();
        }

        [Theory]
        [InlineData(CourseUserRole.User, "Title", true, 1, 1)]
        [InlineData(CourseUserRole.User, null, true, 1, 2)]
        [InlineData(CourseUserRole.User, null, false, 1, 3)]
        [InlineData(CourseUserRole.User, null, false, 0, 4)]
        [InlineData(CourseUserRole.Manager, null, true, 1, 1)]
        [InlineData(CourseUserRole.Manager, null, false, 1, 2)]
        [InlineData(CourseUserRole.Manager, null, false, 0, 3)]
        [InlineData(CourseUserRole.Manager, "Title", false, 1, 1)]
        [InlineData(CourseUserRole.Manager, "Title", false, 0, 2)]
        [InlineData(CourseUserRole.Manager, "Title", true, 0, 1)]
        [InlineData(CourseUserRole.Administrator, null, true, 1, 1)]
        [InlineData(CourseUserRole.Administrator, null, false, 1, 2)]
        [InlineData(CourseUserRole.Administrator, null, false, 0, 3)]
        [InlineData(CourseUserRole.Administrator, "Title", false, 1, 1)]
        [InlineData(CourseUserRole.Administrator, "Title", false, 0, 2)]
        [InlineData(CourseUserRole.Administrator, "Title", true, 0, 1)]
        public async Task Throw_ValidationException_On_Invalid_Data(
            CourseUserRole authorRole,
            string title,
            bool withLocation,
            int sessionsCount,
            int errorsCount)
        {
            var fakeStore = new Mock<ICourseStore>();
            fakeStore.Setup(s => s.CreateAsync(It.IsAny<Course>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var fakeValidator = new Mock<ICourseValidator>();
            fakeValidator.Setup(validator => validator.ValidateAsync(It.IsAny<CourseManager>(), It.IsAny<Course>()))
                .Returns<CourseManager, Course>((_, course) =>
                {
                    ValidationErrorsBag bag = new ValidationErrorsBag();
                    if (course.Author == null || course.Author.Role == CourseUserRole.User)
                    {
                        bag.Add("Author");
                    }
                    if (string.IsNullOrWhiteSpace(course.Title))
                    {
                        bag.Add("Title");
                    }
                    if (course.Location == null)
                    {
                        bag.Add("Location");
                    }
                    if (course.Sessions == null || course.Sessions.Count < 1)
                    {
                        bag.Add("Session");
                    }

                    if (bag.Count > 0)
                    {
                        return Task.FromResult(ValidationResult.Failed(bag));
                    }

                    return Task.FromResult(ValidationResult.Success);
                });

            ICourseUser author = CreateUser(authorRole);
            ICourseUser speaker = CreateUser();
            ILocation location = null;
            if (withLocation)
            {
                location = CreateOnlineLocation();
            }
            List<Session> sessions = Enumerable.Range(0, sessionsCount).Select(_ => new Session()).ToList();

            CourseManager manager = new CourseManager(fakeStore.Object, new ICourseValidator[] { fakeValidator.Object });

            ValidationException exception = await Assert.ThrowsAsync<ValidationException>(async () =>
                await manager.CreateAsync(new Course
                {
                    Author = author,
                    Title = title,
                    Speaker = speaker,
                    Location = location,
                    Sessions = sessions,
                    Visibility = Visibility.Public,
                }));

            Assert.Equal(errorsCount, exception.ErrorsBag.Count);
        }

        [Fact]
        public async Task Throw_ArgumentNullException_When_Passing_Null_Value()
        {
            var fakeStore = new Mock<ICourseStore>();
            CourseManager manager = new CourseManager(fakeStore.Object, new ICourseValidator[] { });

            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await manager.CreateAsync(null));
        }

        private static ILocation CreateOnlineLocation()
        {
            return new OnlineLocation
            {
                Url = "http://localhost",
                Note = "",
            };
        }

        private static ICourseUser CreateUser(CourseUserRole role = CourseUserRole.User)
        {
            return new CourseUser
            {
                Username = "fake",
                Email = "fake@mail.com",
                Role = role,
            };
        }

        private class CourseUser : ICourseUser
        {
            public string Id { get; } = Guid.NewGuid().ToString();
            public string Username { get; set; }
            public string Email { get; set; }
            public CourseUserRole Role { get; set; }
        }
    }
}
