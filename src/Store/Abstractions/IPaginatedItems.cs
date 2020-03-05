using System.Collections.Generic;

namespace KnowledgeShare.Store.Abstractions
{
    public interface IPaginatedItems<TItem>
    {
        int Page { get; }
        int Limit { get; }
        IEnumerable<TItem> Items { get; }
    }
}
