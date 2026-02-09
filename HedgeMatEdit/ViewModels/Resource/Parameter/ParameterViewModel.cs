using HedgeDev.Editor.Material.ViewModels.Base;
using SharpNeedle.Framework.HedgehogEngine.Mirage.MaterialData;
using System;
using static J113D.UndoRedo.GlobalChangeTracker;

namespace HedgeDev.Editor.Material.ViewModels.Resource.Parameter
{
    internal abstract class ParameterViewModel<T> : ViewModelBase where T : unmanaged
    {
        protected readonly MaterialParameter<T> _data;
        private readonly ParametersViewModel<T> _parent;

        private string _name;

        public RelayCommand CmdRemove { get; }

        public string Name
        {
            get => _name;
            set
            {
                if(_name == value)
                {
                    return;
                }

                if(_parent.ContainsKey(value))
                {
                    throw new ArgumentException("Parameter with that name already exists!");
                }

                string oldValue = _name;

                BeginChangeGroup("MaterialParameterViewModel.Name");
                TrackFieldChange(this, nameof(_name), value);
                this.AddChangeGroupInvokePropertyChanged(nameof(Name));
                _parent.MoveParameter(oldValue, _name);
                EndChangeGroup();
            }
        }


        public ParameterViewModel(MaterialParameter<T> data, ParametersViewModel<T> parent, string name)
        {
            _data = data;
            _parent = parent;
            _name = name;
            CmdRemove = new(Remove);
        }


        public void Remove()
        {
            _parent.RemoveParameter(this);
        }
    }
}
