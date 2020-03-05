using System;

namespace KnowledgeShare.Store.Core
{
    public struct Session
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Note { get; set; }
    }
}
