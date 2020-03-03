using System;

namespace KnowledgeShare.Entity
{
    public class Course
    {
        public string Id { get; } = new Guid().ToString();
        public ICourseUser Author { get; set; }
        public string Title { get; set; }
        public ICourseUser Speaker { get; set; }
        public string Description { get; set; }
        public Visibility Visibility { get; set; }
        public ILocation Location { get; set; }
        public Session[] Sessions { get; set; } = new Session[] { };
    }
}