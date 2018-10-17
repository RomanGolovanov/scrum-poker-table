using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScrumPokerTable.UI.Providers.Storage
{
    public interface IDeskStorageCleanupWorker
    {
        void Start();
        void Stop();
    }
}