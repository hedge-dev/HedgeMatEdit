using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using PropertyChanged;
using J113D.Avalonia.Theme;
using HedgeDev.Editor.Material.Config;
using HedgeDev.Editor.Material.Views.Windows;
using HedgeDev.Editor.Material.ViewModels;
using J113D.UndoRedo;
using System;

namespace HedgeDev.Editor.Material
{
    [DoNotNotify]
    internal partial class App : Application
    {
        public static ChangeTracker EditorChangeTracker => ((App)Current!).MaterialEditorTracker;

        public ChangeTracker MaterialEditorTracker { get; }

        public SettingsViewModel Settings { get; }

        public App()
        {
            MaterialEditorTracker = new();
            MaterialEditorTracker.UseTracker();

            Settings = new();
            Settings.PropertyChanged += OnSettingChanged;
        }

        private void OnSettingChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch(e.PropertyName)
            {
                case nameof(SettingsViewModel.FontSize):
                    Resources["AppFontSize"] = (double)Settings.FontSize;
                    Resources["AppFontSizeH1"] = double.Round((double)Settings.FontSize * 1.15);
                    break;
                case nameof(SettingsViewModel.Theme):
                    Settings.Theme.ApplyTheme(this);
                    break;
            }
        }

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
            Settings.Load();
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if(ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                string? filepath = null;
                if(desktop.Args?.Length > 0)
                {
                    filepath = desktop.Args[0];
                }

                desktop.MainWindow = new WndMain()
                {
                    DataContext = new MainViewModel(Settings),
                    InitialFilePath = filepath
                };

            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}