using System;
using System.Collections.Generic;
using KnowledgeShare.Store.Core;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace KnowledgeShare.Store.EntityFrameworkCore
{
    public class CourseDbContext : IdentityUserContext<CourseUser>
    {
        public CourseDbContext() { }

        public CourseDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Course>(entity =>
            {
                entity.HasKey(course => course.Id);
                entity.HasOne(course => course.Author);
                entity.HasOne(course => course.Speaker);
                entity.HasMany(course => course.Registrants);
                entity.HasMany(course => course.Feedbacks);

                entity.Property(course => course.Visibility)
                    .HasConversion(
                        value => value.ToString(),
                        value => (CourseVisibility)Enum.Parse(
                            typeof(CourseVisibility), value));

                entity.Ignore(course => course.Location);
                entity.Property<string>("_location");
                entity.Property<string>("_locationType");

                entity.Property(course => course.Sessions)
                    .HasConversion(
                        value => JsonConvert.SerializeObject(value),
                        value => JsonConvert.DeserializeObject<List<Session>>(value));
            });

            builder.Entity<Registrant>(entity =>
            {
                entity.HasKey(registrant => registrant.Id);
            });

            builder.Entity<Feedback>(entity =>
            {
                entity.HasKey(feedback => feedback.Id);
            });
        }
    }
}
