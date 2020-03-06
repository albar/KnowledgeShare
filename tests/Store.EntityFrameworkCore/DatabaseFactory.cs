using System;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;

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
            var options = CreateOptions();
            var context = new CourseDbContext(options);
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
