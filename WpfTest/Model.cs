using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using WpfTest.DiskInfo;

namespace WpfTest
{
    class Model : IObservable<DiskInfoSpec>
    {
        private readonly List<IObserver<DiskInfoSpec>> _diskSpecObservers;
        
        public Model()
        {
            _diskSpecObservers = new List<IObserver<DiskInfoSpec>>();
        }

        public void GetDiskInfo()
        {
            try
            {
                ComputerDiskInfo compDiskInfo = new ComputerDiskInfo();
                DiskInfoSpec spec = compDiskInfo.GetAllDisks();
                Notify(spec);
            }
            catch (Exception ex)
            {
                NotifyError(new Exception("Failed to get disk info.", ex));
            }
        }

        public void BrowseDirectoryInExplorer(String path)
        {
            try
            {
                if (!Directory.Exists(path))
                    throw new ArgumentException("Directory '{0}' not found.", path);

                new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = @"explorer.exe",
                        Arguments = path,
                        UseShellExecute = true,
                        RedirectStandardOutput = false,
                        RedirectStandardError = false
                    }
                }.Start();
            }
            catch (Exception ex)
            {
                NotifyError(new Exception(string.Format("Failed to open '{0}' in file explorer.", path), ex));
            }
        }

        #region Observable
        public IDisposable Subscribe(IObserver<DiskInfoSpec> observer)
        {
            return new Unsubscriber<DiskInfoSpec>(_diskSpecObservers, observer);
        }

        private void Notify(DiskInfoSpec data)
        {
            foreach (var o in _diskSpecObservers)
            {
                o.OnNext(data);
            }
        }

        private void NotifyError(Exception ex)
        {
            foreach (var o in _diskSpecObservers)
            {
                o.OnError(ex);
            }
        }
        #endregion
    }

    class Unsubscriber<T> : IDisposable
    {
        private readonly List<IObserver<T>> _observers;
        private readonly IObserver<T> _newObserver;

        public Unsubscriber(List<IObserver<T>> observers, IObserver<T> newObserver)
        {
            _observers = observers;
            _newObserver = newObserver;

            if (!_observers.Contains(newObserver))
                _observers.Add(newObserver);
        }

        public void Dispose()
        {
            if (_observers.Contains(_newObserver))
                _observers.Remove(_newObserver);
        }
    }
}
