using System;
using System.Threading;
using System.Threading.Tasks;
using KnowledgeShare.Entity;

namespace KnowledgeShare.Store.Abstractions
{
    public interface IFeedbackStore : IDisposable
    {
        Task CreateAsync(Feedback feedback, CancellationToken token = default);
    }
}
