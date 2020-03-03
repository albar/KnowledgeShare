using System.Threading.Tasks;
using KnowledgeShare.Entity;
using KnowledgeShare.Manager.Abstractions;
using KnowledgeShare.Store.Abstractions;

namespace KnowledgeShare.Manager
{
    public class CourseManager : ICourseManager
    {
        private readonly ICourseUserManager _userManager;
        private readonly ICourseStore _courseStore;

        public CourseManager(ICourseUserManager userManager, ICourseStore courseStore)
        {
            _userManager = userManager;
            _courseStore = courseStore;
        }

        public async Task<Course> CreateAsync(
            ICourseUser author,
            string title,
            ICourseUser speaker,
            string description,
            ILocation location,
            Visibility visibility,
            Session[] sessions)
        {
            var course = new Course
            {
                Author = author,
                Title = title,
                Speaker = speaker,
                Description = description,
                Location = location,
                Visibility = visibility,
                Sessions = sessions,
            };

            return await _courseStore.CreateAsync(course);
        }
    }
}