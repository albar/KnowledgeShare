using System.Data.Common;
using KnowledgeShare.Store.Abstractions;
using KnowledgeShare.Store.Test;
using Microsoft.Data.Sqlite;
using Moq;

namespace KnowledgeShare.Store.EntityFrameworkCore.Test
{
    public class CourseRegistrantStoreTest : CourseRegistrantStoreSpecificationTestBase
    {
        protected override ICourseRegistrantStore GetRegistrantCourseStore()
        {
            var fakeDatabase = new Mock<CourseDbContext>();
            return new CourseStore(fakeDatabase.Object);
        }
    }
}
