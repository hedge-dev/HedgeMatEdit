using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Platform.Storage;
using HedgeDev.Editor.Material.Views.Windows;
using J113D.Avalonia.MessageBox;
using PropertyChanged;

namespace HedgeDev.Editor.Material.Views.Editors
{

    [DoNotNotify]
    internal partial class UcMaterialsEditor : UserControl
    {
        public UcMaterialsEditor()
        {
            InitializeComponent();
        }

        private void OnDragEnter(object? sender, DragEventArgs e)
        {
            DropOverlay.IsVisible = true;
        }

        private void OnDragLeave(object? sender, DragEventArgs e)
        {
            DropOverlay.IsVisible = false;
        }

        private async void OnDrop(object? sender, DragEventArgs e)
        {
            DropOverlay.IsVisible = false;

            IStorageItem[]? files = e.DataTransfer.TryGetFiles();
            if (files == null || files.Length == 0)
            {
                return;
            }

            WndMain window = (WndMain)TopLevel.GetTopLevel(this)!;

            if (files.Length > 1)
            {
                await window.MessageBoxDialog("Invalid action", "Dropped too many files, please drop only 1 file", MessageBoxButtons.Ok, MessageBoxIcon.Info);
                return;
            }

            await window.MenuBar.OnDropFile(files[0].Path);


        }
    }

}