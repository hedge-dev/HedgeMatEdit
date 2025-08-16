using HedgeDev.Editor.Material.ViewModels;
using J113D.Avalonia.Theme;
using J113D.Avalonia.Utilities.Enum;
using System;
using System.Linq;

namespace HedgeDev.Editor.Material.Config
{
    internal sealed class SettingsViewModel : ViewModelBase
    {
        private string _fontSizeText;
        private readonly Settings _settings;

        public static EnumDescription[] ThemeValues = EnumUtils.ToDescriptions<J113DThemeVariant>().ToArray();
        public static EnumDescription[] MaterialVersions = EnumUtils.ToDescriptions<MaterialVersion>().ToArray();

        public J113DThemeVariant Theme
        {
            get => _settings.Theme;
            set => _settings.Theme = value;
        }

        public string FontSizeText
        {
            get => _fontSizeText;
            set
            {
                if(!int.TryParse(value, out int fontSize))
                {
                    throw new ArgumentException("Text has to be a number!");
                }

                if(fontSize < 10)
                {
                    throw new ArgumentException("Font size too tiny! Needs to be at least 10");
                }
                else if(fontSize >= 48)
                {
                    throw new ArgumentException("Font size too large! Needs to be below 48");
                }

                _settings.FontSize = fontSize;
                _fontSizeText = fontSize.ToString();
                InvokePropertyChanged(nameof(FontSize));
            }
        }

        public int FontSize
        {
            get => _settings.FontSize;
            set
            {
                _settings.FontSize = value;
                _fontSizeText = value.ToString();
                InvokePropertyChanged(nameof(FontSizeText));
            }
        }

        public bool ShowIntParameters
        {
            get => _settings.ShowIntParameters;
            set
            {
                _settings.ShowIntParameters = value;
                InvokePropertyChanged(nameof(ShowIntParameters));
            }
        }

        public MaterialVersion DefaultMaterialVersion
        {
            get => _settings.DefaultMaterialVersion;
            set => _settings.DefaultMaterialVersion = value;
        }


        public SettingsViewModel()
        {
            _settings = new();
            _settings.Reset();
            _fontSizeText = _settings.FontSize.ToString();
        }


        public void Load()
        {
            _settings.Load();
            _fontSizeText = _settings.FontSize.ToString();
            InvokePropertyChanged(nameof(Theme));
            InvokePropertyChanged(nameof(FontSizeText));
            InvokePropertyChanged(nameof(FontSize));
            InvokePropertyChanged(nameof(ShowIntParameters));
            InvokePropertyChanged(nameof(DefaultMaterialVersion));
        }

        public void Save()
        {
            _settings.Save();
        }
    }
}
