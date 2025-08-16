using J113D.UndoRedo.Collections;
using SharpNeedle.Framework.HedgehogEngine.Mirage;
using System.Collections.ObjectModel;
using System.Linq;
using static J113D.UndoRedo.GlobalChangeTracker;

namespace HedgeDev.Editor.Material.ViewModels
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

        public void AddNewParameter()
        {
            BeginChangeGroup("SCAParametersViewModel.AddNewParameter");

            SampleChunkNode node = new("SCAParam");
            SCAParameterViewModel viewmodel = new(node, this);

            TrackCallbackChange(
                () => _data.AddChild(node),
                node.Detach
            );

            _parameters.Add(viewmodel);

            EndChangeGroup();
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
    }
}
