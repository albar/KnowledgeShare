using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using KnowledgeShare.Manager;
using KnowledgeShare.Manager.Validation;
using KnowledgeShare.Store.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace KnowledgeShare.Server.Controllers
{
    public class CourseController
    {
        private readonly CourseUserManager _userManager;
        private readonly CourseManager _manager;
        private readonly IHttpContextAccessor _accessor;
        private readonly ILogger<CourseController> _logger;

        public CourseController(
            CourseUserManager userManager,
            CourseManager manager,
            IHttpContextAccessor accessor,
            ILogger<CourseController> logger)
        {
            _userManager = userManager;
            _manager = manager;
            _accessor = accessor;
            _logger = logger;
        }

        [HttpGet("/api/course")]
        [Produces("application/json")]
        public async Task<IActionResult> ListCourses()
        {
            var userId = _accessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await _userManager.FindByIdAsync(userId);
            var courses = await _manager.GetCoursesVisibleTo(user)
                .Include(course => course.Author)
                .Include(course => course.Speaker)
                .OrderByDescending(course => course.UpdatedAt)
                .ToListAsync();

            return new ObjectResult(courses);
        }

        [HttpGet("/api/course/visibility")]
        [Produces("application/json")]
        public IActionResult ListCourseVisibilities()
        {
            var visibilities = Enum.GetValues(typeof(CourseVisibility))
                .Cast<CourseVisibility>()
                .Select(visibility => new
                {
                    Key = (int)visibility,
                    Value = visibility.ToString()
                })
                .ToArray();

            return new ObjectResult(visibilities);
        }

        [HttpPost("/api/course")]
        [Produces("application/json")]
        public async Task<IActionResult> CreateAsync([FromBody] CreateCourseModel model)
        {
            var authorId = _accessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var author = await _userManager.FindByIdAsync(authorId);
            var speaker = await _userManager.FindByIdAsync(model.Speaker);
            var course = new Course
            {
                Author = author,
                Title = model.Title,
                Description = model.Description,
                Speaker = speaker,
                Visibility = model.Visibility,
                Sessions = model.Sessions
            };

            await _manager.CreateAsync(course);

            return new CreatedResult("/api/course", course);
        }

        [HttpGet("/api/course/{id}")]
        [Produces("application/json")]
        public async Task<IActionResult> GetCourse(string id)
        {
            var userId = _accessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await _userManager.FindByIdAsync(userId);
            var courses = await _manager.GetCoursesVisibleTo(user)
                .Where(course => course.Id == id)
                .FirstAsync();

            return new ObjectResult(courses);
        }

        public class CreateCourseModel
        {
            public string Title { get; set; }
            public string Description { get; set; }
            public string Speaker { get; set; }
            public CourseVisibility Visibility { get; set; }
            public List<Session> Sessions { get; set; } = new List<Session>();
        }
    }
}
