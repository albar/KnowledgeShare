namespace KnowledgeShare.Server.Notification
{
    public class KnowledgeShareNotification
    {
        public string Message { get; set; }
        public string MessageRaw { get; set; }
        public object[] Params { get; set; }

        public static KnowledgeShareNotification FromRaw(
            string message,
            object[] parameters)
        {
            return new KnowledgeShareNotification
            {
                Message = string.Format(message, parameters),
                MessageRaw = message,
                Params = parameters
            };
        }
    }
}
