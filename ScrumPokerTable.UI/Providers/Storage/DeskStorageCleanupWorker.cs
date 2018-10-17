using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Hosting;

namespace ScrumPokerTable.UI.Providers.Storage
{
    public class DeskStorageCleanupWorker : IDeskStorageCleanupWorker
    {
        public DeskStorageCleanupWorker(
            IDeskStorage deskStorage,
            TimeSpan timeToLive)
        {
            _deskStorage = deskStorage;
            _timeToLive = timeToLive;
        }

        public void Start()
        {
            lock (_syncObject)
            {
                if (_cancellationTokenSource != null)
                {
                    throw new Exception("Cleanup worker already started");
                }
                _cancellationTokenSource = new CancellationTokenSource();
                HostingEnvironment.QueueBackgroundWorkItem(
                    ct =>
                    {
                        _cleanupWorkerTask = CleanupWorker(ct, _cancellationTokenSource.Token);
                        return _cleanupWorkerTask;
                    });
            }
        }

        public void Stop()
        {
            _cancellationTokenSource?.Cancel();
            _cleanupWorkerTask?.Wait();

            _cancellationTokenSource = null;
            _cleanupWorkerTask = null;
        }

        private async Task CleanupWorker(CancellationToken backgroundCancellationToken, CancellationToken cancellationToken)
        {
            var cancellationTokens = new[] { backgroundCancellationToken, cancellationToken };
            while (!cancellationTokens.Any(c => c.IsCancellationRequested))
            {
                CleanupOldDesks();

                await Task.Delay(TimeSpan.FromHours(1), cancellationToken);
            }
        }

        private void CleanupOldDesks()
        {
            foreach (var desk in _deskStorage.GetDesks())
            {
                if (desk.Timestamp.Add(_timeToLive) < DateTime.UtcNow)
                {
                    _deskStorage.DeleteDesk(desk);
                }
            }
        }

        private Task _cleanupWorkerTask = null;
        private readonly Object _syncObject = new Object();
        private volatile CancellationTokenSource _cancellationTokenSource;
        private readonly IDeskStorage _deskStorage;
        private readonly TimeSpan _timeToLive;
    }
}