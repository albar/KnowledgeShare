using System.Threading.Tasks;
using KnowledgeShare.Entity;

namespace KnowledgeShare.Manager.Abstractions
{
    public interface ICourseUserManager
    {
        Task<ICourseUser> CreateAsync(
            string username,
            string email,
            CourseUserRole role);

        Task<bool> IsExistedAsync(string id);
    }
}