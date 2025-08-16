using Avalonia;
using Avalonia.Interactivity;
using PropertyChanged;
using System;


namespace HedgeDev.Editor.Material.Views.Windows
{
    [DoNotNotify]
    internal partial class WndSettings : J113D.Avalonia.Controls.Window
    {
        public WndSettings()
        {
            DataContext = ((App)Application.Current!).Settings;
            InitializeComponent();
        }

        protected override void OnClosed(EventArgs e)
        {
            ((App)Application.Current!).Settings.Save();
            base.OnClosed(e);
        }

        private void OnCloseClicked(object? sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}