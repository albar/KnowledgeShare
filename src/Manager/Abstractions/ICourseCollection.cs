using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using KnowledgeShare.Entity;

namespace KnowledgeShare.Manager.Abstractions
{
    public interface ICourseCollection
    {
        Task<List<Course>> ToListAsync(CancellationToken token = default);
    }
}