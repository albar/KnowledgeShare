using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using KnowledgeShare.Entity;

namespace KnowledgeShare.Manager.Abstractions
{
    public interface ICollection<TEntity>
    {
        Task<List<TEntity>> ToListAsync(CancellationToken token = default);
    }
}