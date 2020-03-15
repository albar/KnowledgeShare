using System;

namespace KnowledgeShare.Store.Core
{
    public class Registrant
    {
        public string Id { get; } = Guid.NewGuid().ToString();

        public string UserId { get; set; }
        public CourseUser User { get; set; }

        public string CourseId { get; set; }
        public Course Course { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
