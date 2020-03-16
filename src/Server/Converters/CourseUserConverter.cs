using System;
using KnowledgeShare.Store.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KnowledgeShare.Server.Converters
{
    public class CourseUserConverter : JsonConverter
    {
        public override bool CanRead => false;

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(CourseUser);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var user = (CourseUser)value;
            var obj = new
            {
                user.Id,
                user.Email,
                Role = user.Role.ToString(),
            };

            JToken.FromObject(obj).WriteTo(writer);
        }
    }
}
