using System.Data.Common;
using KnowledgeShare.Store.Abstractions;
using KnowledgeShare.Store.Test;
using Microsoft.Data.Sqlite;

namespace KnowledgeShare.Store.EntityFrameworkCore.Test
{
    public class CourseStoreTest : CourseStoreSpecificationTestBase
    {
        private DbConnection _connection;

        protected DbConnection Connection
        {
            get
            {
                if (_connection == null)
                {
                    _connection = new SqliteConnection("DataSource=:memory:");
                    _connection.Open();
                }

                return _connection;
            }
        }

        protected override ICourseStore GetCourseStore()
        {
            var database = new DatabaseFactory(Connection).Create();
            return new CourseStore<CourseDbContext>(database);
        }
    }
}
