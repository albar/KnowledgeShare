using System;

namespace KnowledgeShare.Entity
{
    public class Feedback
    {
        public string Id { get; } = Guid.NewGuid().ToString();
        public Course Course { get; set; }
        public ICourseUser User { get; set; }
        public FeedbackRate Rate { get; set; }
        public string Note { get; set; }
    }
}
