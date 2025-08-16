using HedgeDev.Editor.Material.ViewModels;
using SharpNeedle.Framework.HedgehogEngine.Mirage.MaterialData;

namespace HedgeDev.Editor.Material.Views.DesignData
{
    internal class TextureSet : TextureSetViewModel
    {
        private static readonly Texset _data = new()
        {
            Textures = [
                new()
                {
                    Type = "diffuse",
                    PictureName = "sonic_alb",
                    TexCoordIndex = 0,
                    WrapModeU = SharpNeedle.Framework.HedgehogEngine.Mirage.MaterialData.WrapMode.Repeat,
                    WrapModeV = SharpNeedle.Framework.HedgehogEngine.Mirage.MaterialData.WrapMode.Mirror
                },
                new()
                {
                    Type = "specular",
                    PictureName = "sonic_prm",
                    TexCoordIndex = 1,
                    WrapModeU = SharpNeedle.Framework.HedgehogEngine.Mirage.MaterialData.WrapMode.Clamp,
                    WrapModeV = SharpNeedle.Framework.HedgehogEngine.Mirage.MaterialData.WrapMode.MirrorOnce
                },
                new()
                {
                    Type = "normal",
                    PictureName = "sonic_nrm",
                    TexCoordIndex = 2,
                    WrapModeU = SharpNeedle.Framework.HedgehogEngine.Mirage.MaterialData.WrapMode.Border,
                    WrapModeV = SharpNeedle.Framework.HedgehogEngine.Mirage.MaterialData.WrapMode.Repeat
                }
            ]
        };

        public TextureSet() : base(_data) { }
    }
}
