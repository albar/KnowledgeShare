namespace KnowledgeShare.Manager.Validation
{
    public struct ValidationResult
    {
        public ValidationResult(bool succeeded, ValidationErrorsBag errorsBag)
        {
            Succeeded = succeeded;
            ErrorsBag = errorsBag;
        }

        public bool Succeeded { get; private set; }
        public ValidationErrorsBag ErrorsBag { get; private set; }

        public static ValidationResult Success { get; } = new ValidationResult
        {
            Succeeded = true,
            ErrorsBag = new ValidationErrorsBag(),
        };

        public static ValidationResult Failed { get; } = new ValidationResult
        {
            Succeeded = false,
            ErrorsBag = new ValidationErrorsBag(),
        };

        public static ValidationResult FromErrorsBag(ValidationErrorsBag errorsBag)
        {
            return new ValidationResult
            {
                Succeeded = errorsBag.Count > 0 ? false : true,
                ErrorsBag = errorsBag,
            };
        }
    }
}
