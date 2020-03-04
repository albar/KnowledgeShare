namespace KnowledgeShare.Entity
{
    public class Invitee
    {
        public ICourseUser User { get; set; }
        public Course Course { get; set; }
    }
}