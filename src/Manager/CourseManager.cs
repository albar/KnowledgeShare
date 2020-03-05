using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KnowledgeShare.Entity;
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
                throw new ValidationException(result.ErrorsBag);
            }

            await _store.CreateAsync(course, token);
        }

        public async ValueTask<Course> FindByIdAsync(string courseId, CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            return await _store.FindByIdAsync(courseId, token);
        }

        public IItemCollection<Course> GetItemCollection()
        {
            ThrowIfDisposed();
            return _store.GetItemCollection();
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

        public async Task InviteUserToAsync(Course course, ICourseUser user)
        {
            ThrowIfDisposed();

            if (course == null)
            {
                throw new ArgumentNullException(nameof(course));
            }

            var store = GetCourseInviteeStore();
            await store.InviteUserToAsync(course, user);
            await UpdateCourseAsync(course);
        }

        public async Task InviteUsersToAsync(Course course, IEnumerable<ICourseUser> users)
        {
            ThrowIfDisposed();

            if (course == null)
            {
                throw new ArgumentNullException(nameof(course));
            }

            var store = GetCourseInviteeStore();
            foreach (var user in users)
            {
                await store.InviteUserToAsync(course, user);
            }

            await UpdateCourseAsync(course);
        }

        public async Task AddFeedbackToAsync(
            Course course,
            ICourseUser user,
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
            await _store.UpdateAsync(course);
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
                throw new ValidationException(result.ErrorsBag);
            }

            await _store.UpdateAsync(course, token);
        }

        private async Task<ValidationResult> ValidateCourseAsync(Course course, CancellationToken token)
        {
            var succeeded = true;
            var errorsBag = new ValidationErrorsBag();

            foreach (var validator in _validators)
            {
                var result = await validator.ValidateAsync(this, course, token);
                if (!result.Succeeded)
                {
                    succeeded = false;
                    errorsBag.Append(result.ErrorsBag);
                }
            }

            return new ValidationResult(succeeded, errorsBag);
        }

        private ICourseInviteeStore GetCourseInviteeStore()
        {
            if (_store is ICourseInviteeStore store)
            {
                return store;
            }

            throw new NotSupportedException(
                $"Store is not supported to do this action. {nameof(ICourseInviteeStore)} is not implemented");
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

        protected void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }
    }
}
