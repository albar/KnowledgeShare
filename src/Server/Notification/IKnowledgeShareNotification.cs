using System.Threading.Tasks;

namespace KnowledgeShare.Server.Notification
{
    public interface IKnowledgeShareNotification
    {
        Task Notification(KnowledgeShareNotification notification);
    }
}
