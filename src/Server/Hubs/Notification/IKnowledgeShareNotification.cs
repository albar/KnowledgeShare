using System.Threading.Tasks;

namespace KnowledgeShare.Server.Hubs.Notification
{
    public interface IKnowledgeShareNotification
    {
        Task Notification(KnowledgeShareNotification notification);
    }
}
