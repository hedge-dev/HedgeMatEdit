using Avalonia.Platform.Storage;
using Avalonia;
using J113D.Avalonia.Utilities.IO;
using SharpNeedle.IO;
using System.Collections.Generic;
using System;
using System.IO;

namespace HedgeDev.Editor.Material.Views.Toolbar
{
    internal class MaterialFileHandler : BaseFileHandler
    {
        protected override string FileTypeName
            => "Hedgehog Engine Material";

        public override IReadOnlyList<FilePickerFileType>? FileType { get; } =
        [
            new("Hedgehog Engine Material") {
                Patterns = ["*.material"]
            }
        ];


        public MaterialFileHandler(Visual visual, IFileChangeTracker fileChangeTracker) : base(visual, fileChangeTracker) { }


        protected override void InternalReset()
        {
            ViewModel.NewMaterial();
        }

        protected override void InternalLoad(Uri filePath)
        {
            IFile? file = FileSystem.Instance.Open(filePath.LocalPath) 
                ?? throw new FileNotFoundException("File not found");

            ViewModel.ReadMaterial(file);
        }

        protected override void InternalSave(Uri filePath)
        {
            IFile file = FileSystem.Instance.Create(filePath.LocalPath);
            ViewModel.WriteMaterial(file);        
        }
    }
}
