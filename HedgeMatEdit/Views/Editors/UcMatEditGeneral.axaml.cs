using Avalonia.Controls;
using HedgeDev.Editor.Material.ViewModels.Resource;
using PropertyChanged;

namespace HedgeDev.Editor.Material.Views.Editors
{
    [DoNotNotify]
    public partial class UcMatEditGeneral : UserControl
    {
        private MaterialViewModel ViewModel
                => (MaterialViewModel)DataContext!;

        public UcMatEditGeneral()
        {
            InitializeComponent();
        }

        private void SliderCaptureLost(object? sender, Avalonia.Input.PointerCaptureLostEventArgs e)
        {
            ViewModel.CommitSliderValue();
        }
    }
}
