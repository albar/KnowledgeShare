using System.Threading;
using System.Threading.Tasks;
using KnowledgeShare.Entity;

namespace KnowledgeShare.Manager.Validation.FeedbackValidators
{
    public interface IFeedbackValidator
    {
        Task<ValidationResult> ValidateAsync(
            FeedbackManager manager,
            Feedback feedback,
            CancellationToken token = default);
    }
}
