using Microsoft.Extensions.Caching.Memory;
using System;

namespace UiPath.Orchestrator.Extensions.SecureStores.HashicorpVault
{
    public sealed class HashicorpVaultClientFactory : IHashicorpVaultClientFactory
    {
        private static readonly Lazy<HashicorpVaultClientFactory> _lazy = new Lazy<HashicorpVaultClientFactory>(() => new HashicorpVaultClientFactory());
        public static HashicorpVaultClientFactory Instance => _lazy.Value;

        private readonly MemoryCache _clients;
        private readonly TimeSpan _vaultClientExpiration = TimeSpan.FromMinutes(10);

        private HashicorpVaultClientFactory()
        {
            var cacheOptions = new MemoryCacheOptions
            {
                SizeLimit = 500,
            };
            _clients = new MemoryCache(cacheOptions);
        }

        public IHashicorpVaultClient CreateClient(HashicorpVaultContext context)
        {
            return _clients.GetOrCreate(context.GetHashCode(), e =>
            {
                e.Size = 1;
                e.SlidingExpiration = _vaultClientExpiration;
                return new HashicorpVaultClient(context);
            });
        }
    }
}
