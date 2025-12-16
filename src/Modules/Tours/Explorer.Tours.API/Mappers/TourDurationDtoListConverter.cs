using System.Text.Json;
using System.Text.Json.Serialization;
using Explorer.Tours.API.Dtos;

namespace Explorer.Tours.API.Mappers
{
    public class TourDurationDtoListConverter : JsonConverter<List<TourDurationDto>>
    {
        public override List<TourDurationDto> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartArray)
            {
                var durations = new List<TourDurationDto>();
                while (reader.Read())
                {
                    if (reader.TokenType == JsonTokenType.EndArray)
                    {
                        return durations;
                    }

                    // Use default deserialization for the object in the array
                    var duration = JsonSerializer.Deserialize<TourDurationDto>(ref reader, options);
                    if (duration != null)
                    {
                        durations.Add(duration);
                    }
                }
                return durations;
            }

            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException("Expected StartObject or StartArray");
            }

            var objectDurations = new List<TourDurationDto>();

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    return objectDurations;
                }

                if (reader.TokenType != JsonTokenType.PropertyName)
                {
                    throw new JsonException();
                }

                var propertyName = reader.GetString();
                reader.Read();

                if (reader.TokenType != JsonTokenType.Number)
                {
                    throw new JsonException();
                }

                var durationMinutes = reader.GetInt32();

                objectDurations.Add(new TourDurationDto
                {
                    TransportType = propertyName,
                    DurationMinutes = durationMinutes
                });
            }

            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, List<TourDurationDto> value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            foreach (var duration in value)
            {
                writer.WriteNumber(duration.TransportType, duration.DurationMinutes);
            }

            writer.WriteEndObject();
        }
    }
}
