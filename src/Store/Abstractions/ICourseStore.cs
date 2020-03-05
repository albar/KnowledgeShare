using System;
using System.Threading;
using System.Threading.Tasks;
using KnowledgeShare.Entity;

namespace KnowledgeShare.Store.Abstractions
{
    public interface ICourseStore : IDisposable
    {
        Task CreateAsync(Course course, CancellationToken token = default);
        ValueTask<Course> FindByIdAsync(string courseId, CancellationToken cancellationToken);
    }
}
