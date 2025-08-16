using HedgeDev.Editor.Material.ViewModels.Parameter;
using SharpNeedle.Framework.HedgehogEngine.Mirage.MaterialData;
using System.Collections.Generic;

namespace HedgeDev.Editor.Material.Views.DesignData
{
    internal class BoolParameters : BoolParametersViewModel
    {
        private static readonly Dictionary<string, MaterialParameter<bool>> _designData = new() {
            { "PBRFactor", new() { Value = false } },
            { "diffuse", new() { Value = true } },
            { "some_other_parameter", new() { Value = false } }
        };

        public BoolParameters() : base(new(_designData)) { }
    }
}
