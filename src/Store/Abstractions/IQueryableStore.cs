using System.Linq;

namespace KnowledgeShare.Store.Abstractions
{
    public interface IQueryableStore<TItem>
    {
        IQueryable<TItem> Items { get; }
    }
}
