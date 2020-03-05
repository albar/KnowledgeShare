using System;
using System.Threading;
using System.Threading.Tasks;
using KnowledgeShare.Entity;
using KnowledgeShare.Manager.Validation;
using KnowledgeShare.Manager.Validation.FeedbackValidators;
using KnowledgeShare.Store.Abstractions;
using Moq;
using Xunit;

namespace KnowledgeShare.Manager.Test
{
    public class FeedbackManagerTest
    {
        [Fact]
        public async Task Can_Validate_Create_Feedback()
        {
            var fakeStore = new Mock<IFeedbackStore>();
            fakeStore.Setup(store => store.CreateAsync(
                    It.IsAny<Feedback>(),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var fakeValidator = new Mock<IFeedbackValidator>();
            fakeValidator.Setup(validator => validator.ValidateAsync(
                    It.IsAny<FeedbackManager>(),
                    It.IsAny<Feedback>(),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(ValidationResult.Success));

            FeedbackManager manager = new FeedbackManager(
                fakeStore.Object,
                new IFeedbackValidator[] { fakeValidator.Object });

            await manager.CreateAsync(new Feedback
            {
                Course = new Course(),
                User = CreateUser(),
                Rate = FeedbackRate.Good,
                Note = null,
            });

            fakeStore.Verify(store => store.CreateAsync(
                    It.IsAny<Feedback>(),
                    It.IsAny<CancellationToken>()),
                Times.Once());

            fakeValidator.Verify(validator => validator.ValidateAsync(
                    It.IsAny<FeedbackManager>(),
                    It.IsAny<Feedback>(),
                    It.IsAny<CancellationToken>()),
                Times.Once());
        }

        [Fact]
        public async Task Throw_When_Validation_Failed()
        {
            var fakeStore = new Mock<IFeedbackStore>();

            var fakeValidator = new Mock<IFeedbackValidator>();
            fakeValidator.Setup(validator => validator.ValidateAsync(
                    It.IsAny<FeedbackManager>(),
                    It.IsAny<Feedback>(),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(ValidationResult.Failed));

            FeedbackManager manager = new FeedbackManager(
                fakeStore.Object,
                new IFeedbackValidator[] { fakeValidator.Object });

            await Assert.ThrowsAsync<ValidationException>(async () =>
                await manager.CreateAsync(new Feedback()));
        }

        [Fact]
        public async Task Throw_When_Passing_Null()
        {
            var fakeStore = new Mock<IFeedbackStore>();

            FeedbackManager manager = new FeedbackManager(
                fakeStore.Object,
                new IFeedbackValidator[] { });

            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await manager.CreateAsync(null));
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
