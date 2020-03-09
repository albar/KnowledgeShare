using IdentityServer4.EntityFramework.Options;
using KnowledgeShare.Store.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace KnowledgeShare.Server
{
    public class CourseContext : CourseDbContext
    {
        public CourseContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) :
            base(options, operationalStoreOptions)
        {
        }
    }
}
