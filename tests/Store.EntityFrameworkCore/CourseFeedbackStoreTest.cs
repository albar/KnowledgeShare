using System.Data.Common;
using KnowledgeShare.Store.Abstractions;
using KnowledgeShare.Store.Test;
using Microsoft.Data.Sqlite;
using Moq;

namespace KnowledgeShare.Store.EntityFrameworkCore.Test
{
    public class CourseFeedbackStoreTest : CourseFeedbackStoreSpecificationTest
    {
        protected override ICourseFeedbackStore GetCourseFeedbackStore()
        {
            var fakeDatabase = new Mock<CourseDbContext>();
            return new CourseStore<CourseDbContext>(fakeDatabase.Object);
        }
    }
}
