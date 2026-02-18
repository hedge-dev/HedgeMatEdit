using HedgeDev.Editor.Material.Config;
using HedgeDev.Editor.Material.ViewModels.Base;
using HedgeDev.Editor.Material.ViewModels.Resource;
using HEIO.NET.Json;
using J113D.UndoRedo;
using SharpNeedle.IO;
using SharpNeedle.Resource;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;

namespace HedgeDev.Editor.Material.ViewModels
{
    internal class MainViewModel : ViewModelBase
    {
        private readonly ObservableCollection<MaterialViewModel> _materials;
        private readonly ChangeTracker _mainChangeTracker;
        private MaterialViewModel? _activeMaterial;

        private readonly SettingsViewModel _settings;

        public ReadOnlyObservableCollection<MaterialViewModel> Materials { get; private set; }
        
        public MaterialViewModel? ActiveMaterial
        {
            get => _activeMaterial;
            set
            {
                _activeMaterial = value;
                (_activeMaterial?.ChangeTracker ?? _mainChangeTracker).UseTracker();
            }
        }

        public MainViewModel(SettingsViewModel settings)
        {
            _mainChangeTracker = new();
            _mainChangeTracker.UseTracker();
            _settings = settings;
            _materials = [];
            Materials = new(_materials);
        }

        private void AddAsActive(HEMaterial material)
        {
            MaterialViewModel viewmodel = new(material);
            _materials.Add(viewmodel);
            ActiveMaterial = viewmodel;
        }

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

            AddAsActive(material);
        }

        public void ReadMaterial(IFile file)
        {
            HEMaterial material = new();
            material.Read(file);
            material.ResolveDependencies(new DirectoryResourceResolver(file.Parent));

            AddAsActive(material);
        }

        public void RemoveActiveMaterial()
        {
            if(ActiveMaterial == null)
            {
                return;
            }

            MaterialViewModel active = ActiveMaterial;

            if(Materials.Count == 1)
            {
                ActiveMaterial = null;
                _materials.Remove(active);
            }
            else
            {
                int index = Materials.IndexOf(active);

                if(index == Materials.Count - 1)
                {
                    index--;
                }
                else
                {
                    index++;
                }

                ActiveMaterial = Materials[index];
            }

            _materials.Remove(active);
        }

        public void ClearMaterials()
        {
            ActiveMaterial = null;
            _materials.Clear();
        }

        public string ExportJson(string filename)
        {
            if(ActiveMaterial == null)
            {
                throw new NullReferenceException("No active material!");
            }

            HEMaterial material = ActiveMaterial.GetWriteMaterial(Path.GetFileNameWithoutExtension(filename));
            return JsonSerializer.Serialize(material, JsonConverters.Options);
        }

        public void ImportJson(string json)
        {
            HEMaterial material = JsonSerializer.Deserialize<HEMaterial>(json, JsonConverters.Options)!;
            AddAsActive(material);
        }

    }
}
