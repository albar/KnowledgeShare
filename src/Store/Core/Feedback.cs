using System;

namespace KnowledgeShare.Store.Core
{
    public class Feedback
    {
        public string Id { get; } = Guid.NewGuid().ToString();

        public string CourseId { get; set; }
        public Course Course { get; set; }

        public string UserId { get; set; }
        public CourseUser User { get; set; }

        public FeedbackRate Rate { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
