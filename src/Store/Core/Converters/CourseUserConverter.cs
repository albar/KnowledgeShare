using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace KnowledgeShare.Store.Core.Converters
{
    public class CourseUserConverter : JsonConverter<CourseUser>
    {
        public override CourseUser Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return JsonSerializer.Deserialize<CourseUser>(reader.GetString());
        }

        public override void Write(Utf8JsonWriter writer, CourseUser value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WritePropertyName(JsonEncodedText.Encode("id"));
            writer.WriteStringValue(JsonEncodedText.Encode(value.Id));
            writer.WritePropertyName(JsonEncodedText.Encode("email"));
            writer.WriteStringValue(JsonEncodedText.Encode(value.Email));
            writer.WritePropertyName(JsonEncodedText.Encode("role"));
            writer.WriteStringValue(JsonEncodedText.Encode(value.Role.ToString()));
            writer.WriteEndObject();
        }
    }
}
