using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using KnowledgeShare.Store.Core;
using KnowledgeShare.Store.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using KnowledgeShare.Manager.Exceptions;

namespace KnowledgeShare.Manager
{
    public class CourseUserManager : UserManager<CourseUser>
    {
        public CourseUserManager(
            IUserStore<CourseUser> store,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<CourseUser> passwordHasher,
            IEnumerable<IUserValidator<CourseUser>> userValidators,
            IEnumerable<IPasswordValidator<CourseUser>> passwordValidators,
            ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors,
            IServiceProvider services, ILogger<UserManager<CourseUser>> logger) :
            base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
        }

        public async Task SetCourseUserRoleAsync(
            CourseUser user,
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

            throw new NotSupportedStoreException(Store.GetType().Name, nameof(ICourseUserRoleStore));
        }
    }
}
