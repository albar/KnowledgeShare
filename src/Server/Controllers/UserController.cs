using System.Linq;
using System.Threading.Tasks;
using KnowledgeShare.Manager;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KnowledgeShare.Server.Controllers
{
    public class UserController
    {
        private readonly CourseUserManager _manager;

        public UserController(CourseUserManager manager)
        {
            _manager = manager;
        }

        [HttpGet("/api/user")]
        public async Task<IActionResult> ListUsers()
        {
            var users = await _manager.Users.Select(user => new
            {
                Id = user.Id,
                Email = user.Email
            }).ToListAsync();

            return new ObjectResult(users);
        }
    }
}
