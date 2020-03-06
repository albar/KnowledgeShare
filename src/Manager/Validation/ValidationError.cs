namespace KnowledgeShare.Manager.Validation
{
    public struct ValidationError
    {
        public ValidationError(string key, string description, int code)
        {
            Key = key;
            Description = description;
            Code = code;
        }

        public string Key { get; private set; }
        public string Description { get; private set; }
        public int Code { get; private set; }

        public static ValidationError Create(string key, string description)
        {
            return new ValidationError(key, description, 0);
        }

        public static ValidationError Create(string description)
        {
            return new ValidationError(null, description, 0);
        }
    }
}
