using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KnowledgeShare.Store.Core;
using KnowledgeShare.Store.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace KnowledgeShare.Store.EntityFrameworkCore
{
    public class CourseStore : ICourseStore
    {
        private readonly CourseDbContext _database;

        public CourseStore(CourseDbContext database)
        {
            _database = database;
        }

        protected DbSet<Course> Courses => _database.Set<Course>();

        public async Task CreateAsync(Course course, CancellationToken token = default)
        {
            _database.Add(course);
            await SaveChangeAsync(token);
        }

        public ValueTask<Course> FindByIdAsync(string courseId, CancellationToken token = default)
        {
            return new ValueTask<Course>(Courses.Where(course => course.Id == courseId).SingleOrDefaultAsync(token));
        }

        public async Task RemoveAsync(Course course, CancellationToken token = default)
        {
            _database.Remove(course);
            await SaveChangeAsync(token);
        }

        public async Task UpdateAsync(Course course, CancellationToken token = default)
        {
            _database.Update(course);
            await SaveChangeAsync(token);
        }

        private async Task SaveChangeAsync(CancellationToken token = default)
        {
            await _database.SaveChangesAsync(token);
        }

        #region Dispose
        public void Dispose()
        {
        }
        #endregion
    }
}
