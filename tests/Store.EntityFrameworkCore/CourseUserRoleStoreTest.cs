using KnowledgeShare.Store.Abstractions;
using KnowledgeShare.Store.Test;
using Moq;

namespace KnowledgeShare.Store.EntityFrameworkCore.Test
{
    public class CourseUserRoleStoreTest : CourseUserRoleStoreSpecificationTestBase
    {
        protected override ICourseUserRoleStore GetCourseUserRoleStore()
        {
            var fakeDatabase = new Mock<CourseDbContext>();
            return new CourseUserStore<CourseDbContext>(fakeDatabase.Object);
        }
    }
}
