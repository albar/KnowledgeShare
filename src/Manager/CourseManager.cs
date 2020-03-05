using System;
using System.Collections.Generic;
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

        public async Task UpdateAsync(Course course, CancellationToken token = default)
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

            await _store.UpdateAsync(course, token);
        }

        public async Task InviteUserToAsync(Course course, ICourseUser user)
        {
            ThrowIfDisposed();

            if (course == null)
            {
                throw new ArgumentNullException(nameof(course));
            }

            await _store.InviteUserToAsync(course, user);
            await UpdateAsync(course);
        }

        public async Task InviteUsersToAsync(Course course, IEnumerable<ICourseUser> users)
        {
            ThrowIfDisposed();

            if (course == null)
            {
                throw new ArgumentNullException(nameof(course));
            }

            foreach (var user in users)
            {
                await _store.InviteUserToAsync(course, user);
            }

            await UpdateAsync(course);
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

        private async Task<ValidationResult> ValidateCourseAsync(Course course, CancellationToken token)
        {
            var errorsBag = new ValidationErrorsBag();

            foreach (var validator in _validators)
            {
                var result = await validator.ValidateAsync(this, course, token);
                if (!result.Succeeded)
                {
                    errorsBag.Append(result.ErrorsBag);
                }
            }

            if (errorsBag.Count > 0)
            {
                return ValidationResult.Failed(errorsBag);
            }

            return ValidationResult.Success;
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
