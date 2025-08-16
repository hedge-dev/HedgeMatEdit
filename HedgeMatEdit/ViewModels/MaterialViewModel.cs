using HedgeDev.Editor.Material.ViewModels.Parameter;
using J113D.Avalonia.Utilities.Enum;
using SharpNeedle.Framework.HedgehogEngine.Mirage;
using SharpNeedle.Framework.HedgehogEngine.Mirage.MaterialData;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using static J113D.UndoRedo.GlobalChangeTracker;

namespace HedgeDev.Editor.Material.ViewModels
{
    internal class MaterialViewModel : ViewModelBase
    {
        private readonly HEMaterial _data;

        private MaterialVersion _version;
        private string _alphaTresholdText;

        private string _committedAlphaTresholdText;
        private byte _committedAlphaThreshold;


        public static EnumDescription[] MaterialVersions = EnumUtils.ToDescriptions<MaterialVersion>().ToArray();
        public static EnumDescription[] MaterialBlendModes = EnumUtils.ToDescriptions<MaterialBlendMode>().ToArray();

        public TextureSetViewModel TextureSet { get; }
        public FloatParametersViewModel FloatParameters { get; }
        public IntParametersViewModel IntParameters { get; }
        public BoolParametersViewModel BoolParameters { get; }
        public SCAParametersViewModel SCAParameters { get; }


        public MaterialVersion Version
        {
            get => _version;
            set
            {
                if(_version == value)
                {
                    return;
                }

                BeginChangeGroup("MaterialViewModel.Version");
                TrackFieldChange(this, nameof(_version), value);
                this.AddChangeGroupInvokePropertyChanged(nameof(Version));
                this.AddChangeGroupInvokePropertyChanged(nameof(ShowSCAParameters));
                EndChangeGroup();
            }
        }

        public bool ShowSCAParameters 
            => Version == MaterialVersion.LostWorldAndNewer;

        public string ShaderName
        {
            get => _data.ShaderName ?? string.Empty;
            set
            {
                if(_data.ShaderName == value)
                {
                    return;
                }

                BeginChangeGroup("MaterialViewModel.ShaderName");
                TrackPropertyChange(_data, nameof(HEMaterial.ShaderName), value, "Material.ShaderName");
                this.AddChangeGroupInvokePropertyChanged(nameof(ShaderName));
                EndChangeGroup();
            }
        }

        public float AlphaThreshold
        {
            get => _data.AlphaThreshold / (float)byte.MaxValue;
            set
            {
                byte newValue = (byte)float.Round(float.Clamp(value, 0, 1) * byte.MaxValue);
                _data.AlphaThreshold = newValue;
                _alphaTresholdText = (AlphaThreshold * 100).ToString("0.0", CultureInfo.InvariantCulture) + "%";
                InvokePropertyChanged(nameof(AlphaThresholdText));
            }
        }

        public string AlphaThresholdText
        {
            get => _alphaTresholdText;
            set
            {
                value = value.TrimEnd();

                string floatText = value;
                if(floatText.EndsWith('%'))
                {
                    floatText = floatText.TrimEnd()[..^1];
                }

                if(!float.TryParse(floatText, CultureInfo.InvariantCulture, out float newValue))
                {
                    throw new ArgumentException("Text has to be a number!");
                }

                byte newValue2 = (byte)float.Round(float.Clamp(newValue * 0.01f, 0, 1) * byte.MaxValue);

                if(_data.AlphaThreshold == newValue2)
                {
                    return;
                }

                BeginChangeGroup("MaterialViewModel.AlphaThresholdText");
                TrackPropertyChange(_data, nameof(HEMaterial.AlphaThreshold), newValue2);
                TrackFieldChange(this, nameof(_committedAlphaThreshold), newValue2);
                TrackFieldChange(this, nameof(_alphaTresholdText), value);
                TrackFieldChange(this, nameof(_committedAlphaTresholdText), value);
                this.AddChangeGroupInvokePropertyChanged(nameof(AlphaThreshold));
                this.AddChangeGroupInvokePropertyChanged(nameof(AlphaThresholdText));
                EndChangeGroup();
            }
        }

        public bool NoBackFaceCulling
        {
            get => _data.NoBackFaceCulling;
            set
            {
                if(_data.NoBackFaceCulling == value)
                {
                    return;
                }

                BeginChangeGroup("MaterialViewModel.NoBackFaceCulling");
                TrackPropertyChange(_data, nameof(HEMaterial.NoBackFaceCulling), value);
                this.AddChangeGroupInvokePropertyChanged(nameof(NoBackFaceCulling));
                EndChangeGroup();
            }
        }

        public MaterialBlendMode BlendMode
        {
            get => _data.BlendMode;
            set
            {
                if(_data.BlendMode == value)
                {
                    return;
                }

                BeginChangeGroup("MaterialViewModel.BlendMode");
                TrackPropertyChange(_data, nameof(HEMaterial.BlendMode), value);
                this.AddChangeGroupInvokePropertyChanged(nameof(BlendMode));
                EndChangeGroup();
            }
        }


        public MaterialViewModel(HEMaterial data)
        {
            _data = data;

            if(data.Root != null)
            {
                _version = MaterialVersion.LostWorldAndNewer;
            }
            else if(data.DataVersion > 1)
            {
                _version = MaterialVersion.Generations;
            }
            else
            {
                _version = MaterialVersion.Unleashed;
            }

            TextureSet = new(_data.Texset);
            FloatParameters = new(_data.FloatParameters);
            IntParameters = new(_data.IntParameters);
            BoolParameters = new(_data.BoolParameters);

            SampleChunkUtil.EnsureStructure(_data, true, out SampleChunkNode? scaParamNode);
            SCAParameters = new(scaParamNode!);

            _alphaTresholdText = (AlphaThreshold * 100).ToString("0.0", CultureInfo.InvariantCulture) + "%";

            _committedAlphaThreshold = _data.AlphaThreshold;
            _committedAlphaTresholdText = _alphaTresholdText;
        }

        
        public void CommitSliderValue()
        {
            BeginChangeGroup("MaterialViewModel.CommitSliderValue");

            byte newValue = _data.AlphaThreshold;
            _data.AlphaThreshold = _committedAlphaThreshold;
            TrackPropertyChange(_data, nameof(HEMaterial.AlphaThreshold), newValue);
            TrackFieldChange(this, nameof(_committedAlphaThreshold), newValue);


            string newTextValue = _alphaTresholdText;
            _alphaTresholdText = _committedAlphaTresholdText;
            TrackFieldChange(this, nameof(_alphaTresholdText), newTextValue);
            TrackFieldChange(this, nameof(_committedAlphaTresholdText), newTextValue);

            this.AddChangeGroupInvokePropertyChanged(nameof(AlphaThreshold));
            this.AddChangeGroupInvokePropertyChanged(nameof(AlphaThresholdText));

            EndChangeGroup();
        }
    
        public HEMaterial GetWriteMaterial(string exportName)
        {
            HEMaterial result = new()
            {
                Name = exportName,
                ShaderName = _data.ShaderName,
                AlphaThreshold = _data.AlphaThreshold,
                NoBackFaceCulling = _data.NoBackFaceCulling,
                BlendMode = _data.BlendMode
            };

            switch(_version)
            {
                case MaterialVersion.Unleashed:
                    result.DataVersion = 1;
                    result.Root = null;
                    break;
                case MaterialVersion.Generations:
                    result.DataVersion = 3;
                    result.Root = null;
                    break;
                case MaterialVersion.LostWorldAndNewer:
                    result.DataVersion = 3;

                    SampleChunkNode MirrorNode(SampleChunkNode source)
                    {
                        SampleChunkNode result = new(source.Name, source.Value);

                        if(source.Data != null)
                        {
                            result.Data = source.Data is HEMaterial ? result : source.Data;
                        }

                        foreach(SampleChunkNode node in source.Children)
                        {
                            result.AddChild(MirrorNode(node));
                        }

                        return result;
                    }

                    result.Root = MirrorNode(_data.Root!);
                    SampleChunkUtil.EnsureStructure(result, false, out _);
                    break;
            }

            void CopyParameters<T>(Dictionary<string, MaterialParameter<T>> from, Dictionary<string, MaterialParameter<T>> to) where T : unmanaged
            {
                foreach(KeyValuePair<string, MaterialParameter<T>> parameter in from)
                {
                    to.Add(parameter.Key, new() { Value = parameter.Value.Value });
                }
            }

            CopyParameters(_data.FloatParameters, result.FloatParameters);
            CopyParameters(_data.IntParameters, result.IntParameters);
            CopyParameters(_data.BoolParameters, result.BoolParameters);

            result.Texset.Name = exportName;

            foreach(Texture texture in _data.Texset.Textures)
            {
                result.Texset.Textures.Add(new()
                {
                    PictureName = texture.PictureName,
                    TexCoordIndex = texture.TexCoordIndex,
                    WrapModeU = texture.WrapModeU,
                    WrapModeV = texture.WrapModeV,
                    Type = texture.Type,
                    Name = exportName + "-" + result.Texset.Textures.Count.ToString("D4")
                });
            }

            return result;
        }

    }
}
