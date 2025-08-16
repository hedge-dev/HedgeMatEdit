using System;
using System.Windows.Input;

namespace HedgeDev.Editor.Material.ViewModels
{
    internal class RelayCommand : ICommand
    {
        private readonly Action _action;

#pragma warning disable CS0067 // Event is never used
        public event EventHandler? CanExecuteChanged;
#pragma warning restore CS0067

        public RelayCommand(Action action)
        {
            _action = action;
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            _action();
        }
    }
}
