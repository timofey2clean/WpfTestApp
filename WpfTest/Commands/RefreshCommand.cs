namespace WpfTest.Commands
{
    class RefreshCommand : CommandBase
    {
        public RefreshCommand(MainViewModel viewModel)
            : base(viewModel)
        {            
        }

        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override void Execute(object parameter)
        {
            _viewModel.Refresh();         
        }
    }
}
