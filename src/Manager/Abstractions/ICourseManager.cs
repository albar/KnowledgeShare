using System.Collections.Generic;
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
            List<Session> sessions);

        ICollection<Course> GetAllAccessibleBy(ICourseUser accessor);

        Task<Course> FindAccessibleByAsync(ICourseUser accessor, string courseId);

        Task<Course> UpdateAccessibleByAsync(ICourseUser acessor, string courseId, UpdatableCourse updatableCourse);
    }
}
