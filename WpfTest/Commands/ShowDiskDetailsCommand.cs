using WpfTest.DiskInfo;

namespace WpfTest.Commands
{
    class ShowDiskDetailsCommand : CommandBase
    {
        public ShowDiskDetailsCommand(MainViewModel viewModel)
            : base(viewModel)
        {            
        }

        public override bool CanExecute(object parameter)
        {
            return parameter as PhysDisk != null;
        }

        public override void Execute(object parameter)
        {
            PhysDisk disk = parameter as PhysDisk;
            if (disk != null)            
                _viewModel.ShowDiskDetails(disk);            
        }
    }
}
