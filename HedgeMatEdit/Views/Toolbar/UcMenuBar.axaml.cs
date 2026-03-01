using Avalonia.Controls;
using Avalonia.Interactivity;
using HedgeDev.Editor.Material.ViewModels;
using HedgeDev.Editor.Material.ViewModels.Resource;
using HedgeDev.Editor.Material.Views.Windows;
using J113D.Avalonia.Utilities.MessageBox;
using J113D.Avalonia.Utilities.IO;
using PropertyChanged;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;

namespace HedgeDev.Editor.Material.Views.Toolbar
{
    [DoNotNotify]
    internal partial class UcMenuBar : UserControl
    {
        private readonly MaterialMultiFileHandler _materialFileHander;
        private readonly JsonFileHandler _jsonFileHandler;

        private MainViewModel ViewModel
            => (MainViewModel)DataContext!;

        public bool HasUnsavedChanges => _materialFileHander.HasUnsavedChanges;

        public UcMenuBar()
        {
            _materialFileHander = new(this);
            _jsonFileHandler = new(this, _materialFileHander);
            _materialFileHander.DataUriUpdated += OnDataUriUpdated;
            InitializeComponent();
        }

        private void OnDataUriUpdated(BaseMultiFileHandler<MaterialViewModel> filehandler, MaterialViewModel data, Uri? uri)
        {
            if (uri == null)
            {
                data.Name = "Unnamed";
            }
            else
            {
                data.Name = Path.GetFileNameWithoutExtension(uri.LocalPath);
            }
        }

        public Task<bool> CloseConfirmation()
        {
            return _materialFileHander.CloseConfirmation();
        }


        public async void OnNewMaterial(object sender, RoutedEventArgs e)
        {
            _materialFileHander.New();
            this.SetMessage("Created new material");
        }

        public async void OnOpenMaterial(object sender, RoutedEventArgs e)
        {
            if (await _materialFileHander.Open() != null)
            {
                this.SetMessage("File(s) opened");
            }
        }

        public async void OnSaveMaterial(object sender, RoutedEventArgs e)
        {
            await SaveMaterial(false);
        }

        public async void OnSaveMaterialAs(object sender, RoutedEventArgs e)
        {
            await SaveMaterial(true);
        }

        public async void OnSaveAllMaterials(object sender, RoutedEventArgs e)
        {
            if (await _materialFileHander.SaveAll())
            {
                this.SetMessage("Files saved");
            }
        }

        private async Task SaveMaterial(bool newPath)
        {
            if (ViewModel.ActiveMaterial != null && await _materialFileHander.Save(ViewModel.ActiveMaterial, newPath))
            {
                this.SetMessage("File saved");
            }
        }

        public async void OnCloseMaterial(object sender, RoutedEventArgs e)
        {
            if (ViewModel.ActiveMaterial != null && await _materialFileHander.Close(ViewModel.ActiveMaterial))
            {
                ViewModel.RemoveActiveMaterial();
                this.SetMessage("File closed");
            }
        }

        public async void OnCloseAllMaterials(object sender, RoutedEventArgs e)
        {
            if (await _materialFileHander.CloseAll())
            {
                ViewModel.ClearMaterials();
                this.SetMessage("Files closed");
            }
        }


        public async void OnExportJson(object sender, RoutedEventArgs e)
        {
            if (await _jsonFileHandler.Save(true))
            {
                this.SetMessage("Exported Material to Json file");
            }
        }

        public void OnExportJsonToClipboard(object sender, RoutedEventArgs e)
        {
            if (ViewModel.ActiveMaterial == null)
            {
                this.SetMessage("No active material!", false);
                return;
            }

            Uri? filepath = _materialFileHander.GetFilepath(ViewModel.ActiveMaterial);

            string jsonFilename = filepath != null
                ? Path.GetFileName(filepath.LocalPath)
                : "Material";

            string json = ViewModel.ExportJson(jsonFilename);
            TextCopy.ClipboardService.SetText(json);

            this.SetMessage("Material exported to json and copied to clipboard", false);
        }

        public async void OnImportJson(object sender, RoutedEventArgs e)
        {
            if (await _jsonFileHandler.Open())
            {
                this.SetMessage("Imported Material from Json file");
            }
        }

        public async void OnImportJsonFromClipboard(object sender, RoutedEventArgs e)
        {
            string clipboard = TextCopy.ClipboardService.GetText() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(clipboard))
            {
                this.SetMessage("Clipboard is empty", true);
                return;
            }

            try
            {
                if (ViewModel.ActiveMaterial == null)
                {
                    ViewModel.ImportJson(clipboard);
                }
                else
                {
                    ViewModel.ActiveMaterial.ImportJson(clipboard);
                }
            }
            catch (Exception exception)
            {
                Window window = (Window)TopLevel.GetTopLevel(this)!;

                await window.MessageBoxDialog(
                    "Clipboard invalid!",
                    $"Failed to import Material from Json in clipboard:\n{exception.Message}",
                    MessageBoxButtons.Ok,
                    MessageBoxIcon.Error);

                return;
            }

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


        public async Task OnDropFiles(IEnumerable<Uri> filepaths)
        {
            int successCount = 0;
            int invalidCount = 0;
            List<Exception> fails = [];

            foreach (Uri filepath in filepaths)
            {
                string extension = Path.GetExtension(filepath.LocalPath).ToLower();

                try
                {
                    if (extension == ".material")
                    {
                        await _materialFileHander.OpenNoDialog([filepath]);
                        successCount++;
                    }
                    else if (extension == ".json")
                    {
                        await _jsonFileHandler.OpenNoDialog(filepath);
                        successCount++;
                    }
                    else
                    {
                        invalidCount++;
                    }
                }
                catch (Exception exception)
                {
                    fails.Add(exception);
                }
            }

            int totalCount = successCount + invalidCount + fails.Count;

            if(invalidCount > 0)
            {
                WndMain window = (WndMain)TopLevel.GetTopLevel(this)!;

                if(totalCount == 1)
                {
                    await window.MessageBoxDialog("Invalid file", $"The file has an invalid format. Please drop a .material or a .json file.", MessageBoxButtons.Ok, MessageBoxIcon.Error);
                }
                else
                {
                    await window.MessageBoxDialog("Invalid file(s)", $"{invalidCount} file(s) have an invalid format. Please drop a .material or a .json file.", MessageBoxButtons.Ok, MessageBoxIcon.Error);
                }
            }

            if(successCount > 0)
            {
                if(totalCount == 1)
                {
                    this.SetMessage($"Opened file");
                }
                else
                {
                    this.SetMessage($"Opened {successCount} files");
                }
            }
            else
            {
                this.ClearMessage();
            }
        }
    }
}
