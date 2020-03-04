using System.Linq;
using KnowledgeShare.Entity;
using KnowledgeShare.Manager.Abstractions;
using KnowledgeShare.Manager.Collection;

namespace KnowledgeShare.Manager
{
    public static class CourseQueryableExtensions
    {
        public static ICollection<Course> ToCollection(
            this IQueryable<Course> query)
        {
            return new CourseCollection(query);
        }
    }
}