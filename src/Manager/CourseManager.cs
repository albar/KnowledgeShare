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
    public class CourseManager
    {
        private readonly ICourseStore _store;
        private readonly IEnumerable<ICourseValidator> _validators;

        public CourseManager(ICourseStore store, IEnumerable<ICourseValidator> validators)
        {
            _store = store;
            _validators = validators;
        }

        public async Task CreateAsync(Course course, CancellationToken token = default)
        {
            if (course == null)
            {
                throw new ArgumentNullException("course");
            }

            var result = await ValidateCourseAsync(course);
            if (!result.Succeeded)
            {
                throw new ValidationException(result.ErrorsBag);
            }

            await _store.CreateAsync(course, token);
        }

        private async Task<ValidationResult> ValidateCourseAsync(Course course)
        {
            var errorsBag = new ValidationErrorsBag();

            foreach (var validator in _validators)
            {
                var result = await validator.ValidateAsync(this, course);
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
    }
}
