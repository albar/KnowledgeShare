using System.Collections.Generic;
using System.Linq;

namespace KnowledgeShare.Manager.Validation
{
    public struct ValidationResult
    {
        public ValidationResult(bool succeeded, IEnumerable<ValidationError> errors)
        {
            Succeeded = succeeded;
            Errors = errors;
        }

        public bool Succeeded { get; private set; }
        public IEnumerable<ValidationError> Errors { get; private set; }

        public static ValidationResult Success { get; } = new ValidationResult
        {
            Succeeded = true,
            Errors = new ValidationError[] { },
        };

        public static ValidationResult Failed { get; } = new ValidationResult
        {
            Succeeded = false,
            Errors = new ValidationError[] { },
        };

        public static ValidationResult FromErrorsBag(IEnumerable<ValidationError> errors)
        {
            return new ValidationResult
            {
                Succeeded = errors.Count() > 0 ? false : true,
                Errors = errors,
            };
        }
    }
}
