using WpfTest.DiskInfo;

namespace WpfTest.Commands
{
    class ShowVolumeDetailsCommand : CommandBase
    {
        public ShowVolumeDetailsCommand(MainViewModel viewModel)
            : base(viewModel)
        {            
        }

        public override bool CanExecute(object parameter)
        {
            return parameter as Volume != null;
        }

        public override void Execute(object parameter)
        {
            Volume vol = parameter as Volume;
            if (vol != null)            
                _viewModel.ShowVolumeDetails(vol);
        }
    }
}
