using System.Threading;

namespace UiPath.Orchestrator.Extensions.SecureStores.BeyondTrust
{
    public class SemaphoreFactory
    {
        private static readonly SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);
        public static SemaphoreSlim SemaphoreSlim => semaphoreSlim;
    }
}
