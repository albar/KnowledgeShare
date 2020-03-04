using System.Collections.Generic;

namespace KnowledgeShare.Manager.Abstractions
{
    public interface IPaginatedCollection<TEntity>
    {
        int Page { get; }
        int Limit { get; }
        IEnumerable<TEntity> Items { get; }
    }
}