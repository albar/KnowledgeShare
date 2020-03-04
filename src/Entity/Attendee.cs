namespace KnowledgeShare.Entity
{
    public class Attendee
    {
        public ICourseUser User { get; set; }
        public Course Course { get; set; }
    }
}