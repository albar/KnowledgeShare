using System;

namespace KnowledgeShare.Manager.Validation
{
    public class ValidationException : Exception
    {
        public ValidationException(ValidationErrorsBag errorsBag)
        {
            ErrorsBag = errorsBag;
        }

        public ValidationErrorsBag ErrorsBag { get; }
    }
}
