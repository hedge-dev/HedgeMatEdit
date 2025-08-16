using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Input;
using PropertyChanged;
using System;
using System.Windows.Input;

namespace HedgeDev.Editor.Material.Views
{
    [DoNotNotify]
    internal class UndoRedoTextBox : TextBox, ICommand
    {
        protected override Type StyleKeyOverride => typeof(TextBox);

#pragma warning disable CS0067
        public event EventHandler? CanExecuteChanged;
#pragma warning restore CS0067

        public UndoRedoTextBox() : base()
        {
            IsUndoEnabled = false;

            KeyBindings.Add(new()
            {
                Gesture = new(Key.Z, KeyModifiers.Control),
                Command = this
            });
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            BindingOperations.GetBindingExpressionBase(this, TextProperty)?.UpdateSource();
            WindowUtil.Undo(this);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if(e.Key == Key.Enter && !AcceptsReturn)
            {
                BindingOperations.GetBindingExpressionBase(this, TextProperty)?.UpdateSource();
                e.Handled = true;
                return;
            }

            base.OnKeyDown(e);
        }
    }
}
