using System;

namespace KnowledgeShare.Store.Core
{
    public class Registrant
    {
        public string Id { get; } = Guid.NewGuid().ToString();
        public CourseUser User { get; set; }
        public Course Course { get; set; }
    }
}
