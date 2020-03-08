using System;

namespace KnowledgeShare.Manager.Exceptions
{
    public class NotSupportedStoreException : NotSupportedException
    {
        public NotSupportedStoreException(string notSupportedTypeName) :
            base($"The store {notSupportedTypeName} is not supported to do this action")
        { }

        public NotSupportedStoreException(string notSupportedTypeName, string requiredTypeName) :
            base($"The store {notSupportedTypeName} is not supported to do this action. {requiredTypeName} is required to be implemented")
        { }
    }
}
