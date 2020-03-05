using System.Threading;
using System.Threading.Tasks;
using KnowledgeShare.Store.Core;

namespace KnowledgeShare.Store.Abstractions
{
    public interface ICourseRegistrantStore
    {
        Task RegisterUserToAsync(
            Course course,
            ICourseUser user,
            CancellationToken token = default);
    }
}
