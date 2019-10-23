using System;

namespace UiPath.Orchestrator.Extensions.SecureStores.AzureKeyVault
{
    public interface IAzureKeyVaultClientFactory
    {
        IAzureKeyVaultClient CreateClient(AzureKeyVaultContext context);
    }
}
