using HedgeDev.Editor.Material.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HEIO.NET.Json
{
    internal static class JsonConverters
    {
        public static JsonSerializerOptions Options { get; }

        static JsonConverters()
        {
            Options = new()
            {
                WriteIndented = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };
            Options.Converters.Add(new Vector4JsonConverter());
            Options.Converters.Add(new Vector4IntJsonConverter());
            Options.Converters.Add(new JsonStringEnumConverter());
            Options.Converters.Add(new MaterialParameterJsonConverterFactory());
            Options.Converters.Add(new MaterialJsonConverter());
            Options.Converters.Add(new TexsetJsonConverter());
            Options.Converters.Add(new TextureJsonConverter());
        }
    }
}
