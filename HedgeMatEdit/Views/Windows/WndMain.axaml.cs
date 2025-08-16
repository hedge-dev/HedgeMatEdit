using Avalonia;
using Avalonia.Controls;
using J113D.Avalonia.MessageBox;
using PropertyChanged;
using System;

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

        public WndMain()
        {
            InitializeComponent();
        }

        public void SetMessage(string message, bool warning)
        {
            if(_messageType != MessageType.None)
            {
                SetAndRaise(MessageProperty, ref _message, null);
                SetAndRaise(MessageTypeProperty, ref _messageType, MessageType.None);
            }

            SetAndRaise(MessageProperty, ref _message, message);
            SetAndRaise(MessageTypeProperty, ref _messageType, warning ? MessageType.Error : MessageType.Success);
        }

        protected override async void OnClosing(WindowClosingEventArgs e)
        {
            if(!_ignoreResetConfirmation && MenuBar.HasFileChanged)
            {
                e.Cancel = true;
                if(await MenuBar.CloseConfirmation())
                {
                    _ignoreResetConfirmation = true;
                    Close();
                }
            }

            base.OnClosing(e);
        }

        private async void OnLoaded(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if(string.IsNullOrWhiteSpace(InitialFilePath))
            {
                return;
            }

            Uri uri = new(InitialFilePath, UriKind.RelativeOrAbsolute);
            try
            {
                await MenuBar.OnDropFile(uri);
            }
            catch(Exception exc)
            {
                await this.MessageBoxDialog("Filepath invalid", $"The file \"{InitialFilePath}\" failed to load:\n{exc.Message}", MessageBoxButtons.Ok, MessageBoxIcon.Error);
                return;
            }
        }
    }
}