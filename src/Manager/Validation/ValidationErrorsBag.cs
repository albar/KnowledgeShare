using System;
using System.Collections.Generic;

namespace KnowledgeShare.Manager.Validation
{
    public class ValidationErrorsBag
    {
        private List<string> ErrorsMessage { get; } = new List<string>();
        public int Count => ErrorsMessage.Count;

        public void Append(ValidationErrorsBag errorsBag)
        {
            ErrorsMessage.AddRange(errorsBag.ErrorsMessage);
        }

        public void Add(string errorMessage)
        {
            ErrorsMessage.Add(errorMessage);
        }
    }
}
