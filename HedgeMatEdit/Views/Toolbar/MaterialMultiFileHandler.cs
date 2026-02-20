using Avalonia;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using HedgeDev.Editor.Material.ViewModels;
using HedgeDev.Editor.Material.ViewModels.Resource;
using J113D.Avalonia.Utilities.IO;
using SharpNeedle.IO;
using System;
using System.Collections.Generic;
using System.IO;

namespace HedgeDev.Editor.Material.Views.Toolbar
{
    internal class MaterialMultiFileHandler : BaseMultiFileHandler<MaterialViewModel>
    {
        protected override string FileTypeName
            => "Hedgehog Engine Material";

        public override IReadOnlyList<FilePickerFileType>? FileType { get; } =
        [
            new("Hedgehog Engine Material") {
                Patterns = ["*.material"]
            }
        ];

        private readonly Visual _visual;

        protected MainViewModel ViewModel
            => (MainViewModel)_visual.DataContext!;

        protected override Window Window
            => _visual as Window ?? (Window)TopLevel.GetTopLevel(_visual)!;

        public MaterialMultiFileHandler(Visual visual)
        {
            _visual = visual;
        }

        protected override IDataChangeState? GetDataChangeState(MaterialViewModel data)
        {
            return data;
        }

        protected override MaterialViewModel InternalLoad(Uri filePath)
        {
            IFile file = FileSystem.Instance.Open(filePath.LocalPath)!;
            ViewModel.ReadMaterial(file);
            return ViewModel.ActiveMaterial!;
        }

        protected override MaterialViewModel InternalNew()
        {
            ViewModel.NewMaterial();
            return ViewModel.ActiveMaterial!;
        }

        protected override void InternalSave(MaterialViewModel data, Uri filePath)
        {
            IFile file = FileSystem.Instance.Create(filePath.LocalPath);
            HEMaterial material = data.GetWriteMaterial(Path.GetFileNameWithoutExtension(file.Name));
            material.Write(file);
            material.WriteDependencies(file.Parent);
        }
    }
}
