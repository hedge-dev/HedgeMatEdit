using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using HedgeDev.Editor.Material.Config;
using PropertyChanged;

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

    }

}
