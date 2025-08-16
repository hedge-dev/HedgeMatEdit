using J113D.Common.Json;
using SharpNeedle.Framework.HedgehogEngine.Mirage.MaterialData;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace HedgeDev.Editor.Material.Json
{
    internal class TextureJsonConverter : SampleChunkResourceJsonConverter<Texture>
    {
        private const string _type = nameof(Texture.Type);
        private const string _pictureName = nameof(Texture.PictureName);
        private const string _texCoordIndex = nameof(Texture.TexCoordIndex);
        private const string _wrapModeU = nameof(Texture.WrapModeU);
        private const string _wrapModeV = nameof(Texture.WrapModeV);

        public override ReadOnlyDictionary<string, PropertyDefinition> PropertyDefinitions { get; } = new(new Dictionary<string, PropertyDefinition>()
        {
            { _name, new(PropertyTokenType.String, string.Empty) },
            { _dataVersion, new(PropertyTokenType.Number, 1u) },

            { _type , new(PropertyTokenType.String, string.Empty) },
            { _pictureName , new(PropertyTokenType.String, string.Empty) },
            { _texCoordIndex , new(PropertyTokenType.Number, (byte)0) },
            { _wrapModeU , new(PropertyTokenType.String, WrapMode.Repeat) },
            { _wrapModeV , new(PropertyTokenType.String, WrapMode.Repeat) },
        });

        protected override object? ReadValue(ref Utf8JsonReader reader, string propertyName, ReadOnlyDictionary<string, object?> values, JsonSerializerOptions options)
        {
            switch(propertyName)
            {
                case _pictureName:
                case _type:
                    return reader.GetString();
                case _texCoordIndex:
                    return reader.GetByte();
                case _wrapModeU:
                case _wrapModeV:
                    return JsonSerializer.Deserialize<WrapMode>(ref reader, options);
                default:
                    return base.ReadValue(ref reader, propertyName, values, options);
            }
        }
        protected override Texture Create(ReadOnlyDictionary<string, object?> values)
        {
            Texture result = base.Create(values);

            result.Type = (string)values[_type]!;
            result.PictureName = (string)values[_pictureName]!;
            result.TexCoordIndex = (byte)values[_texCoordIndex]!;
            result.WrapModeU = (WrapMode)values[_wrapModeU]!;
            result.WrapModeV = (WrapMode)values[_wrapModeV]!;

            return result;
        }

        protected override void WriteValues(Utf8JsonWriter writer, Texture value, JsonSerializerOptions options)
        {
            base.WriteValues(writer, value, options);

            writer.WriteString(_type, value.Type);
            writer.WriteString(_pictureName, value.PictureName ?? string.Empty);

            if(value.TexCoordIndex != 0)
            {
                writer.WriteNumber(_texCoordIndex, value.TexCoordIndex);
            }

            if(value.WrapModeU != WrapMode.Repeat)
            {
                writer.WritePropertyName(_wrapModeU);
                JsonSerializer.Serialize(writer, value.WrapModeU, options);
            }

            if(value.WrapModeV != WrapMode.Repeat)
            {
                writer.WritePropertyName(_wrapModeV);
                JsonSerializer.Serialize(writer, value.WrapModeV, options);
            }
        }
    }
}
