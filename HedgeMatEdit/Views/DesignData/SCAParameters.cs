using HedgeDev.Editor.Material.ViewModels.Resource.SampleChunk;
using SharpNeedle.Framework.HedgehogEngine.Mirage;

namespace HedgeDev.Editor.Material.Views.DesignData
{
    internal class SCAParameters : SCAParametersViewModel
    {
        private static readonly SampleChunkNode _data;

        static SCAParameters()
        {
            _data = new("SCAParam");
            _data.AddChild(new("Param1", 1u));
            _data.AddChild(new("Param2", 32));
            _data.AddChild(new("Param3", 23.5f));
            _data.AddChild(new("Param4", true));
        }

        public SCAParameters() : base(_data) { }
    }
}
