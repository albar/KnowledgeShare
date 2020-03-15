using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;

namespace KnowledgeShare.Server.Notification
{
    public class CourseUserIdProvider : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {
            return connection.User.FindFirstValue("sub");
        }
    }
}
