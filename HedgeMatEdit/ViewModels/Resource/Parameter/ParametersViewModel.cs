using HedgeDev.Editor.Material.ViewModels.Base;
using J113D.UndoRedo.Collections;
using SharpNeedle.Framework.HedgehogEngine.Mirage.MaterialData;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using static J113D.UndoRedo.GlobalChangeTracker;

namespace HedgeDev.Editor.Material.ViewModels.Resource.Parameter
{

    internal abstract class ParametersViewModel<V> : ViewModelBase where V : unmanaged
    {
        private readonly TrackDictionary<string, MaterialParameter<V>> _data;
        private readonly TrackList<ParameterViewModel<V>> _parameters;

        public ReadOnlyObservableCollection<ParameterViewModel<V>> Parameters { get; }

        public RelayCommand CmdAddNewParameter { get; }

        public ParametersViewModel(Dictionary<string, MaterialParameter<V>> data)
        {
            _data = new(data);
            ObservableCollection<ParameterViewModel<V>> parameters = new(
                _data.Select(x => CreateViewmodel(x.Value, x.Key)));

            _parameters = new(parameters);
            Parameters = new(parameters);

            CmdAddNewParameter = new(AddNewParameter);
        }


        public bool ContainsKey(string name)
        {
            return _data.ContainsKey(name);
        }

        public void MoveParameter(string oldName, string newName)
        {
            BeginChangeGroup("MaterialParametersViewModel.MoveParameter");

            MaterialParameter<V> parameter = _data[oldName];

            _data.Remove(oldName);
            _data.Add(newName, parameter);

            EndChangeGroup();
        }

        private void AddParameter(string name, MaterialParameter<V> parameter)
        {
            BeginChangeGroup("MaterialParametersViewModel.AddParameter");

            string key = J113D.Common.GenericHelper.FindNextFreeKey(_data, name);
            ParameterViewModel<V> parameterViewModel = CreateViewmodel(parameter, key);

            _data.Add(key, parameter);
            _parameters.Add(parameterViewModel);

            EndChangeGroup();
        }

        public void AddNewParameter()
        {
            AddParameter("NewParam", new());
        }

        public void RemoveParameter(ParameterViewModel<V> parameter)
        {
            BeginChangeGroup("MaterialParametersViewModel.RemoveParameter");

            _data.Remove(parameter.Name);
            _parameters.Remove(parameter);

            EndChangeGroup();
        }

        public void FromJsonImport(Dictionary<string, MaterialParameter<V>> parameters)
        {
            BeginChangeGroup("ParametersViewModel<V>.FromJsonImport");

            foreach (ParameterViewModel<V> parameter in Parameters.ToArray())
            {
                RemoveParameter(parameter);
            }

            foreach (KeyValuePair<string, MaterialParameter<V>> parameter in parameters)
            {
                AddParameter(parameter.Key, parameter.Value);
            }

            EndChangeGroup();
        }

        protected abstract ParameterViewModel<V> CreateViewmodel(MaterialParameter<V> data, string name);
    }
}
