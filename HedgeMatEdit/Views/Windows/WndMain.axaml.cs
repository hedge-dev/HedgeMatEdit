using Avalonia;
using Avalonia.Controls;
using HedgeDev.Editor.Material.ViewModels;
using J113D.Avalonia.MessageBox;
using Octokit;
using PropertyChanged;
using System;
using System.Diagnostics;
using System.Reflection;

namespace HedgeDev.Editor.Material.Views.Windows
{
    [DoNotNotify]
    internal partial class WndMain : Window
    {
        private bool _ignoreResetConfirmation;
        private string? _message;
        private MessageType _messageType;

        public string? InitialFilePath { get; init; }

        public static readonly DirectProperty<WndMain, string?> MessageProperty =
            AvaloniaProperty.RegisterDirect<WndMain, string?>(nameof(MessageProperty), o => o._message);

        public static readonly DirectProperty<WndMain, MessageType> MessageTypeProperty =
            AvaloniaProperty.RegisterDirect<WndMain, MessageType>(nameof(MessageProperty), o => o._messageType);

        public MainViewModel ViewModel => (MainViewModel)DataContext!;


        public WndMain(MainViewModel viewmodel)
        {
            DataContext = viewmodel;
            InitializeComponent();
        }

        public WndMain() : this(new MainViewModel(new())) { }

        public void SetMessage(string message, bool warning)
        {
            if (_messageType != MessageType.None)
            {
                SetAndRaise(MessageProperty, ref _message, null);
                SetAndRaise(MessageTypeProperty, ref _messageType, MessageType.None);
            }

            SetAndRaise(MessageProperty, ref _message, message);
            SetAndRaise(MessageTypeProperty, ref _messageType, warning ? MessageType.Error : MessageType.Success);
        }

        protected override async void OnClosing(WindowClosingEventArgs e)
        {
            if (!_ignoreResetConfirmation && MenuBar.HasUnsavedChanges)
            {
                e.Cancel = true;
                if (await MenuBar.CloseConfirmation())
                {
                    _ignoreResetConfirmation = true;
                    Close();
                }
            }

            base.OnClosing(e);
        }

        private async void OnLoaded(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(InitialFilePath))
            {
                Uri uri = new(InitialFilePath, UriKind.RelativeOrAbsolute);
                try
                {
                    await MenuBar.OnDropFile(uri);
                }
                catch (Exception exc)
                {
                    await this.MessageBoxDialog("Filepath invalid", $"The file \"{InitialFilePath}\" failed to load:\n{exc.Message}", MessageBoxButtons.Ok, MessageBoxIcon.Error);
                }
            }

            try
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                AssemblyName assemblyName = assembly.GetName();
                string name = assemblyName.Name!;
                Version version = assemblyName.Version!;

                GitHubClient client = new(new ProductHeaderValue(name, version.ToString()));
                Release release = await client.Repository.Release.GetLatest("hedge-dev", "HedgeMatEdit");

                Version latestVersion = new(release.TagName);

                if (version < latestVersion)
                {
                    MessageBoxResult? result = await this.MessageBoxDialog("New Release", $"Version {release.TagName} is available for download!\nWould you like to visit the download page?", MessageBoxButtons.YesNo, MessageBoxIcon.Info);

                    if (result == MessageBoxResult.Yes)
                    {
                        Process.Start("explorer", release.HtmlUrl);
                    }
                }
            }
            catch { }
        }
    }
}