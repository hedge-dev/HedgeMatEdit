using J113D.Common.Json;
using SharpNeedle.Framework.HedgehogEngine.Mirage.MaterialData;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace HedgeDev.Editor.Material.Json
{
    internal class TexsetJsonConverter : SampleChunkResourceJsonConverter<Texset>
    {
        private const string _textures = nameof(Texset.Textures);

        public override ReadOnlyDictionary<string, PropertyDefinition> PropertyDefinitions { get; } = new(new Dictionary<string, PropertyDefinition>()
        {
            { _name, new(PropertyTokenType.String, string.Empty) },
            { _dataVersion, new(PropertyTokenType.Number, 0u) },

            { _textures , new(PropertyTokenType.Array, null) }
        });

        protected override object? ReadValue(ref Utf8JsonReader reader, string propertyName, ReadOnlyDictionary<string, object?> values, JsonSerializerOptions options)
        {
            switch(propertyName)
            {
                case _textures:
                    return JsonSerializer.Deserialize<Texture[]>(ref reader, options);
                default:
                    return base.ReadValue(ref reader, propertyName, values, options);
            }
        }
        protected override Texset Create(ReadOnlyDictionary<string, object?> values)
        {
            Texset result = base.Create(values);
            result.Textures.AddRange((Texture[])values[_textures]!);
            return result;
        }

        protected override void WriteValues(Utf8JsonWriter writer, Texset value, JsonSerializerOptions options)
        {
            base.WriteValues(writer, value, options);

            writer.WritePropertyName(_textures);
            JsonSerializer.Serialize(writer, value.Textures, options);
        }
    }
}
