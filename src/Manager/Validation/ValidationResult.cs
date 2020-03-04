namespace KnowledgeShare.Manager.Validation
{
    public struct ValidationResult
    {
        public bool Succeeded { get; private set; }
        public ValidationErrorsBag ErrorsBag { get; private set; }

        public static ValidationResult Success { get; } = new ValidationResult { Succeeded = true };

        public static ValidationResult Failed(ValidationErrorsBag errorsBag)
        {
            return new ValidationResult
            {
                Succeeded = false,
                ErrorsBag = errorsBag,
            };
        }
    }
}
