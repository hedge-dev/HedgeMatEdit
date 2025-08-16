using SharpNeedle.Framework.HedgehogEngine.Mirage;
using System.Linq;

namespace HedgeDev.Editor.Material.ViewModels
{
    internal static class SampleChunkUtil
    {
        public static void EnsureStructure(HEMaterial material, bool addSCAParam, out SampleChunkNode? scaParamNode)
        {
            if(material.Root == null)
            {
                material.SetupNodes();
            }

            material.Root!.Value = SampleChunkNode.RootSignature;

            SampleChunkNode? materialNode = material.Root.Children.FirstOrDefault(x => x.Name == "Material");
            if(materialNode == null)
            {
                materialNode = new("Material");
                material.Root.InsertChild(0, materialNode);
            }

            materialNode.Value = 1;

            scaParamNode = materialNode.Children.FirstOrDefault(x => x.Name == "SCAParam");
            if(addSCAParam && scaParamNode == null)
            {
                scaParamNode = new("SCAParam", 1);
                materialNode.InsertChild(0, scaParamNode);
            }
            else if(!addSCAParam && scaParamNode != null && scaParamNode.Children.Count == 0)
            {
                scaParamNode.Detach();
                scaParamNode = null;
            }

            if(scaParamNode != null)
            {
                scaParamNode.Value = 1;
            }

            SampleChunkNode? contextsNode = materialNode.Children.FirstOrDefault(x => x.Name == "Contexts");
            if(contextsNode == null)
            {
                contextsNode = new("Contexts");
                materialNode.AddChild(contextsNode);
            }

            contextsNode.Data = material;
            contextsNode.Value = material.DataVersion;
        }
    }
}
