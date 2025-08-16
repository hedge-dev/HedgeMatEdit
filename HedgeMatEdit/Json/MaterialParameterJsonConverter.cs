using SharpNeedle.Framework.HedgehogEngine.Mirage.MaterialData;
using SharpNeedle.Structs;
using System;
using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HedgeDev.Editor.Material.Json
{
    internal class MaterialParameterJsonConverter<T> : JsonConverter<MaterialParameter<T>> where T : unmanaged
    {
        public override MaterialParameter<T>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return new() { Value = JsonSerializer.Deserialize<T>(ref reader, options) };
        }

        public override void Write(Utf8JsonWriter writer, MaterialParameter<T> value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value.Value, options);
        }
    }

    internal class MaterialParameterJsonConverterFactory : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert == typeof(MaterialParameter<Vector4>)
                || typeToConvert == typeof(MaterialParameter<Vector4Int>)
                || typeToConvert == typeof(MaterialParameter<bool>);
        }

        public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            Type converterType = typeof(MaterialParameterJsonConverter<>).MakeGenericType(typeToConvert.GenericTypeArguments);
            return (JsonConverter)Activator.CreateInstance(converterType)!;
        }
    }
}
