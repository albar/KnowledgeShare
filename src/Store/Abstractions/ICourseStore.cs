using System.Threading.Tasks;
using KnowledgeShare.Entity;

namespace KnowledgeShare.Store.Abstractions
{
    public interface ICourseStore
    {
        Task<Course> CreateAsync(Course course);
    }
}