using SharpNeedle.Framework.HedgehogEngine.Mirage.MaterialData;
using System.Collections.Generic;
using System.Numerics;

namespace HedgeDev.Editor.Material.ViewModels.Resource.Parameter
{
    internal class FloatParametersViewModel : ParametersViewModel<Vector4>
    {
        public FloatParametersViewModel(Dictionary<string, MaterialParameter<Vector4>> data) 
            : base(data) { }

        protected override ParameterViewModel<Vector4> CreateViewmodel(MaterialParameter<Vector4> data, string name)
        {
            return new FloatParameterViewModel(data, this, name);
        }
    }
}
