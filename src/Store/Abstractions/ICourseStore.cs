using System.Threading;
using System.Threading.Tasks;
using KnowledgeShare.Entity;

namespace KnowledgeShare.Store.Abstractions
{
    public interface ICourseStore
    {
        Task CreateAsync(Course course, CancellationToken token = default);
    }
}
