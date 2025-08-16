using Avalonia.Platform.Storage;
using Avalonia;
using SharpNeedle.IO;
using System.Collections.Generic;
using System;
using System.IO;

namespace HedgeDev.Editor.Material.Views.Toolbar
{
    internal class JsonExportHandler : BaseFileHandler
    {
        protected override string FileTypeName
            => "Json";

        public override IReadOnlyList<FilePickerFileType>? FileType { get; } =
        [
            new("Json") {
                Patterns = ["*.json"]
            }
        ];


        public JsonExportHandler(Visual visual) : base(visual, null) { }

        protected override void InternalSave(Uri filePath)
        {
            string json = ViewModel.ExportJson(Path.GetFileName(filePath.LocalPath));

            IFile file = FileSystem.Instance.Create(filePath.LocalPath);
            using(Stream stream = file.Open(FileAccess.Write))
            {
                StreamWriter writer = new(stream);
                writer.Write(json);
                writer.Flush();
            }
        }
    }
}
