using Avalonia.Platform.Storage;
using Avalonia;
using SharpNeedle.IO;
using J113D.Avalonia.Utilities.IO;
using System.Collections.Generic;
using System;
using System.IO;

namespace HedgeDev.Editor.Material.Views.Toolbar
{
    internal class JsonImportHandler : BaseFileHandler
    {
        protected override string FileTypeName
            => "Json";

        public override IReadOnlyList<FilePickerFileType>? FileType { get; } =
        [
            new("Json") {
                Patterns = ["*.json"]
            }
        ];


        public JsonImportHandler(Visual visual, IFileChangeTracker fileChangeTracker) : base(visual, fileChangeTracker) { }


        protected override void InternalLoad(Uri filePath)
        {
            IFile file = FileSystem.Instance.Open(filePath.LocalPath)!;
            string json;
            using(Stream stream = file.Open(FileAccess.Read))
            {
                StreamReader reader = new(stream);
                json = reader.ReadToEnd();
            }

            ViewModel.ImportJson(json);
        }
    }
}
