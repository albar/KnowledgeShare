using System;
using System.Collections.Generic;

namespace KnowledgeShare.Entity
{
    public class Course
    {
        public string Id { get; } = Guid.NewGuid().ToString();
        public virtual ICourseUser Author { get; set; }
        public string Title { get; set; }
        public virtual ICourseUser Speaker { get; set; }
        public string Description { get; set; }
        public Visibility Visibility { get; set; }
        public virtual ILocation Location { get; set; }
        public virtual List<Session> Sessions { get; set; } = new List<Session>();

        public virtual List<Invitee> Invitee { get; } = new List<Invitee>();
        public virtual List<Attendee> Attendee { get; } = new List<Attendee>();
    }
}
