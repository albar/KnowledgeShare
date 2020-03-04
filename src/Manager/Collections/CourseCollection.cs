using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KnowledgeShare.Entity;
using KnowledgeShare.Manager.Abstractions;

namespace KnowledgeShare.Manager.Collection
{
    public class CourseCollection : ICourseCollection
    {
        public CourseCollection(IQueryable<Course> query)
        {
            Query = query;
        }

        public IQueryable<Course> Query { get; }

        public async Task<List<Course>> ToListAsync(CancellationToken token = default)
        {
            return await Task.Run(Query.ToList, token);
        }
    }
}