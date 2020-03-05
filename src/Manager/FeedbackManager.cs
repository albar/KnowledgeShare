using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using KnowledgeShare.Entity;
using KnowledgeShare.Manager.Validation;
using KnowledgeShare.Manager.Validation.FeedbackValidators;
using KnowledgeShare.Store.Abstractions;

namespace KnowledgeShare.Manager
{
    public class FeedbackManager : IDisposable
    {
        private readonly IFeedbackStore _store;
        private readonly IEnumerable<IFeedbackValidator> _validators;
        private bool _disposed;

        public FeedbackManager(
            IFeedbackStore store,
            IEnumerable<IFeedbackValidator> validators)
        {
            _store = store;
            _validators = validators;
        }

        public async Task CreateAsync(Feedback feedback, CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            if (feedback == null)
            {
                throw new ArgumentNullException(nameof(feedback));
            }

            var result = await ValidateFeedbackAsync(feedback);
            if (!result.Succeeded)
            {
                throw new ValidationException(result.ErrorsBag);
            }

            await _store.CreateAsync(feedback, token);
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _store.Dispose();
                _disposed = true;
            }
        }

        private async Task<ValidationResult> ValidateFeedbackAsync(
            Feedback feedback,
            CancellationToken token = default)
        {
            var succeeded = true;
            var errorsBag = new ValidationErrorsBag();

            foreach (var validator in _validators)
            {
                var result = await validator.ValidateAsync(this, feedback, token);
                if (!result.Succeeded)
                {
                    succeeded = false;
                    errorsBag.Append(result.ErrorsBag);
                }
            }

            return new ValidationResult(succeeded, errorsBag);
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }
    }
}
