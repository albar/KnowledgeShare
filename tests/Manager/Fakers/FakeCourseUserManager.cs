using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KnowledgeShare.Entity;
using KnowledgeShare.Manager.Abstractions;

namespace KnowledgeShare.Manager.Test.Fakers
{
    public class FakeCourseUserManager : ICourseUserManager
    {
        private readonly List<ICourseUser> _users = new List<ICourseUser>();

        public async Task<ICourseUser> CreateAsync(string username, string email, CourseUserRole role)
        {
            var user = new CourseUser
            {
                Username = username,
                Email = email,
                Role = role,
            };

            _users.Add(user);
            return user;
        }

        public Task<bool> IsExistedAsync(string id)
        {
            return Task.FromResult(_users.Any(u => u.Id == id));
        }

        private class CourseUser : ICourseUser
        {
            public string Id { get; } = new Guid().ToString();
            public string Username { get; set; }
            public string Email { get; set; }
            public CourseUserRole Role { get; set; }
        }
    }
}