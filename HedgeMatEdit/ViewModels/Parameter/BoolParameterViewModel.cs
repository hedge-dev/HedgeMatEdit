using J113D.UndoRedo;
using SharpNeedle.Framework.HedgehogEngine.Mirage.MaterialData;
using static J113D.UndoRedo.GlobalChangeTracker;

namespace HedgeDev.Editor.Material.ViewModels.Parameter
{
    internal class BoolParameterViewModel : ParameterViewModel<bool>
    {
        public bool Toggled
        {
            get => _data.Value;
            set
            {
                if(_data.Value == value)
                {
                    return;
                }

                BeginChangeGroup("BoolParameterViewModel.Toggled");
                TrackPropertyChange(_data, nameof(MaterialParameter<bool>.Value), value);
                this.AddChangeGroupInvokePropertyChanged(nameof(Toggled));
                EndChangeGroup();
            }
        }

        public BoolParameterViewModel(MaterialParameter<bool> data, ParametersViewModel<bool> parent, string name) 
            : base(data, parent, name)
        {
        }
    }
}
