using System.Threading;
using System.Threading.Tasks;
using KnowledgeShare.Store.Core;

namespace KnowledgeShare.Manager.Validation.CourseValidators
{
    public interface ICourseValidator
    {
        Task<ValidationResult> ValidateAsync(
            CourseManager manager,
            Course course,
            CancellationToken token = default);
    }
}
