using System.Linq;
using System.Threading.Tasks;
using KnowledgeShare.Entity;

namespace KnowledgeShare.Store.Abstractions
{
    public interface ICourseStore
    {
        IQueryable<Course> Query { get; }

        Task<Course> CreateAsync(Course course);
    }
}