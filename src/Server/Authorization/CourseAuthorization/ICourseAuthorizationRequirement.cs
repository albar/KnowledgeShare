using System.Threading.Tasks;
using KnowledgeShare.Store.Core;
using Microsoft.AspNetCore.Authorization;

namespace KnowledgeShare.Server.Authorization.CourseAuthorization
{
    public interface ICourseAuthorizationRequirement : IAuthorizationRequirement
    {
        Task<bool> CheckAsync(Course course);
    }
}
