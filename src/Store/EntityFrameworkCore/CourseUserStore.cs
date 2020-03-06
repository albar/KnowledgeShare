using System;
using System.Threading;
using System.Threading.Tasks;
using KnowledgeShare.Store.Abstractions;
using KnowledgeShare.Store.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace KnowledgeShare.Store.EntityFrameworkCore
{
    public class CourseUserStore :
        UserOnlyStore<CourseUser, CourseDbContext>,
        ICourseUserRoleStore
    {
        public CourseUserStore(
            CourseDbContext context,
            IdentityErrorDescriber describer = null)
            : base(context, describer)
        {
        }

        public Task SetCourseUserRoleAsync(
            CourseUser user,
            CourseUserRole role,
            CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            user.Role = role;
            return Task.CompletedTask;
        }
    }
}
