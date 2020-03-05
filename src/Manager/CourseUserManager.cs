using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using KnowledgeShare.Store.Core;
using KnowledgeShare.Store.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace KnowledgeShare.Manager
{
    public class CourseUserManager<TUser> : UserManager<TUser> where TUser : CourseUser
    {
        public CourseUserManager(
            IUserStore<TUser> store,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<TUser> passwordHasher,
            IEnumerable<IUserValidator<TUser>> userValidators,
            IEnumerable<IPasswordValidator<TUser>> passwordValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            IServiceProvider services,
            ILogger<UserManager<TUser>> logger) :
            base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
        }

        public async Task SetCourseUserRoleAsync(
            TUser user,
            CourseUserRole role,
            CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var store = GetCourseUserRoleStore();
            await store.SetCourseUserRoleAsync(user, role, token);
            await UpdateUserAsync(user);
        }

        private ICourseUserRoleStore GetCourseUserRoleStore()
        {
            if (Store is ICourseUserRoleStore store)
            {
                return store;
            }

            throw new NotSupportedException(
                $"Store is not suppported to do this action. {nameof(ICourseUserRoleStore)} is not implemented");
        }
    }
}
