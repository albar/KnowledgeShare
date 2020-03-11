using System.Threading.Tasks;
using KnowledgeShare.Manager;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KnowledgeShare.Server.Controllers
{
    public class CourseController
    {
        private readonly CourseManager _manager;

        public CourseController(CourseManager manager)
        {
            _manager = manager;
        }

        [HttpGet("api/course")]
        public async Task<IActionResult> ListCourses()
        {
            var courses = await _manager.Courses.ToListAsync();
            return new ObjectResult(courses);
        }
    }
}
