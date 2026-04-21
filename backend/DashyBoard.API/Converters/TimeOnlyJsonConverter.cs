using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DashyBoard.API.Converters;

public class TimeOnlyJsonConverter : JsonConverter<TimeOnly>
{
    private static readonly string[] Formats = ["HH:mm:ss", "HH:mm", "H:mm:ss", "H:mm"];

    public override TimeOnly Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        var value = reader.GetString();

        if (
            TimeOnly.TryParseExact(
                value,
                Formats,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var result
            )
        )
            return result;

        throw new JsonException(
            $"Kunde inte konvertera \"{value}\" till TimeOnly. Använd formatet HH:mm eller HH:mm:ss."
        );
    }

    public override void Write(Utf8JsonWriter writer, TimeOnly value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString("HH:mm:ss"));
    }
}
