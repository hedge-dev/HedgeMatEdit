using HedgeDev.Editor.Material.Attributes;
using SharpNeedle.Framework.HedgehogEngine.Mirage.MaterialData;
using static J113D.UndoRedo.GlobalChangeTracker;
using System.Globalization;
using J113D.Avalonia.Utilities.Enum;
using System.Linq;

namespace HedgeDev.Editor.Material.ViewModels
{
    internal class TextureViewModel : ViewModelBase
    {
        internal readonly Texture _data;
        private readonly TextureSetViewModel _parent;

        private string _texcoordIndex;

        public static EnumDescription[] WrapModes = EnumUtils.ToDescriptions<VMWrapMode>().ToArray();

        public string Type
        {
            get => _data.Type ?? string.Empty;
            set
            {
                if(value == _data.Type)
                {
                    return;
                }

                BeginChangeGroup("TextureViewModel.Type");
                TrackPropertyChange(_data, nameof(Texture.Type), value);
                this.AddChangeGroupInvokePropertyChanged(nameof(Type));
                EndChangeGroup();
            }
        }

        public string PictureName
        {
            get => _data.PictureName ?? string.Empty;
            set
            {
                if(value == _data.PictureName)
                {
                    return;
                }

                BeginChangeGroup("TextureViewModel.PictureName");
                TrackPropertyChange(_data, nameof(Texture.PictureName), value);
                this.AddChangeGroupInvokePropertyChanged(nameof(PictureName));
                EndChangeGroup();
            }
        }

        [ByteTextValidation]
        public string TexcoordIndex
        {
            get => _texcoordIndex;
            set
            {
                if(value == _texcoordIndex)
                {
                    return;
                }

                BeginChangeGroup("TextureViewModel.TexcoordIndex");

                TrackFieldChange(this, nameof(_texcoordIndex), value);
                this.AddChangeGroupInvokePropertyChanged(nameof(TexcoordIndex));

                byte byteValue;
                try
                {
                    byteValue = ByteTextValidationAttribute.Validate(value);

                    if(byteValue != _data.TexCoordIndex)
                    {
                        TrackPropertyChange(_data, nameof(Texture.TexCoordIndex), byteValue);
                    }
                }
                finally
                {
                    EndChangeGroup();
                }
            }
        }

        public VMWrapMode WrapModeU
        {
            get => (VMWrapMode)_data.WrapModeU;
            set
            {
                WrapMode dataValue = (WrapMode)value;
                if(dataValue == _data.WrapModeU)
                {
                    return;
                }

                BeginChangeGroup("TextureViewModel.WrapModeU");
                TrackPropertyChange(_data, nameof(Texture.WrapModeU), dataValue);
                this.AddChangeGroupInvokePropertyChanged(nameof(WrapModeU));
                EndChangeGroup();
            }
        }

        public VMWrapMode WrapModeV
        {
            get => (VMWrapMode)_data.WrapModeV;
            set
            {
                WrapMode dataValue = (WrapMode)value;
                if(dataValue == _data.WrapModeU)
                {
                    return;
                }

                BeginChangeGroup("TextureViewModel.WrapModeV");
                TrackPropertyChange(_data, nameof(Texture.WrapModeV), dataValue);
                this.AddChangeGroupInvokePropertyChanged(nameof(WrapModeV));
                EndChangeGroup();
            }
        }

        public TextureViewModel(Texture data, TextureSetViewModel parent)
        {
            _data = data;
            _parent = parent;
            _texcoordIndex = _data.TexCoordIndex.ToString(CultureInfo.InvariantCulture);
        }

        public void Remove()
        {
            _parent.RemoveTexture(this);
        }
    }
}
