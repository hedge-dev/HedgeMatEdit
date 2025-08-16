using J113D.Common.Json;
using SharpNeedle.Framework.HedgehogEngine.Mirage;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace HedgeDev.Editor.Material.Json
{
    internal abstract class SampleChunkResourceJsonConverter<T> : SimpleJsonObjectConverter<T> where T : SampleChunkResource, new()
    {
        protected const string _name = nameof(SampleChunkResource.Name);
        protected const string _dataVersion = nameof(SampleChunkResource.DataVersion);
        protected const string _sampleChunkRoot = "SampleChunkRoot";

        public override ReadOnlyDictionary<string, PropertyDefinition> PropertyDefinitions { get; } = new(new Dictionary<string, PropertyDefinition>()
        {
            { _name, new(PropertyTokenType.String, string.Empty) },
            { _dataVersion, new(PropertyTokenType.Number, 0u) },
            { _sampleChunkRoot, new(PropertyTokenType.Object, null, true) }
        });


        protected override object? ReadValue(ref Utf8JsonReader reader, string propertyName, ReadOnlyDictionary<string, object?> values, JsonSerializerOptions options)
        {
            switch(propertyName)
            {
                case _name:
                    return reader.GetString();
                case _dataVersion:
                    return reader.GetUInt32();
                case _sampleChunkRoot:
                    return JsonSerializer.Deserialize<SampleChunkNode>(ref reader, options);
                default:
                    throw new InvalidPropertyException();
            }
        }

        protected override T Create(ReadOnlyDictionary<string, object?> values)
        {
            values.TryGetValue(_sampleChunkRoot, out object? root);

            T result = new()
            {
                Name = (string)values[_name]!,
                DataVersion = (uint)values[_dataVersion]!,
                Root = (SampleChunkNode?)root
            };

            return result;
        }

        protected override void WriteValues(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            writer.WriteString(_name, value.Name);

            if(value.DataVersion != (uint)PropertyDefinitions[_dataVersion].Default!)
            {
                writer.WriteNumber(_dataVersion, value.DataVersion);
            }

            if(PropertyDefinitions.ContainsKey(_sampleChunkRoot) && value.Root != null)
            {
                writer.WritePropertyName(_sampleChunkRoot);
                JsonSerializer.Serialize(writer, value.Root);
            }
        }
    }
}
