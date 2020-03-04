using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KnowledgeShare.Entity;
using KnowledgeShare.Manager.Abstractions;
using KnowledgeShare.Manager.Exceptions;
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
            List<Session> sessions)
        {
            var errors = new ValidationErrorsBag();

            if (author == null)
            {
                errors.AddInto("author", $"Author is required");
            }
            else if (!await _userManager.IsExistedAsync(author.Id)) // validate author is exists
            {
                errors.AddInto("author", $"Author with id='{author.Id}' is not registered");
            }
            else if ( // validate author is either Administrator or Manager
                author.Role != CourseUserRole.Administrator &&
                author.Role != CourseUserRole.Manager)
            {
                // throw new NotImplementedException("Role validation is not implemented");
                errors.AddInto("author", "Author should be either Administrator or Manager");
            }

            // validate title is not null or whitespace
            if (string.IsNullOrWhiteSpace(title))
            {
                errors.AddInto("title", "Title cannot be empty");
            }

            if (speaker == null)
            {
                errors.AddInto("speaker", $"Speaker is required");
            }
            else if (!await _userManager.IsExistedAsync(speaker.Id)) // validate speaker is exists in database
            {
                errors.AddInto("speaker", $"Speaker with id='{speaker.Id}' is not registered");
            }

            // validate location based on its type
            if (location == null)
            {
                errors.AddInto("location", "Location is required");
            }
            else if (location is OnlineLocation online &&
                !Uri.TryCreate(online.Url, UriKind.Absolute, out var _))
            {
                errors.AddInto("location", "Location URL is not valid");
            }

            // validate sessions at least one
            if (sessions.Count < 1)
            {
                errors.AddInto("sessions", "At least one session required");
            }

            if (errors.Count > 0)
            {
                throw new ValidationException(errors);
            }

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

        public Abstractions.ICollection<Course> GetAllAccessibleToUser(ICourseUser accessor)
        {
            return _courseStore.Query.Where(course => course.Visibility == Visibility.Public
                    || accessor.Role == CourseUserRole.Administrator
                    || (accessor.Role == CourseUserRole.Manager && course.Author.Equals(accessor))
                    || course.Invitee.Any(invitee => invitee.User.Equals(accessor))
                    || course.Attendee.Any(attendee => attendee.User.Equals(accessor))
                ).ToCollection();
        }

        public async Task<Course> FindAccessibleToUserByIdAsync(ICourseUser accessor, string id)
        {
            return await _courseStore.FindByIdAsync(id);
        }
    }
}