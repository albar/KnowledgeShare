using System;

namespace KnowledgeShare.Store.Core
{
    public class CourseUser
    {
        public string Id { get; } = Guid.NewGuid().ToString();
        public string Username { get; set; }
        public string Email { get; set; }
        public CourseUserRole Role { get; set; }
    }
}
