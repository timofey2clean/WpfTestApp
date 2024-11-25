using WpfTest.DiskInfo;

namespace WpfTest.Commands
{
    class ShowPartitionDetailsCommand : CommandBase
    {
        public ShowPartitionDetailsCommand(MainViewModel viewModel)
            : base(viewModel)
        {            
        }

        public override bool CanExecute(object parameter)
        {
            return parameter as Partition != null;
        }

        public override void Execute(object parameter)
        {
            Partition partition = parameter as Partition;
            if (partition != null)
                _viewModel.ShowPartitionDetails(partition);
        }
    }
}
