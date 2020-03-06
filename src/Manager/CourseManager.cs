using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KnowledgeShare.Store.Core;
using KnowledgeShare.Manager.Validation;
using KnowledgeShare.Manager.Validation.CourseValidators;
using KnowledgeShare.Store.Abstractions;

namespace KnowledgeShare.Manager
{
    public class CourseManager : IDisposable
    {
        private readonly ICourseStore _store;
        private readonly IEnumerable<ICourseValidator> _validators;
        private bool _disposed;

        public CourseManager(ICourseStore store, IEnumerable<ICourseValidator> validators)
        {
            _store = store;
            _validators = validators;
        }

        public IQueryable<Course> Courses => GetQueryableStore().Items;

        public async Task CreateAsync(Course course, CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            if (course == null)
            {
                throw new ArgumentNullException(nameof(course));
            }

            var result = await ValidateCourseAsync(course, token);
            if (!result.Succeeded)
            {
                throw new ValidationException(result.Errors);
            }

            await _store.CreateAsync(course, token);
        }

        public async ValueTask<Course> FindByIdAsync(string courseId, CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            return await _store.FindByIdAsync(courseId, token);
        }

        public async Task UpdateAsync(Course course, CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            if (course == null)
            {
                throw new ArgumentNullException(nameof(course));
            }

            await UpdateCourseAsync(course, token);
        }

        public async Task RegisterUserToAsync(Course course, CourseUser user)
        {
            ThrowIfDisposed();

            if (course == null)
            {
                throw new ArgumentNullException(nameof(course));
            }
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var store = GetCourseRegistrantStore();
            await store.RegisterUserToAsync(course, user);
            await UpdateCourseAsync(course);
        }

        public async Task RegisterUsersToAsync(Course course, IEnumerable<CourseUser> users)
        {
            ThrowIfDisposed();

            if (course == null)
            {
                throw new ArgumentNullException(nameof(course));
            }
            if (users == null)
            {
                throw new ArgumentNullException(nameof(users));
            }

            var store = GetCourseRegistrantStore();
            foreach (var user in users.Where(user => user is { }))
            {
                await store.RegisterUserToAsync(course, user);
            }

            await UpdateCourseAsync(course);
        }

        public IQueryable<Registrant> GetReigstrants(Course course)
        {
            ThrowIfDisposed();

            if (course == null)
            {
                throw new ArgumentNullException(nameof(course));
            }

            var store = GetCourseRegistrantStore();
            return store.GetRegistrants(course);
        }

        public async Task AddFeedbackToAsync(
            Course course,
            CourseUser user,
            FeedbackRate rate,
            string message,
            CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            if (course == null)
            {
                throw new ArgumentNullException(nameof(course));
            }
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var store = GetCourseFeedbackStore();

            await store.AddFeedbackToAsync(course, user, rate, message, token);
            await UpdateCourseAsync(course);
        }

        public async Task RemoveAsync(Course course, CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            if (course == null)
            {
                throw new ArgumentNullException(nameof(course));
            }

            await _store.RemoveAsync(course);
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _store.Dispose();
                _disposed = true;
            }
        }

        private async Task UpdateCourseAsync(Course course, CancellationToken token = default)
        {
            var result = await ValidateCourseAsync(course, token);
            if (!result.Succeeded)
            {
                throw new ValidationException(result.Errors);
            }

            await _store.UpdateAsync(course, token);
        }

        private async Task<ValidationResult> ValidateCourseAsync(Course course, CancellationToken token)
        {
            var succeeded = true;
            var errors = new List<ValidationError>();

            foreach (var validator in _validators)
            {
                var result = await validator.ValidateAsync(this, course, token);
                if (!result.Succeeded)
                {
                    succeeded = false;
                    errors.AddRange(result.Errors);
                }
            }

            return new ValidationResult(succeeded, errors);
        }

        private ICourseRegistrantStore GetCourseRegistrantStore()
        {
            if (_store is ICourseRegistrantStore store)
            {
                return store;
            }

            throw new NotSupportedException(
                $"Store is not supported to do this action. {nameof(ICourseRegistrantStore)} is not implemented");
        }

        private ICourseFeedbackStore GetCourseFeedbackStore()
        {
            if (_store is ICourseFeedbackStore store)
            {
                return store;
            }

            throw new NotSupportedException(
                $"Store is not supported to do this action. {nameof(ICourseFeedbackStore)} is not implemented");
        }

        private IQueryableStore<Course> GetQueryableStore()
        {
            if (_store is IQueryableStore<Course> store)
            {
                return store;
            }

            throw new NotSupportedException(
                $"Store is not supported to do this action. {nameof(IQueryableStore<Course>)} is not implemented");
        }

        protected void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }
    }
}
