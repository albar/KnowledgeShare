using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using KnowledgeShare.Manager;
using KnowledgeShare.Server.Authorization;
using KnowledgeShare.Server.Authorization.CourseAuthorization;
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
        private readonly IAuthorizationService _authorization;
        private readonly ILogger<CourseController> _logger;

        public CourseController(
            CourseUserManager userManager,
            CourseManager manager,
            IHttpContextAccessor accessor,
            IAuthorizationService authorization,
            ILogger<CourseController> logger)
        {
            _userManager = userManager;
            _manager = manager;
            _accessor = accessor;
            _authorization = authorization;
            _logger = logger;
        }

        [HttpGet("/api/course")]
        [Produces("application/json")]
        public async Task<IActionResult> ListCoursesAsync()
        {
            var userId = _accessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await _userManager.FindByIdAsync(userId);
            var courses = await _manager.GetCoursesVisibleTo(user)
                .Include(course => course.Author)
                .Include(course => course.Speaker)
                .OrderByDescending(course => course.CreatedAt)
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
        public async Task<IActionResult> CreateAsync([FromBody] CourseModel model)
        {
            var authorId = _accessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var author = await _userManager.FindByIdAsync(authorId);

            _logger.LogInformation(author.Role.ToString());

            var result = await _authorization.AuthorizeAsync(
                _accessor.HttpContext.User,
                new Course(),
                new CreateCourseRequirement(author));

            if (!result.Succeeded)
            {
                throw new AuthorizationException(result.Failure);
            }

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
        public async Task<IActionResult> GetCourseAsync(string id)
        {
            var userId = _accessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await _userManager.FindByIdAsync(userId);

            var course = await _manager.Courses
                .Include(course => course.Author)
                .Include(course => course.Speaker)
                .Include(course => course.Registrants)
                .Include(course => course.Feedbacks)
                .FirstAsync(course => course.Id == id);

            var result = await _authorization.AuthorizeAsync(
                _accessor.HttpContext.User,
                course,
                new ViewCourseRequirement(user));

            if (!result.Succeeded)
            {
                throw new AuthorizationException(result.Failure);
            }

            return new ObjectResult(course);
        }

        [HttpPatch("/api/course/{id}")]
        [Produces("application/json")]
        public async Task<IActionResult> UpdateCourseAsync(string id, [FromBody] CourseModel model)
        {
            var userId = _accessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await _userManager.FindByIdAsync(userId);

            var course = await _manager.Courses
                .Include(course => course.Author)
                .Include(course => course.Speaker)
                .FirstAsync(course => course.Id == id);

            var result = await _authorization.AuthorizeAsync(
                _accessor.HttpContext.User,
                course,
                new EditCourseRequirement(user));

            if (!result.Succeeded)
            {
                throw new AuthorizationException(result.Failure);
            }

            await model.PutIntoAsync(course, _userManager);
            await _manager.UpdateAsync(course);

            return new ObjectResult(course);
        }

        [HttpPost("/api/course/{id}/register")]
        [Produces("application/json")]
        public async Task<IActionResult> RegisterAsync(string id)
        {
            var userId = _accessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await _userManager.FindByIdAsync(userId);

            var course = await _manager.Courses
                .Include(course => course.Author)
                .Include(course => course.Speaker)
                .FirstAsync(course => course.Id == id);

            var result = await _authorization.AuthorizeAsync(
                _accessor.HttpContext.User,
                course,
                new RegisterCourseRequirement());

            if (!result.Succeeded)
            {
                throw new AuthorizationException(result.Failure);
            }

            await _manager.RegisterUserToAsync(course, user);
            return new OkResult();
        }

        public class CourseModel
        {
            public string Title { get; set; }
            public string Description { get; set; }
            public string Speaker { get; set; }
            public CourseVisibility Visibility { get; set; }
            public List<Session> Sessions { get; set; }

            public async Task PutIntoAsync(Course course, CourseUserManager manager = null)
            {
                if (!string.IsNullOrWhiteSpace(Title))
                {
                    course.Title = Title;
                }
                if (!string.IsNullOrWhiteSpace(Description))
                {
                    course.Description = Description;
                }
                if (!string.IsNullOrWhiteSpace(Speaker) &&
                    course.Speaker.Id != Speaker &&
                    manager != null)
                {
                    var speaker = await manager.FindByIdAsync(Speaker);
                    course.Speaker = speaker;
                }
                course.Visibility = Visibility;
                if (Sessions != null)
                {
                    course.Sessions = Sessions;
                }
            }
        }
    }
}
