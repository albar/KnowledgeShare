using System.Collections.Generic;

namespace KnowledgeShare.Manager.Validation
{
    public class ValidationErrorsBag
    {
        public List<string> ErrorMessages { get; } = new List<string>();
        public int Count => ErrorMessages.Count;

        public void Append(ValidationErrorsBag errorsBag)
        {
            ErrorMessages.AddRange(errorsBag.ErrorMessages);
        }

        public void Add(string errorMessage)
        {
            ErrorMessages.Add(errorMessage);
        }
    }
}
