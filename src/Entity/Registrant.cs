using System;

namespace KnowledgeShare.Entity
{
    public class Registrant
    {
        public string Id { get; } = Guid.NewGuid().ToString();
        public ICourseUser User { get; set; }
        public Course Course { get; set; }
    }
}
