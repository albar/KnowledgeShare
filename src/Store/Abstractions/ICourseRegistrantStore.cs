using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KnowledgeShare.Store.Core;

namespace KnowledgeShare.Store.Abstractions
{
    public interface ICourseRegistrantStore
    {
        Task RegisterUserToAsync(
            Course course,
            CourseUser user,
            CancellationToken token = default);

        IQueryable<Registrant> GetRegistrants(Course course);
    }
}
