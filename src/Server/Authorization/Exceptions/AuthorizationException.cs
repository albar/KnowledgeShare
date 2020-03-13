using System;
using Microsoft.AspNetCore.Authorization;

namespace KnowledgeShare.Server.Authorization
{
    public class AuthorizationException : Exception
    {
        public AuthorizationException(AuthorizationFailure failure)
        {
            Failure = failure;
        }

        public AuthorizationFailure Failure { get; }
    }
}
