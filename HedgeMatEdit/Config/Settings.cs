using J113D.Common;
using J113D.Avalonia.Theme;
using System.Text.Json;
using HedgeDev.Editor.Material.ViewModels;
using System;
using System.IO;

namespace HedgeDev.Editor.Material.Config
{
    internal sealed class Settings : BaseSettings
    {
        public J113DThemeVariant Theme
        {
            get => (J113DThemeVariant)this[nameof(Theme)];
            set => this[nameof(Theme)] = value;
        }

        public int FontSize
        {
            get => (int)this[nameof(FontSize)];
            set => this[nameof(FontSize)] = int.Clamp(value, 10, 47);
        }

        public bool ShowIntParameters
        {
            get => (bool)this[nameof(ShowIntParameters)];
            set => this[nameof(ShowIntParameters)] = value;
        }

        public MaterialVersion DefaultMaterialVersion
        {
            get => (MaterialVersion)this[nameof(DefaultMaterialVersion)];
            set => this[nameof(DefaultMaterialVersion)] = value;
        }

        public Settings() : base() { }

        public override void Reset()
        {
            Theme = J113DThemeVariant.Dark;
            FontSize = 14;
            ShowIntParameters = false;
            DefaultMaterialVersion = MaterialVersion.LostWorldAndNewer;
        }

        protected override object ConvertValue(string name, JsonElement value)
        {
            return name switch
            {
                nameof(Theme) => Enum.Parse<J113DThemeVariant>(value.GetString()!),
                nameof(FontSize) => value.GetInt32(),
                nameof(ShowIntParameters) => value.GetBoolean(),
                nameof(DefaultMaterialVersion) => Enum.Parse<MaterialVersion>(value.GetString()!),
                _ => throw new InvalidDataException(),
            };
        }

    }
}
