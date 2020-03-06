using System.Data.Common;
using KnowledgeShare.Store.Abstractions;
using KnowledgeShare.Store.Test;
using Microsoft.Data.Sqlite;

namespace KnowledgeShare.Store.EntityFrameworkCore.Test
{
    public class CourseFeedbackStoreTest : CourseFeedbackStoreSpecificationTest
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

        protected override ICourseFeedbackStore GetCourseFeedbackStore()
        {
            var database = new DatabaseFactory(Connection).Create();
            return new CourseStore(database);
        }
    }
}
