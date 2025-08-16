using Avalonia;
using Avalonia.Controls;
using HedgeDev.Editor.Material.Views.Windows;

namespace HedgeDev.Editor.Material.Views
{
    internal static class WindowUtil
    {
        public static void SetMessage(this Visual visual, string message, bool warning = false)
        {
            ((WndMain)TopLevel.GetTopLevel(visual)!).SetMessage(message, warning);
        }

        public static void Undo(this Visual visual)
        {
            WndMain window = (WndMain)TopLevel.GetTopLevel(visual)!;
            if(App.EditorChangeTracker.Undo())
            {
                window.SetMessage("Performed undo");
            }
        }

        public static void Redo(this Visual visual)
        {
            WndMain window = (WndMain)TopLevel.GetTopLevel(visual)!;
            if(App.EditorChangeTracker.Redo())
            {
                window.SetMessage("Performed redo");
            }
        }
    }
}
