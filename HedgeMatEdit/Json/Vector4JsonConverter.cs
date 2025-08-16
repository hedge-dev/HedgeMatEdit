using System;
using System.Globalization;
using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HEIO.NET.Json
{
    internal class Vector4JsonConverter : JsonConverter<Vector4>
    {
        public override Vector4 Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if(reader.TokenType != JsonTokenType.String)
            {
                throw new JsonException("Expected a string for Vector4!");
            }

            string[] values = reader.GetString()!.Split(',');
            return new(
                float.Parse(values[0], CultureInfo.InvariantCulture),
                float.Parse(values[1], CultureInfo.InvariantCulture),
                float.Parse(values[2], CultureInfo.InvariantCulture),
                float.Parse(values[3], CultureInfo.InvariantCulture)
            );
        }

        public override void Write(Utf8JsonWriter writer, Vector4 value, JsonSerializerOptions options)
        {
            const string format = "0.0####";

            string output =
                value.X.ToString(format, CultureInfo.InvariantCulture)
                + ", "
                + value.Y.ToString(format, CultureInfo.InvariantCulture)
                + ", "
                + value.Z.ToString(format, CultureInfo.InvariantCulture)
                + ", "
                + value.W.ToString(format, CultureInfo.InvariantCulture);

            writer.WriteStringValue(output);
        }
    }
}
