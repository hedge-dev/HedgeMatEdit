using Avalonia;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using HedgeDev.Editor.Material.ViewModels;
using PropertyChanged;
using System.Collections.Generic;
using System.IO;

namespace HedgeDev.Editor.Material.Views.Editors
{
    [DoNotNotify]
    public partial class UcMatEditTextures : UserControl
    {
        public UcMatEditTextures()
        {
            InitializeComponent();
        }

        private async void OnImageSelect(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            Window window = (Window)TopLevel.GetTopLevel(this)!;

            IReadOnlyList<IStorageFile> files = await window.StorageProvider.OpenFilePickerAsync(new()
            {
                Title = $"Select DDS texture file",
                AllowMultiple = false,
                FileTypeFilter = [new("DDS Texture") { Patterns = ["*.dds"] }]
            });

            if(files == null || files.Count == 0)
            {
                return;
            }

            ((TextureViewModel)((StyledElement)sender!).DataContext!).PictureName = Path.GetFileNameWithoutExtension(files[0].Path.LocalPath);
        }
    }
}
