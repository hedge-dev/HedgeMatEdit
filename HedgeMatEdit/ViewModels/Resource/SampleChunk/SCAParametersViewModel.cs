using HedgeDev.Editor.Material.ViewModels.Base;
using J113D.UndoRedo.Collections;
using SharpNeedle.Framework.HedgehogEngine.Mirage;
using System.Collections.ObjectModel;
using System.Linq;
using static J113D.UndoRedo.GlobalChangeTracker;

namespace HedgeDev.Editor.Material.ViewModels.Resource.SampleChunk
{
    internal class SCAParametersViewModel : ViewModelBase
    {
        private SampleChunkNode _data;
        private TrackList<SCAParameterViewModel> _parameters;
        
        public ReadOnlyObservableCollection<SCAParameterViewModel> Parameters { get; }

        public SCAParametersViewModel(SampleChunkNode data)
        {
            _data = data;

            ObservableCollection<SCAParameterViewModel> parameters = new(
                data.Children.Select(x => new SCAParameterViewModel(x, this)));

            _parameters = new(parameters);
            Parameters = new(parameters);
        }

        private void AddParameter(SampleChunkNode node)
        {
            BeginChangeGroup("SCAParametersViewModel.AddParameter");

            SCAParameterViewModel viewmodel = new(node, this);

            TrackCallbackChange(
                () => _data.AddChild(node),
                node.Detach
            );

            _parameters.Add(viewmodel);

            EndChangeGroup();
        }

        public void AddNewParameter()
        {
            AddParameter(new("SCAParam"));
        }

        public void RemoveParameter(SCAParameterViewModel parameter)
        {
            BeginChangeGroup("SCAParametersViewModel.RemoveParameter");

            int index = _data.Children.IndexOf(parameter._data);

            TrackCallbackChange(
                parameter._data.Detach,
                () => _data.InsertChild(index, parameter._data)
            );

            _parameters.Remove(parameter);

            EndChangeGroup();
        }

        public void FromJsonImport(SampleChunkNode data)
        {
            BeginChangeGroup("SCAParametersViewModel.FromJsonImport");

            foreach (SCAParameterViewModel parameter in Parameters.ToArray())
            {
                RemoveParameter(parameter);
            }

            foreach (SampleChunkNode parameter in data.Children.ToArray())
            {
                parameter.Detach();
                AddParameter(parameter);
            }

            EndChangeGroup();
        }
    }
}
