namespace KnowledgeShare.Store.Core
{
    public interface ICourseUser
    {
        string Id { get; }
        string Username { get; set; }
        string Email { get; set; }
        CourseUserRole Role { get; set; }
    }
}
