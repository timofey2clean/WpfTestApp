using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace WpfTest.Commands
{
    abstract class CommandBase : ICommand
    {
        protected readonly MainViewModel _viewModel;

        public CommandBase(MainViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        abstract public bool CanExecute(object parameter);

        abstract public void Execute(object parameter);
    }
}
