using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Platform.Storage;
using HedgeDev.Editor.Material.Config;
using HedgeDev.Editor.Material.Views.Windows;
using J113D.Avalonia.MessageBox;
using PropertyChanged;
using System.Collections.Generic;
using System.Linq;

namespace HedgeDev.Editor.Material.Views.Editors
{
    [DoNotNotify]
    public partial class UcMaterialEditor : UserControl
    {
        public UcMaterialEditor()
        {
            InitializeComponent();

            IntParameters.Bind(IsVisibleProperty, new Binding()
            {
                Source = ((App)Application.Current!).Settings,
                Path = nameof(SettingsViewModel.ShowIntParameters)
            });
        }

        private void OnScrollViewerSizeChanged(object? sender, SizeChangedEventArgs e)
        {
            ScrollPanel.Padding = ScrollPanel.ScrollBarMaximum.Y > 0 ? new(0, 0, 4, 0) : new(0);
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

            IStorageItem[]? files = e.Data.GetFiles()?.ToArray();
            if(files == null || files.Length == 0)
            {
                return;
            }

            WndMain window = (WndMain)TopLevel.GetTopLevel(this)!;

            if(files.Length > 1)
            {
                await window.MessageBoxDialog("Invalid action", "Dropped too many files, please drop only 1 file", MessageBoxButtons.Ok, MessageBoxIcon.Info);
                return;
            }

            await window.MenuBar.OnDropFile(files[0].Path);
            
        }
    }

}
