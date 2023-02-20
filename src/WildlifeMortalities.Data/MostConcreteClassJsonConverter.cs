using System.Text.Json;
using System.Text.Json.Serialization;

namespace WildlifeMortalities.Data;

public class MostConcreteClassJsonConverter<T> : JsonConverter<T>
{
    private const string TypeNodeName = "CLRTYPE";

    public override bool CanConvert(Type typeToConvert) =>
        typeToConvert.IsAssignableFrom(typeof(T));

    public override T? Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        var reader2 = reader;

        Type? type = null;
        while (reader.Read())
        {
            if (type != null)
            {
                continue;
            }

            if (reader.TokenType == JsonTokenType.PropertyName)
            {
                var propertyName = reader.GetString();
                if (propertyName == TypeNodeName)
                {
                    reader.Read();
                    var value = reader.GetString();
                    if (!string.IsNullOrEmpty(value))
                    {
                        type = Type.GetType(value);
                    }
                }
            }
        }

        if (type == null)
        {
            throw new InvalidOperationException("unable to find CheckinPropertyDescription type");
        }

        var result = JsonSerializer.Deserialize(
            ref reader2,
            type,
            type == typeof(T) ? null : options
        );
        if (result is not T description)
        {
            throw new InvalidOperationException("unable to find CheckinPropertyDescription type");
        }

        return description;
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        if (value != null)
        {
            var type = value.GetType();

            writer.WritePropertyName(TypeNodeName);
            writer.WriteStringValue(type.AssemblyQualifiedName);

            var doc = JsonSerializer.SerializeToDocument(value, type);
            foreach (var element in doc.RootElement.EnumerateObject())
            {
                element.WriteTo(writer);
            }
        }

        writer.WriteEndObject();
    }
}
