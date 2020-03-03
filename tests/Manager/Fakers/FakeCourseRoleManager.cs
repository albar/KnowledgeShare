using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KnowledgeShare.Entity;
using KnowledgeShare.Manager.Abstractions;

namespace KnowledgeShare.Manager.Test.Fakers
{
    public class FakeCourseRoleManager : ICourseRoleManager
    {
        private readonly List<ICourseRole> _roles = new List<ICourseRole>();
        public async Task<ICourseRole> CreateAsync(string name)
        {
            var role = new CourseRole
            {
                Name = name,
            };

            _roles.Add(role);
            return role;
        }

        private class CourseRole : ICourseRole
        {
            public string Id { get; } = new Guid().ToString();
            public string Name { get; set; }
        }
    }
}