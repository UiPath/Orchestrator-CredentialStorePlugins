using System;

namespace UiPath.Orchestrator.Extensions.SecureStores.AzureKeyVault
{
    public class AzureKeyVaultContext
    {
        public Uri KeyVaultUri { get; set; }

        public string ClientId { get; set; }

        public string ClientSecret { get; set; }
    }
}
