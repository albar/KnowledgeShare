using System;

namespace KnowledgeShare.Entity
{
    public struct Session
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Note { get; set; }
    }
}