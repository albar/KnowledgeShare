using System.Threading.Tasks;

namespace KnowledgeShare.Server.Hubs.Course
{
    public interface ICourseHub
    {
        Task CourseCreated(Store.Core.Course course);
        Task CourseUpdated(Store.Core.Course course);
    }
}
