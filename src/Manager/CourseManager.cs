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
            ThrowIfDisposed();
            token.ThrowIfCancellationRequested();

            if (course == null)
            {
                throw new ArgumentNullException("course");
            }

            var result = await ValidateCourseAsync(course, token);
            if (!result.Succeeded)
            {
                throw new ValidationException(result.ErrorsBag);
            }

            await _store.CreateAsync(course, token);
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
