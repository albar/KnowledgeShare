using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;

namespace KnowledgeShare.Server.Hubs.Notification
{
    public class CourseUserIdProvider : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {
            return connection.User.FindFirstValue("sub");
        }
    }
}
