using Avalonia;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using HedgeDev.Editor.Material.ViewModels;
using J113D.Avalonia.Utilities.IO;
using SharpNeedle.IO;
using System;
using System.Collections.Generic;
using System.IO;

namespace HedgeDev.Editor.Material.Views.Toolbar
{
    internal class JsonFileHandler : BaseSingleFileHandler
    {
        private readonly Visual _visual;
        private readonly MaterialMultiFileHandler _materialFileHandler;

        protected override string FileTypeName
            => "Json";

        public override IReadOnlyList<FilePickerFileType>? FileType { get; } =
        [
            new("Json") {
                Patterns = ["*.json"]
            }
        ];

        protected override IDataChangeState? DataChangeState => null;



        protected MainViewModel ViewModel
            => (MainViewModel)_visual.DataContext!;

        protected override Window Window
            => _visual as Window ?? (Window)TopLevel.GetTopLevel(_visual)!;


        public JsonFileHandler(Visual visual, MaterialMultiFileHandler materialFileHandler)
        {
            _visual = visual;
            _materialFileHandler = materialFileHandler;
        }


        protected override void InternalReset()
        {
            throw new NotSupportedException();
        }

        protected override void InternalLoad(Uri filePath)
        {
            IFile file = FileSystem.Instance.Open(filePath.LocalPath)!;
            string json;
            using (Stream stream = file.Open(FileAccess.Read))
            {
                StreamReader reader = new(stream);
                json = reader.ReadToEnd();
            }

            ViewModel.ImportJson(json);
            _materialFileHandler.Add(ViewModel.ActiveMaterial!, null);
        }

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
