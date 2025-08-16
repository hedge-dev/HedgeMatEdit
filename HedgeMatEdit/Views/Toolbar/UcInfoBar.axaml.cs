using Avalonia.Controls;
using PropertyChanged;
using System.Reflection;

namespace HedgeDev.Editor.Material.Views.Toolbar
{
    [DoNotNotify]
    public partial class UcInfoBar : UserControl
    {
        public UcInfoBar()
        {
            InitializeComponent();

            Assembly assembly = Assembly.GetExecutingAssembly();
            VersionText.Text = assembly.GetName().Version!.ToString();
        }
    }
}
