using Avalonia;
using Avalonia.Controls;
using HedgeDev.Editor.Material.ViewModels;
using J113D.Avalonia.Utilities.IO;
using System;

namespace HedgeDev.Editor.Material.Views
{
    internal abstract class BaseFileHandler : J113D.Avalonia.Utilities.IO.BaseFileHandler
    {
        private readonly Visual _visual;

        protected MainViewModel ViewModel
            => (MainViewModel)_visual.DataContext!;

        protected override Window Window
            => _visual as Window ?? (Window)TopLevel.GetTopLevel(_visual)!;

        protected override IFileChangeTracker? FileChangeTracker { get; }


        public BaseFileHandler(Visual visual, IFileChangeTracker? fileChangeTracker)
        {
            _visual = visual;
            FileChangeTracker = fileChangeTracker;
        }


        protected override void InternalReset()
        {
            throw new NotSupportedException();
        }

        protected override void InternalSave(Uri filePath)
        {
            throw new NotSupportedException();
        }

        protected override void InternalLoad(Uri filePath)
        {
            throw new NotSupportedException();
        }
    }
}
