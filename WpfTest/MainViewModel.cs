using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using WpfTest.Commands;
using WpfTest.Common;
using WpfTest.DiskInfo;

namespace WpfTest
{
    class MainViewModel : ViewModelBase, IObserver<DiskInfoSpec>
    {
        private readonly Model _model;
        private readonly List<IDisposable> _disposables;

        private Volume _selectedVolume;
        private PhysDisk _selectedDisk;
        private Partition _selectedPartition;

        public MainViewModel()
        {
            Volumes = new ObservableCollection<Volume>();
            PhysDisks = new ObservableCollection<PhysDisk>();
            Partitions = new ObservableCollection<Partition>();

            BrowseVolumeCommand = new BrowseVolumeCommand(this);
            ShowDiskDetailsCommand = new ShowDiskDetailsCommand(this);
            ShowVolumeDetailsCommand = new ShowVolumeDetailsCommand(this);
            ShowPartitionDetailsCommand = new ShowPartitionDetailsCommand(this);
            RefreshCommand = new RefreshCommand(this);
            ShowLogCommand = new ShowLogCommand(this);

            _model = new Model();
            _disposables = new List<IDisposable> { _model.Subscribe(this) };
        }

        public ObservableCollection<Volume> Volumes { get; private set; }
        public ObservableCollection<PhysDisk> PhysDisks { get; private set; }
        public ObservableCollection<Partition> Partitions { get; private set; }

        public Volume SelectedVolume
        {
            get
            {
                return _selectedVolume;
            }
            set
            {
                _selectedVolume = value;

                if (!ReferenceEquals(null, _selectedVolume))
                {
                    if (!ReferenceEquals(SelectedPartition, _selectedVolume.Partition))
                        SelectedPartition = _selectedVolume.Partition;

                    if (!ReferenceEquals(SelectedDisk, _selectedVolume.PhysDisk))
                        SelectedDisk = _selectedVolume.PhysDisk;
                }

                OnPropertyChanged("SelectedVolume");
            }
        }

        public Partition SelectedPartition
        {
            get
            {
                return _selectedPartition;
            }
            set
            {
                _selectedPartition = value;

                if (!ReferenceEquals(null, _selectedPartition))
                {
                    if (!ReferenceEquals(SelectedVolume, _selectedPartition.Volume))
                        SelectedVolume = _selectedPartition.Volume;

                    if (!ReferenceEquals(SelectedDisk, _selectedPartition.PhysDisk))
                        SelectedDisk = _selectedPartition.PhysDisk;
                }

                OnPropertyChanged("SelectedPartition");
            }
        }

        public PhysDisk SelectedDisk
        {
            get
            {
                return _selectedDisk;
            }
            set
            {
                _selectedDisk = value;

                if (!ReferenceEquals(null, _selectedDisk) && _selectedDisk.Partitions.Count > 0)
                {
                    if (ReferenceEquals(_selectedDisk.Partitions.FirstOrDefault(_ => _.Volume == SelectedVolume), null))
                        SelectedVolume = _selectedDisk.Partitions[0].Volume;

                    if (!_selectedDisk.Partitions.Contains(SelectedPartition))
                        SelectedPartition = _selectedDisk.Partitions[0];
                }

                OnPropertyChanged("SelectedDisk");
            }
        }
            
        #region Commands
        public ICommand BrowseVolumeCommand { get; private set; }

        public ICommand ShowDiskDetailsCommand { get; private set; }

        public ICommand ShowVolumeDetailsCommand { get; private set; }

        public ICommand ShowPartitionDetailsCommand { get; private set; }

        public ICommand RefreshCommand { get; private set; }

        public ICommand ShowLogCommand { get; private set; }
        #endregion

        #region Commands methods
        public void BrowseVolume(Volume volume)
        {
            if (!ReferenceEquals(null, volume))
                _model.BrowseDirectoryInExplorer(volume.Name);
        }

        public void ShowDiskDetails(PhysDisk disk)
        {
            if (!ReferenceEquals(null, disk))
                MessageBox.Show(disk.AsString(), "Disk details", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void ShowVolumeDetails(Volume volume)
        {
            if (!ReferenceEquals(null, volume))
                MessageBox.Show(volume.AsString(), "Volume details", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void ShowPartitionDetails(Partition partition)
        {
            if (!ReferenceEquals(null, partition))
                MessageBox.Show(partition.AsString(), "Partition details", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void Refresh()
        {
            _model.GetDiskInfo();
        }

        public void ShowLog()
        {
            MessageBox.Show(Log.Inst().GetText(), "Log", MessageBoxButton.OK, MessageBoxImage.None);
        }
        #endregion

        #region Observer
        public void OnNext(DiskInfoSpec spec)
        {
            RefillCollections(spec);
        }

        public void OnError(Exception ex)
        {
            Log.Inst().Exception(ex);
            ShowError(ex);
        }

        public void OnCompleted()
        {
            foreach (IDisposable disposable in _disposables)
            {
                disposable.Dispose();
            }
        }
        #endregion

        private void RefillCollections(DiskInfoSpec spec)
        {
            PhysDisks.Clear();
            foreach (var disk in spec.PhysDisks)
                PhysDisks.Add(disk);

            Partitions.Clear();
            foreach (var partition in spec.Patitions)
                Partitions.Add(partition);

            Volumes.Clear();
            foreach (var vol in spec.Volumes)
                Volumes.Add(vol);
        }

        private void ShowError(Exception ex)
        {
            MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
