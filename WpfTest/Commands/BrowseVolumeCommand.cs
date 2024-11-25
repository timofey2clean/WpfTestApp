using WpfTest.DiskInfo;

namespace WpfTest.Commands
{
    class BrowseVolumeCommand : CommandBase
    {
        public BrowseVolumeCommand(MainViewModel viewModel)
            : base(viewModel)
        {            
        }

        public override bool CanExecute(object parameter)
        {
            return parameter as Volume != null;
        }

        public override void Execute(object parameter)
        {
            Volume volume = parameter as Volume;
            if (volume != null)            
                _viewModel.BrowseVolume(volume);            
        }
    }
}
