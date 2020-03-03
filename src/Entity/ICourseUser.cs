namespace KnowledgeShare.Entity
{
    public interface ICourseUser
    {
        string Id { get; }
        string Username { get; set; }
        string Email { get; set; }
        CourseUserRole Role { get; set; }
    }
}