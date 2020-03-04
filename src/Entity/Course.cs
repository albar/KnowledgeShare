using System;
using System.Collections.Generic;

namespace KnowledgeShare.Entity
{
    public class Course
    {
        public string Id { get; } = Guid.NewGuid().ToString();
        public ICourseUser Author { get; set; }
        public string Title { get; set; }
        public ICourseUser Speaker { get; set; }
        public string Description { get; set; }
        public Visibility Visibility { get; set; }
        public ILocation Location { get; set; }
        public Session[] Sessions { get; set; } = new Session[] { };

        public List<Invitee> Invitee { get; } = new List<Invitee>();
        public List<Attendee> Attendee { get; } = new List<Attendee>();
    }
}