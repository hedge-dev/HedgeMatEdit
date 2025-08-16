using SharpNeedle.Structs;
using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HEIO.NET.Json
{
    internal class Vector4IntJsonConverter : JsonConverter<Vector4Int>
    {
        public override Vector4Int Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if(reader.TokenType != JsonTokenType.String)
            {
                throw new JsonException("Expected a string for Vector4Int!");
            }

            string[] values = reader.GetString()!.Split(' ');
            return new(
                int.Parse(values[0], CultureInfo.InvariantCulture),
                int.Parse(values[1], CultureInfo.InvariantCulture),
                int.Parse(values[2], CultureInfo.InvariantCulture),
                int.Parse(values[3], CultureInfo.InvariantCulture)
            );
        }

        public override void Write(Utf8JsonWriter writer, Vector4Int value, JsonSerializerOptions options)
        {
            writer.WriteStringValue($"{value.X} {value.Y} {value.Z} {value.W}");
        }
    }
}
