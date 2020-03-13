using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using KnowledgeShare.Store.Core;

namespace KnowledgeShare.Manager.Validation.CourseValidators
{
    public class DefaultCourseValidator : ICourseValidator
    {
        public Task<ValidationResult> ValidateAsync(
            CourseManager manager,
            Course course,
            CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();

            var errors = new List<ValidationError>();

            if (string.IsNullOrWhiteSpace(course.Title))
            {
                errors.Add(ValidationError.Create(
                    nameof(course.Title),
                    "Course should have a title"));
            }
            if (course.Author == null)
            {
                errors.Add(ValidationError.Create(
                    nameof(course.Author),
                    "Course should have an author"));
            }
            if (course.Speaker == null)
            {
                errors.Add(ValidationError.Create(
                    nameof(course.Author),
                    "Course should have a speaker"));
            }
            if (course.Sessions.Count == 0)
            {
                errors.Add(ValidationError.Create(
                    nameof(course.Author),
                    "Course should at least have a session"));
            }

            // add location validation

            return Task.FromResult(ValidationResult.FromErrors(errors));
        }
    }
}
