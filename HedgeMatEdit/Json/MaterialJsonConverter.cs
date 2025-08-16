using J113D.Common.Json;
using SharpNeedle.Framework.HedgehogEngine.Mirage;
using SharpNeedle.Framework.HedgehogEngine.Mirage.MaterialData;
using SharpNeedle.Structs;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Numerics;
using System.Text.Json;

namespace HedgeDev.Editor.Material.Json
{
    internal class MaterialJsonConverter : SampleChunkResourceJsonConverter<HEMaterial>
    {
        private const string _shaderName = nameof(HEMaterial.ShaderName);
        private const string _alphaThreshold = nameof(HEMaterial.AlphaThreshold);
        private const string _noBackFaceCulling = nameof(HEMaterial.NoBackFaceCulling);
        private const string _blendMode = nameof(HEMaterial.BlendMode);
        private const string _floatParameters = nameof(HEMaterial.FloatParameters);
        private const string _intParameters = nameof(HEMaterial.IntParameters);
        private const string _boolParameters = nameof(HEMaterial.BoolParameters);
        private const string _texset = nameof(HEMaterial.Texset);

        public override ReadOnlyDictionary<string, PropertyDefinition> PropertyDefinitions { get; } = new(new Dictionary<string, PropertyDefinition>()
        {
            { _name, new(PropertyTokenType.String, string.Empty) },
            { _dataVersion, new(PropertyTokenType.Number, 3u) },
            { _sampleChunkRoot, new(PropertyTokenType.Object, null, true) },

            { _shaderName, new(PropertyTokenType.String, string.Empty) },
            { _alphaThreshold, new(PropertyTokenType.Number, (byte)128) },
            { _noBackFaceCulling, new(PropertyTokenType.Bool, false) },
            { _blendMode, new(PropertyTokenType.String, MaterialBlendMode.Normal) },
            { _floatParameters, new(PropertyTokenType.Object, null, true) },
            { _intParameters, new(PropertyTokenType.Object, null, true) },
            { _boolParameters, new(PropertyTokenType.Object, null, true) },
            { _texset, new(PropertyTokenType.Object, null, true) }
        });

        protected override object? ReadValue(ref Utf8JsonReader reader, string propertyName, ReadOnlyDictionary<string, object?> values, JsonSerializerOptions options)
        {
            switch(propertyName)
            {
                case _shaderName:
                    return reader.GetString();
                case _alphaThreshold:
                    return reader.GetByte();
                case _noBackFaceCulling:
                    return reader.GetBoolean();
                case _blendMode:
                    return JsonSerializer.Deserialize<MaterialBlendMode>(ref reader, options);
                case _floatParameters:
                    return JsonSerializer.Deserialize<Dictionary<string, MaterialParameter<Vector4>>>(ref reader, options);
                case _intParameters:
                    return JsonSerializer.Deserialize<Dictionary<string, MaterialParameter<Vector4Int>>>(ref reader, options);
                case _boolParameters:
                    return JsonSerializer.Deserialize<Dictionary<string, MaterialParameter<bool>>>(ref reader, options);
                case _texset:
                    return JsonSerializer.Deserialize<Texset>(ref reader, options);
                default:
                    return base.ReadValue(ref reader, propertyName, values, options);
            }
        }

        protected override HEMaterial Create(ReadOnlyDictionary<string, object?> values)
        {
            HEMaterial result = base.Create(values);

            SampleChunkNode? contextsNode = result.Root?.FindNode("Contexts");
            if(contextsNode != null)
            {
                contextsNode.Data = result;
                contextsNode.Value = result.DataVersion;
            }
            

            result.ShaderName = (string)values[_shaderName]!;
            result.AlphaThreshold = (byte)values[_alphaThreshold]!;
            result.NoBackFaceCulling = (bool)values[_noBackFaceCulling]!;
            result.BlendMode = (MaterialBlendMode)values[_blendMode]!;

            void SetupParameters<T>(string name, Dictionary<string, MaterialParameter<T>> target) where T : unmanaged
            {
                Dictionary<string, MaterialParameter<T>>? dict = (Dictionary<string, MaterialParameter<T>>?)values[name];
                if(dict == null)
                {
                    return;
                }

                foreach(KeyValuePair<string, MaterialParameter<T>> item in dict)
                {
                    target.Add(item.Key, item.Value);
                }
            }

            SetupParameters(_floatParameters, result.FloatParameters);
            SetupParameters(_intParameters, result.IntParameters);
            SetupParameters(_boolParameters, result.BoolParameters);

            if(values[_texset] is Texset texset)
            {
                result.Texset = texset;
            }

            return result;
        }

        protected override void WriteValues(Utf8JsonWriter writer, HEMaterial value, JsonSerializerOptions options)
        {
            base.WriteValues(writer, value, options);

            writer.WriteString(_shaderName, value.ShaderName);

            if(value.AlphaThreshold != 128)
            {
                writer.WriteNumber(_alphaThreshold, value.AlphaThreshold);
            }

            if(value.NoBackFaceCulling)
            {
                writer.WriteBoolean(_noBackFaceCulling, value.NoBackFaceCulling);
            }
            
            if(value.BlendMode != MaterialBlendMode.Normal)
            {
                writer.WritePropertyName(_blendMode);
                JsonSerializer.Serialize(writer, value.BlendMode, options);
            }
            
            if(value.FloatParameters.Count > 0)
            {
                writer.WritePropertyName(_floatParameters);
                JsonSerializer.Serialize(writer, value.FloatParameters, options);
            }

            if(value.IntParameters.Count > 0)
            {
                writer.WritePropertyName(_intParameters);
                JsonSerializer.Serialize(writer, value.IntParameters, options);
            }

            if(value.BoolParameters.Count > 0)
            {
                writer.WritePropertyName(_boolParameters);
                JsonSerializer.Serialize(writer, value.BoolParameters, options);
            }

            if(value.Texset.Textures.Count > 0)
            {
                writer.WritePropertyName(_texset);
                JsonSerializer.Serialize(writer, value.Texset, options);
            }
        }
    }
}
