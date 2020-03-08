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

        public string Key { get; }
        public string Description { get; }
        public int Code { get; }

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
