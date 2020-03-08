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

        public bool Succeeded { get; }
        public IEnumerable<ValidationError> Errors { get; }

        public static ValidationResult Success { get; } = new ValidationResult(true, new ValidationError[] { });

        public static ValidationResult Failed { get; } = new ValidationResult(false, new ValidationError[] { });

        public static ValidationResult FromErrorsBag(IEnumerable<ValidationError> errors)
        {
            return new ValidationResult(errors.Count() > 0 ? false : true, errors);
        }
    }
}
