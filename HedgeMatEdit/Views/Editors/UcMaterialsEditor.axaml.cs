using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using HedgeDev.Editor.Material.ViewModels.Base;
using HedgeDev.Editor.Material.Views.Windows;
using J113D.Avalonia.Utilities.MessageBox;
using PropertyChanged;
using System.Linq;

namespace HedgeDev.Editor.Material.Views.Editors
{

    [DoNotNotify]
    internal partial class UcMaterialsEditor : UserControl
    {
        public UcMaterialsEditor()
        {
            InitializeComponent();
        }


        protected override void OnLoaded(RoutedEventArgs e)
        {
            base.OnLoaded(e);

            ((Window)TopLevel.GetTopLevel(this)!).KeyBindings.Add(new()
            {
                Command = new RelayCommand(FocusSearch),
                Gesture = new(Key.F, KeyModifiers.Control)
            });
        }

        private void FocusSearch()
        {
            SearchTextBox.Focus();
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
            await window.MenuBar.OnDropFiles(files.Select(x => x.Path));
        }
    }

}