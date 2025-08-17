using HedgeDev.Editor.Material.Config;
using HEIO.NET.Json;
using SharpNeedle.IO;
using SharpNeedle.Resource;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text.Json;

namespace HedgeDev.Editor.Material.ViewModels
{
    internal class MainViewModel : ViewModelBase
    {
        private readonly SettingsViewModel _settings;

        public MaterialViewModel Material { get; private set; }


        public MainViewModel(SettingsViewModel settings)
        {
            _settings = settings;
            NewMaterial();
        }


        [MemberNotNull(nameof(Material))]
        public void NewMaterial()
        {
            HEMaterial material = new()
            {
                ShaderName = "Common_d",
                AlphaThreshold = 128
            };

            switch(_settings.DefaultMaterialVersion)
            {
                case MaterialVersion.Unleashed:
                    material.DataVersion = 1;
                    break;
                case MaterialVersion.Generations:
                    material.DataVersion = 3;
                    break;
                case MaterialVersion.LostWorldAndNewer:
                    material.DataVersion = 3;
                    material.SetupNodes();
                    break;
            }

            Material = new(material);
        }

        public void ReadMaterial(IFile file)
        {
            HEMaterial material = new();
            material.Read(file);
            material.ResolveDependencies(new DirectoryResourceResolver(file.Parent));
            Material = new(material);
        }

        public void WriteMaterial(IFile file)
        {
            if(Material == null)
            {
                throw new InvalidOperationException("No material data");
            }

            Material.GetWriteMaterial(Path.GetFileNameWithoutExtension(file.Name)).Write(file);
        }
    
        public string ExportJson(string filename)
        {
            HEMaterial material = Material.GetWriteMaterial(Path.GetFileNameWithoutExtension(filename));
            return JsonSerializer.Serialize(material, JsonConverters.Options);
        }

        public void ImportJson(string json)
        {
            HEMaterial material = JsonSerializer.Deserialize<HEMaterial>(json, JsonConverters.Options)!;
            Material = new(material);
        }
    }
}
