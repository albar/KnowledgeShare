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

        public async Task<IPaginatedCollection<Course>> PaginateAsync(
            int page, int limit, CancellationToken token = default)
        {
            var items = await Task.Run(() =>
                _query.Skip(limit * (page - 1)).Take(limit).ToList(),
                token);

            return new PaginatedCourseCollection(items, page, limit);
        }
    }
}