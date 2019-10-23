using System;

namespace UiPath.Orchestrator.Extensions.SecureStores.AzureKeyVault
{
    public class AzureKeyVaultClientFactory : IAzureKeyVaultClientFactory
    {
        public IAzureKeyVaultClient CreateClient(AzureKeyVaultContext context)
        {
            return new AzureKeyVaultClient(context);
        }
    }
}
