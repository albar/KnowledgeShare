using System;
using System.Collections.Generic;

namespace KnowledgeShare.Store.Core
{
    public class Course
    {
        public string Id { get; } = Guid.NewGuid().ToString();
        public virtual CourseUser Author { get; set; }
        public string Title { get; set; }
        public virtual CourseUser Speaker { get; set; }
        public string Description { get; set; }
        public CourseVisibility Visibility { get; set; }
        public virtual ILocation Location { get; set; }
        public virtual List<Session> Sessions { get; set; } = new List<Session>();

        public virtual List<Registrant> Registrants { get; } = new List<Registrant>();
        public virtual List<Feedback> Feedbacks { get; } = new List<Feedback>();
    }
}
