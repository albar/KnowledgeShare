using System;

namespace KnowledgeShare.Manager.Exceptions
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