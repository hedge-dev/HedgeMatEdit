using System.ComponentModel;

namespace HedgeDev.Editor.Material.ViewModels.Resource.Texture
{
    internal enum VMWrapMode : int
    {
        Repeat,

        Mirror,

        Clamp,

        [Description("Mirror once")]
        MirrorOnce,

        Border
    }
}
