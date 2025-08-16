using System.ComponentModel;

namespace HedgeDev.Editor.Material.ViewModels
{
    internal enum SCAParameterType
    {
        [Description("Unsigned Integer")]
        UnsignedInteger,
        [Description("Signed Integer")]
        SignedInteger,
        Float,
        Boolean
    }
}
