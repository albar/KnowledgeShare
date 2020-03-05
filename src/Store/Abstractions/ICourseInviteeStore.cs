using System.Threading;
using System.Threading.Tasks;
using KnowledgeShare.Entity;

namespace KnowledgeShare.Store.Abstractions
{
    public interface ICourseInviteeStore
    {
        Task InviteUserToAsync(
            Course course,
            ICourseUser user,
            CancellationToken token = default);
    }
}
