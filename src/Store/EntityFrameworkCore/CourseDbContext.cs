using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.EntityFramework.Extensions;
using IdentityServer4.EntityFramework.Interfaces;
using IdentityServer4.EntityFramework.Options;
using KnowledgeShare.Store.Core;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace KnowledgeShare.Store.EntityFrameworkCore
{
    public class CourseDbContext : IdentityUserContext<CourseUser>, IPersistedGrantDbContext
    {
        private readonly IOptions<OperationalStoreOptions> _operationalStoreOptions;

        public CourseDbContext() { }

        public CourseDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) :
            base(options)
        {
            _operationalStoreOptions = operationalStoreOptions;
        }

        public DbSet<PersistedGrant> PersistedGrants { get; set; }
        public DbSet<DeviceFlowCodes> DeviceFlowCodes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ConfigurePersistedGrantContext(_operationalStoreOptions.Value);
            CounfigureCourseContext(builder);
        }

        private void CounfigureCourseContext(ModelBuilder builder)
        {
            builder.Entity<Course>(entity =>
            {
                entity.HasKey(course => course.Id);

                entity.HasOne(course => course.Author);
                entity.HasOne(course => course.Speaker);
                entity.HasMany(course => course.Registrants);
                entity.HasMany(course => course.Feedbacks);

                entity.Property(course => course.Visibility).HasConversion(
                    value => value.ToString(),
                    value => (CourseVisibility)Enum.Parse(
                        typeof(CourseVisibility), value));

                entity.Ignore(course => course.Location);
                entity.Property<string>("_location");
                entity.Property<string>("_locationType");

                entity.Property(course => course.Sessions).HasConversion(
                    value => JsonSerializer.Serialize(value, null),
                    value => JsonSerializer.Deserialize<List<Session>>(value, null));
            });

            builder.Entity<Registrant>(entity =>
            {
                entity.HasKey(registrant => registrant.Id);
                entity.HasAlternateKey(registrant => new
                {
                    registrant.CourseId,
                    registrant.UserId,
                });
            });

            builder.Entity<Feedback>(entity =>
            {
                entity.HasKey(feedback => feedback.Id);
                entity.HasAlternateKey(feedback => new
                {
                    feedback.CourseId,
                    feedback.UserId,
                });
            });
        }

        public Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }
    }
}
