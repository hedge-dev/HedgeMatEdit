using SharpNeedle.Framework.HedgehogEngine.Mirage.MaterialData;
using System.Collections.Generic;

namespace HedgeDev.Editor.Material.ViewModels.Resource.Parameter
{
    internal class BoolParametersViewModel : ParametersViewModel<bool>
    {
        public BoolParametersViewModel(Dictionary<string, MaterialParameter<bool>> data) : base(data) { }

        protected override ParameterViewModel<bool> CreateViewmodel(MaterialParameter<bool> data, string name)
        {
            return new BoolParameterViewModel(data, this, name);
        }
    }
}
