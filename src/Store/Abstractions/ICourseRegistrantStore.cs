using System.Threading;
using System.Threading.Tasks;
using KnowledgeShare.Entity;

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
