using System.Threading.Tasks;
using KnowledgeShare.Entity;

namespace KnowledgeShare.Manager.Abstractions
{
    public interface ICourseRoleManager
    {
        Task<ICourseRole> CreateAsync(string name);
    }
}