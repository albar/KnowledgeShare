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
            var fakeStore = new Mock<ICourseStore>();
            fakeStore.Setup(store => store.CreateAsync(
                    It.IsAny<Course>(),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            CourseManager manager = new CourseManager(
                fakeStore.Object,
                new ICourseValidator[] { });

            await manager.CreateAsync(new Course());

            fakeStore.Verify(store => store.CreateAsync(
                    It.IsAny<Course>(),
                    It.IsAny<CancellationToken>()),
                Times.Once());
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
        public async Task Shoul_Validate_The_Course_When_Creating(
            CourseUserRole authorRole,
            string title,
            bool withLocation,
            int sessionsCount,
            int errorsCount)
        {
            var fakeStore = new Mock<ICourseStore>();
            fakeStore.Setup(store => store.CreateAsync(
                    It.IsAny<Course>(),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var fakeValidator = new Mock<ICourseValidator>();
            fakeValidator.Setup(validator => validator.ValidateAsync(
                    It.IsAny<CourseManager>(),
                    It.IsAny<Course>(),
                    It.IsAny<CancellationToken>()))
                .Returns<CourseManager, Course, CancellationToken>((_, course, __) =>
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
            List<Session> sessions = Enumerable.Range(0, sessionsCount)
                .Select(_ => new Session())
                .ToList();

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

            fakeStore.Verify(store => store.CreateAsync(
                    It.IsAny<Course>(),
                    It.IsAny<CancellationToken>()),
                Times.Never());

            fakeValidator.Verify(validator => validator.ValidateAsync(
                    It.IsAny<CourseManager>(),
                    It.IsAny<Course>(),
                    It.IsAny<CancellationToken>()),
                Times.Once());
        }

        [Fact]
        public async Task Throw_When_Creating_Null_Course()
        {
            var fakeStore = new Mock<ICourseStore>();
            CourseManager manager = new CourseManager(
                fakeStore.Object,
                new ICourseValidator[] { });

            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await manager.CreateAsync(null));
        }

        [Fact]
        public async Task Can_Find_A_Course_By_Id()
        {
            Course course = new Course { };

            var fakeStore = new Mock<ICourseStore>();
            fakeStore.Setup(store => store.FindByIdAsync(
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>()))
                .Returns<string, CancellationToken>((id, _) =>
                    new ValueTask<Course>(id == course.Id ? course : null));

            CourseManager manager = new CourseManager(
                fakeStore.Object,
                new ICourseValidator[] { });

            Course foundCourse = await manager.FindByIdAsync(course.Id);

            Assert.Equal(course.Id, foundCourse.Id);

            fakeStore.Verify(store => store.FindByIdAsync(
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>()),
                Times.Once());
        }

        [Fact]
        public async Task Can_Update_A_Course()
        {
            var fakeStore = new Mock<ICourseStore>();
            fakeStore.Setup(store => store.UpdateAsync(
                    It.IsAny<Course>(),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var fakeValidator = new Mock<ICourseValidator>();
            fakeValidator.Setup(validator => validator.ValidateAsync(
                    It.IsAny<CourseManager>(),
                    It.IsAny<Course>(),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(ValidationResult.Success));

            CourseManager manager = new CourseManager(
                fakeStore.Object,
                new ICourseValidator[] { fakeValidator.Object });

            await manager.UpdateAsync(new Course());

            fakeStore.Verify(store => store.UpdateAsync(
                    It.IsAny<Course>(),
                    It.IsAny<CancellationToken>()),
                Times.Once());

            fakeValidator.Verify(validator => validator.ValidateAsync(
                    It.IsAny<CourseManager>(),
                    It.IsAny<Course>(),
                    It.IsAny<CancellationToken>()),
                Times.Once());
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
        public async Task Should_Validate_The_Course_When_Updating(
            CourseUserRole authorRole,
            string title,
            bool withLocation,
            int sessionsCount,
            int errorsCount)
        {
            var fakeStore = new Mock<ICourseStore>();
            fakeStore.Setup(store => store.UpdateAsync(
                    It.IsAny<Course>(),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var fakeValidator = new Mock<ICourseValidator>();
            fakeValidator.Setup(validator => validator.ValidateAsync(
                    It.IsAny<CourseManager>(),
                    It.IsAny<Course>(),
                    It.IsAny<CancellationToken>()))
                .Returns<CourseManager, Course, CancellationToken>((_, course, __) =>
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
            List<Session> sessions = Enumerable.Range(0, sessionsCount)
                .Select(_ => new Session())
                .ToList();

            CourseManager manager = new CourseManager(fakeStore.Object, new ICourseValidator[] { fakeValidator.Object });

            ValidationException exception = await Assert.ThrowsAsync<ValidationException>(async () =>
                await manager.UpdateAsync(new Course
                {
                    Author = author,
                    Title = title,
                    Speaker = speaker,
                    Location = location,
                    Sessions = sessions,
                    Visibility = Visibility.Public,
                }));

            Assert.Equal(errorsCount, exception.ErrorsBag.Count);

            fakeStore.Verify(store => store.UpdateAsync(
                    It.IsAny<Course>(),
                    It.IsAny<CancellationToken>()),
                Times.Never());

            fakeValidator.Verify(validator => validator.ValidateAsync(
                    It.IsAny<CourseManager>(),
                    It.IsAny<Course>(),
                    It.IsAny<CancellationToken>()),
                Times.Once());
        }

        [Fact]
        public async Task Throw_When_Updating_Null_Course()
        {
            var fakeStore = new Mock<ICourseStore>();
            CourseManager manager = new CourseManager(
                fakeStore.Object,
                new ICourseValidator[] { });

            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await manager.UpdateAsync(null));
        }

        [Fact]
        public async Task Can_Invite_User_To_A_Course()
        {
            var fakeStore = new Mock<ICourseStore>();

            fakeStore.Setup(store => store.InviteUserToAsync(
                    It.IsAny<Course>(),
                    It.IsAny<ICourseUser>(),
                    It.IsAny<CancellationToken>()))
                .Returns<Course, ICourseUser, CancellationToken>((course, user, _) =>
                {
                    course.Invitee.Add(new Invitee
                    {
                        Course = course,
                        User = user,
                    });

                    return Task.CompletedTask;
                });

            fakeStore.Setup(store => store.UpdateAsync(
                    It.IsAny<Course>(),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var fakeValidator = new Mock<ICourseValidator>();
            fakeValidator.Setup(validator => validator.ValidateAsync(
                    It.IsAny<CourseManager>(),
                    It.IsAny<Course>(),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(ValidationResult.Success));

            CourseManager manager = new CourseManager(
                fakeStore.Object,
                new ICourseValidator[] { fakeValidator.Object });

            ICourseUser user = CreateUser();
            Course course = new Course();

            await manager.InviteUserToAsync(course, user);

            Assert.True(course.Invitee.Any(invitee => invitee.User.Id == user.Id));

            fakeStore.Verify(store => store.InviteUserToAsync(
                    It.IsAny<Course>(),
                    It.IsAny<ICourseUser>(),
                    It.IsAny<CancellationToken>()),
                Times.Once());

            fakeStore.Verify(store => store.UpdateAsync(
                    It.IsAny<Course>(),
                    It.IsAny<CancellationToken>()),
                Times.Once());

            fakeValidator.Verify(validator => validator.ValidateAsync(
                    It.IsAny<CourseManager>(),
                    It.IsAny<Course>(),
                    It.IsAny<CancellationToken>()),
                Times.Once());
        }

        [Fact]
        public async Task Can_Invite_Multiple_Users_To_A_Course()
        {
            const int usersCount = 10;

            var fakeStore = new Mock<ICourseStore>();

            fakeStore.Setup(store => store.InviteUserToAsync(
                    It.IsAny<Course>(),
                    It.IsAny<ICourseUser>(),
                    It.IsAny<CancellationToken>()))
                .Returns<Course, ICourseUser, CancellationToken>((course, user, _) =>
                {
                    course.Invitee.Add(new Invitee
                    {
                        Course = course,
                        User = user,
                    });

                    return Task.CompletedTask;
                });

            fakeStore.Setup(store => store.UpdateAsync(
                    It.IsAny<Course>(),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var fakeValidator = new Mock<ICourseValidator>();
            fakeValidator.Setup(validator => validator.ValidateAsync(
                    It.IsAny<CourseManager>(),
                    It.IsAny<Course>(),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(ValidationResult.Success));

            CourseManager manager = new CourseManager(
                fakeStore.Object,
                new ICourseValidator[] { fakeValidator.Object });

            IEnumerable<ICourseUser> users = Enumerable.Range(0, usersCount)
                .Select(_ => CreateUser());

            Course course = new Course();

            Assert.Equal(0, course.Invitee.Count);

            await manager.InviteUsersToAsync(course, users);

            Assert.Equal(usersCount, course.Invitee.Count);

            fakeStore.Verify(store => store.InviteUserToAsync(
                    It.IsAny<Course>(),
                    It.IsAny<ICourseUser>(),
                    It.IsAny<CancellationToken>()),
                Times.Exactly(10));

            fakeStore.Verify(store => store.UpdateAsync(
                    It.IsAny<Course>(),
                    It.IsAny<CancellationToken>()),
                Times.Once());

            fakeValidator.Verify(validator => validator.ValidateAsync(
                    It.IsAny<CourseManager>(),
                    It.IsAny<Course>(),
                    It.IsAny<CancellationToken>()),
                Times.Once());
        }

        [Fact]
        public async Task Throw_When_Inviting_User_With_Null_Course()
        {
            var fakeStore = new Mock<ICourseStore>();

            CourseManager manager = new CourseManager(
                fakeStore.Object,
                new ICourseValidator[] { });

            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await manager.InviteUserToAsync(null, CreateUser()));
        }


        [Fact]
        public async Task Throw_When_Inviting_Multiple_Users_With_Null_Course()
        {
            var fakeStore = new Mock<ICourseStore>();

            CourseManager manager = new CourseManager(
                fakeStore.Object,
                new ICourseValidator[] { });

            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await manager.InviteUsersToAsync(
                    null,
                    new ICourseUser[] { CreateUser() }));
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
