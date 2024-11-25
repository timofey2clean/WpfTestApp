using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WpfTest.Commands
{
    class ShowLogCommand : CommandBase
    {
        public ShowLogCommand(MainViewModel viewModel)
            : base(viewModel)
        {            
        }

        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override void Execute(object parameter)
        {
            _viewModel.ShowLog();         
        }
    }
}
