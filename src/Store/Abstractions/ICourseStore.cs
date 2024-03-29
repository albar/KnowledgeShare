using System;
using System.Threading;
using System.Threading.Tasks;
using KnowledgeShare.Store.Core;

namespace KnowledgeShare.Store.Abstractions
{
    public interface ICourseStore : IDisposable
    {
        Task CreateAsync(Course course, CancellationToken token = default);
        ValueTask<Course> FindByIdAsync(string courseId, CancellationToken token = default);
        Task UpdateAsync(Course course, CancellationToken token = default);
        Task RemoveAsync(Course course, CancellationToken token = default);
    }
}
