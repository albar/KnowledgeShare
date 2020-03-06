using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace KnowledgeShare.Store.Core
{
    public class Course
    {
        public string Id { get; } = Guid.NewGuid().ToString();
        public virtual CourseUser Author { get; set; }
        public string Title { get; set; }
        public virtual CourseUser Speaker { get; set; }
        public string Description { get; set; }
        public CourseVisibility Visibility { get; set; }
        public virtual ILocation Location
        {
            get
            {
                if (_locationCache is { })
                {
                    return _locationCache;
                }
                if (_location == null)
                {
                    return null;
                }

                return _locationCache ??= (ILocation)JsonConvert.DeserializeObject(
                    _location, Type.GetType(_locationType));
            }
            set
            {
                _locationCache = value;

                if (value == null)
                {
                    _locationType = nameof(ILocation);
                    _locationType = null;
                    return;
                }

                _locationType = value.GetType().FullName;
                _location = JsonConvert.SerializeObject(value);
            }
        }
        public virtual List<Session> Sessions { get; set; } = new List<Session>();

        public virtual List<Registrant> Registrants { get; } = new List<Registrant>();
        public virtual List<Feedback> Feedbacks { get; } = new List<Feedback>();

        private string _location = null;
        private string _locationType = nameof(ILocation);
        private ILocation _locationCache = null;
    }
}
