using J113D.UndoRedo;
using System.ComponentModel;

namespace HedgeDev.Editor.Material.ViewModels
{
    internal abstract class ViewModelBase : IInvokeNotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public void InvokePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new(propertyName));
        }
    }
}
