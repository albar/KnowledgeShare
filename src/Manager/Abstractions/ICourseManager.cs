using System.Threading.Tasks;
using KnowledgeShare.Entity;

namespace KnowledgeShare.Manager.Abstractions
{
    public interface ICourseManager
    {
        Task<Course> CreateAsync(
            ICourseUser author,
            string title,
            ICourseUser speaker,
            string description,
            ILocation location,
            Visibility visibility,
            Session[] sessions);

        ICollection<Course> GetAllAccessibleToUser(ICourseUser accessor);
        Task<Course> FindAccessibleToUserById(ICourseUser accessor, string id);
    }
}