using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KnowledgeShare.Entity;
using KnowledgeShare.Manager.Abstractions;

namespace KnowledgeShare.Manager.Collection
{
    public class CourseCollection : Abstractions.ICollection<Course>
    {
        private readonly IQueryable<Course> _query;

        public CourseCollection(IQueryable<Course> query)
        {
            _query = query;
        }

        public async Task<List<Course>> ToListAsync(CancellationToken token = default)
        {
            return await Task.Run(_query.ToList, token);
        }
    }
}