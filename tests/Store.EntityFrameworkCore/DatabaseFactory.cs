using System.Data.Common;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;

namespace KnowledgeShare.Store.EntityFrameworkCore.Test
{
    public class DatabaseFactory
    {
        private readonly DbConnection _connection;

        public DatabaseFactory(DbConnection connection)
        {
            _connection = connection;
        }

        public CourseDbContext Create()
        {
            var dbContextOptions = CreateOptions();
            var storeOption = new Mock<IOptions<OperationalStoreOptions>>();
            storeOption.SetupGet(opt => opt.Value).Returns(new OperationalStoreOptions());
            var context = new CourseDbContext(dbContextOptions, storeOption.Object);
            context.Database.EnsureCreated();
            return context;
        }

        private DbContextOptions CreateOptions()
        {
            return new DbContextOptionsBuilder()
                .UseSqlite(_connection)
                .Options;
        }
    }
}
