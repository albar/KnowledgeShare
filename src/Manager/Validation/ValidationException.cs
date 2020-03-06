using System;
using System.Collections.Generic;
using System.Linq;

namespace KnowledgeShare.Manager.Validation
{
    public class ValidationException : Exception
    {
        public ValidationException(IEnumerable<ValidationError> errors)
        {
            Errors = errors.ToArray();
        }

        public ValidationError[] Errors { get; }
    }
}
