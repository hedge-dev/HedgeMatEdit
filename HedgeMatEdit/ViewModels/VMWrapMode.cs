using System.ComponentModel;

namespace HedgeDev.Editor.Material.ViewModels
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
