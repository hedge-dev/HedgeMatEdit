using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using HedgeDev.Editor.Material.ViewModels;
using HedgeDev.Editor.Material.Views.Windows;
using J113D.Avalonia.MessageBox;
using J113D.Avalonia.Utilities.IO;
using J113D.UndoRedo;
using PropertyChanged;
using System;
using System.IO;
using System.Threading.Tasks;

namespace HedgeDev.Editor.Material.Views.Toolbar
{
    [DoNotNotify]
    internal partial class UcMenuBar : UserControl, IFileChangeTracker
    {
        private ChangeTracker.Pin? _fileChangePin;
        private readonly MaterialFileHandler _materialFileHandler;
        private readonly JsonExportHandler _jsonExportHandler;
        private readonly JsonImportHandler _jsonImportHandler;

        private MainViewModel ViewModel
            => (MainViewModel)DataContext!;

        public bool HasFileChanged => _fileChangePin != null ? !_fileChangePin.Value.IsValid : App.EditorChangeTracker.CanUndo;

        public static readonly DirectProperty<UcMenuBar, string?> DisplayFilenameProperty =
            AvaloniaProperty.RegisterDirect<UcMenuBar, string?>(nameof(DisplayFilename), o => o.DisplayFilename);

        private string DisplayFilename => _materialFileHandler?.LoadedFilePath != null
            ? Path.GetFileName(_materialFileHandler.LoadedFilePath.LocalPath)
            : string.Empty;

        public UcMenuBar()
        {
            InitializeComponent();
            _materialFileHandler = new(this, this);
            _jsonExportHandler = new(this);
            _jsonImportHandler = new(this, this);
        }


        public void StoreCurrentState(bool clearHistory)
        {
            if(clearHistory)
            {
                App.EditorChangeTracker.Reset();
                _fileChangePin = null;
            }
            else
            {
                _fileChangePin = App.EditorChangeTracker.PinCurrent();
            }
        }


        public Task<bool> CloseConfirmation()
        {
            return _materialFileHandler.ResetConfirmation();
        }

        private void UpdateFilename()
        {
            RaisePropertyChanged(DisplayFilenameProperty, ".", DisplayFilename);
        }

        public async void OnNewMaterial(object sender, RoutedEventArgs e)
        {
            if(await _materialFileHandler.Reset())
            {
                this.SetMessage("Created new material");
                UpdateFilename();
            }
        }

        public async void OnOpenMaterial(object sender, RoutedEventArgs e)
        {
            if(await _materialFileHandler.Open())
            {
                this.SetMessage("File opened");
                UpdateFilename();
            }
        }

        public async void OnSaveMaterial(object sender, RoutedEventArgs e)
        {
            if(await _materialFileHandler.Save(false))
            {
                UpdateFilename();
                this.SetMessage("File saved");
            }
        }

        public async void OnSaveMaterialAs(object sender, RoutedEventArgs e)
        {
            if(await _materialFileHandler.Save(true))
            {
                UpdateFilename();
                this.SetMessage("File saved");
            }
        }


        public async void OnExportJson(object sender, RoutedEventArgs e)
        {
            if(await _jsonExportHandler.Save(true))
            {
                this.SetMessage("Exported Material to Json file");
            }
        }

        public void OnExportJsonToClipboard(object sender, RoutedEventArgs e)
        {
            string jsonFilename = _materialFileHandler.LoadedFilePath != null
                ? Path.GetFileName(_materialFileHandler.LoadedFilePath.LocalPath)
                : "Material";

            string json = ViewModel.ExportJson(jsonFilename);
            TextCopy.ClipboardService.SetText(json);

            this.SetMessage("Material exported to json and copied to clipboard", false);
        }

        public async void OnImportJson(object sender, RoutedEventArgs e)
        {
            if(await _jsonImportHandler.Open())
            {
                _materialFileHandler.ForgetFilePath();
                UpdateFilename();
                this.SetMessage("Imported Material from Json file");
            }
        }

        public async void OnImportJsonFromClipboard(object sender, RoutedEventArgs e)
        {
            string clipboard = TextCopy.ClipboardService.GetText() ?? string.Empty;
            if(string.IsNullOrWhiteSpace(clipboard))
            {
                this.SetMessage("Clipboard is empty", true);
                return;
            }

            if(!await _materialFileHandler.ResetConfirmation())
            {
                return;
            }

            try
            {
                ViewModel.ImportJson(clipboard);
            }
            catch(Exception exception)
            {
                Window window = (Window)TopLevel.GetTopLevel(this)!;

                await window.MessageBoxDialog(
                    "Clipboard invalid!",
                    $"Failed to import Material from Json in clipboard:\n{exception.Message}",
                    MessageBoxButtons.Ok,
                    MessageBoxIcon.Error);

                return;
            }

            _materialFileHandler.ForgetFilePath();
            UpdateFilename();
            this.SetMessage("Imported Material from Json in clipboard");

        }


        private void OnUndo(object? sender, RoutedEventArgs e)
        {
            this.Undo();
        }

        private void OnRedo(object? sender, RoutedEventArgs e)
        {
            this.Redo();
        }


        private async void OnSettingsOpen(object sender, RoutedEventArgs e)
        {
            Window topLevel = (Window)TopLevel.GetTopLevel(this)!;
            WndSettings window = new();
            await window.ShowDialog(topLevel);
        }


        public async Task OnDropFile(Uri filepath)
        {
            string extension = Path.GetExtension(filepath.LocalPath).ToLower();

            if(extension == ".material")
            {
                if(await _materialFileHandler.ResetConfirmation()
                    && await _materialFileHandler.OpenNoDialog(filepath))
                {
                    UpdateFilename();
                    this.SetMessage("File Opened");
                }
            }
            else if(extension == ".json")
            {
                if(await _jsonImportHandler.ResetConfirmation()
                    && await _jsonImportHandler.OpenNoDialog(filepath))
                {
                    _materialFileHandler.ForgetFilePath();
                    UpdateFilename();
                    this.SetMessage("Imported Material from Json");
                }
            }
            else
            {
                WndMain window = (WndMain)TopLevel.GetTopLevel(this)!;
                await window.MessageBoxDialog("Invalid file", "The dropped file has an invalid format. Please drop a .material or a .json file.", MessageBoxButtons.Ok, MessageBoxIcon.Error);
            }
        }
    }
}
