using SharpNeedle.Framework.HedgehogEngine.Mirage.MaterialData;
using SharpNeedle.Structs;
using System.Collections.Generic;

namespace HedgeDev.Editor.Material.ViewModels.Parameter
{
    internal class IntParametersViewModel : ParametersViewModel<Vector4Int>
    {
        public IntParametersViewModel(Dictionary<string, MaterialParameter<Vector4Int>> data) : base(data) { }

        protected override ParameterViewModel<Vector4Int> CreateViewmodel(MaterialParameter<Vector4Int> data, string name)
        {
            return new IntParameterViewModel(data, this, name);
        }
    }
}
