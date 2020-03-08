using System;
using System.Collections.Generic;
using System.Text.Json;

namespace KnowledgeShare.Store.Core
{
    public class Course
    {
        public string Id { get; } = Guid.NewGuid().ToString();
        public CourseUser Author { get; set; }
        public string Title { get; set; }
        public CourseUser Speaker { get; set; }
        public string Description { get; set; }
        public CourseVisibility Visibility { get; set; }
        public ILocation Location
        {
            get => GetLocation();
            set => SetLocation(value);
        }
        public List<Session> Sessions { get; set; } = new List<Session>();

        public List<Registrant> Registrants { get; } = new List<Registrant>();
        public List<Feedback> Feedbacks { get; } = new List<Feedback>();

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        private string _location = null;
        private string _locationType = typeof(ILocation).AssemblyQualifiedName;
        private ILocation _locationCache = null;

        private ILocation GetLocation()
        {
            if (_locationCache is { })
            {
                return _locationCache;
            }
            if (_location == null)
            {
                _locationType = typeof(ILocation).AssemblyQualifiedName;
                return null;
            }

            return _locationCache ??= (ILocation)JsonSerializer.Deserialize(
                _location, Type.GetType(_locationType));
        }

        private void SetLocation(ILocation location)
        {
            _locationCache = location;

            if (location == null)
            {
                _locationType = typeof(ILocation).AssemblyQualifiedName;
                _location = null;
                return;
            }

            _locationType = location.GetType().AssemblyQualifiedName;
            _location = JsonSerializer.Serialize(location);
        }
    }
}
