using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace KnowledgeShare.Store.Abstractions
{
    public interface IItemCollection<TItem>
    {
        ValueTask<List<TItem>> ToListAsync(CancellationToken token = default);
        ValueTask<IPaginatedItems<TItem>> PaginateAsync(
            int page,
            int limit,
            CancellationToken token = default);
    }
}
