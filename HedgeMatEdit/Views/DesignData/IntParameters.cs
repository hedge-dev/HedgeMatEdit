using HedgeDev.Editor.Material.ViewModels.Resource.Parameter;
using SharpNeedle.Framework.HedgehogEngine.Mirage.MaterialData;
using SharpNeedle.Structs;
using System.Collections.Generic;

namespace HedgeDev.Editor.Material.Views.DesignData
{
    internal class IntParameters : IntParametersViewModel
    {
        private static readonly Dictionary<string, MaterialParameter<Vector4Int>> _designData = new() {
            { "PBRFactor", new() { Value = new(1, 2, 3, 4) } },
            { "diffuse", new() { Value = new(1, 2, 3, 4) } },
            { "some_other_parameter", new() { Value = new(1, 2, 3, 4) } }
        };

        public IntParameters() : base(new(_designData)) { }
    }
}
