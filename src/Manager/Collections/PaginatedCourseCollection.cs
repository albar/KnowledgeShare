using System.Collections.Generic;
using KnowledgeShare.Entity;
using KnowledgeShare.Manager.Abstractions;

namespace KnowledgeShare.Manager.Collection
{
    public class PaginatedCourseCollection : IPaginatedCollection<Course>
    {
        public PaginatedCourseCollection(List<Course> items, int page, int limit)
        {
            Items = items;
            Page = page;
            Limit = limit;
        }

        public int Page { get; }
        public int Limit { get; }
        public IEnumerable<Course> Items { get; }
    }
}