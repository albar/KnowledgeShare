using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KnowledgeShare.Store.Core;
using KnowledgeShare.Store.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace KnowledgeShare.Store.EntityFrameworkCore
{
    public class CourseStore : ICourseStore, ICourseRegistrantStore
    {
        private readonly CourseDbContext _database;

        public CourseStore(CourseDbContext database)
        {
            _database = database;
        }

        protected DbSet<Course> Courses => _database.Set<Course>();
        protected DbSet<Registrant> Registrants => _database.Set<Registrant>();

        public async Task CreateAsync(Course course, CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();

            _database.Add(course);
            await SaveChangeAsync(token);
        }

        public ValueTask<Course> FindByIdAsync(string courseId, CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();

            return new ValueTask<Course>(
                Courses.Where(course => course.Id == courseId)
                    .SingleOrDefaultAsync(token));
        }

        public async Task UpdateAsync(Course course, CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();

            _database.Update(course);
            await SaveChangeAsync(token);
        }

        public Task RegisterUserToAsync(Course course, CourseUser user, CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();

            if (course == null)
            {
                throw new ArgumentNullException(nameof(course));
            }
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            course.Registrants.Add(CreateRegistrant(course, user));
            return Task.CompletedTask;
        }

        public IQueryable<Registrant> GetRegistrants(Course course)
        {
            return Registrants.Where(registrant => registrant.Course.Id == course.Id);
        }

        public async Task RemoveAsync(Course course, CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();

            _database.Remove(course);
            await SaveChangeAsync(token);
        }

        private async Task SaveChangeAsync(CancellationToken token = default)
        {
            await _database.SaveChangesAsync(token);
        }

        private Registrant CreateRegistrant(Course course, CourseUser user)
        {
            return new Registrant
            {
                Course = course,
                User = user,
            };
        }

        #region Dispose
        public void Dispose()
        {
        }
        #endregion
    }
}
