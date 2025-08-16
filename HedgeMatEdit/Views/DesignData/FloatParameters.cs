using HedgeDev.Editor.Material.ViewModels.Parameter;
using SharpNeedle.Framework.HedgehogEngine.Mirage.MaterialData;
using System.Collections.Generic;
using System.Numerics;

namespace HedgeDev.Editor.Material.Views.DesignData
{
    internal class FloatParameters : FloatParametersViewModel
    {
        private static readonly Dictionary<string, MaterialParameter<Vector4>> _designData = new() {
            { "PBRFactor", new() { Value = new(0.5f, 0.25f, 1.0f, 10.0f) } },
            { "diffuse", new() { Value = new(0.5f, 0.25f, 1.0f, 10.0f) } },
            { "some_other_parameter", new() { Value = new(0.5f, 0.25f, 1.0f, 10.0f) } }
        };

        public FloatParameters() : base(new(_designData)) { }
    }
}
