namespace UiPath.Orchestrator.Extensions.SecureStores.HashicorpVault
{
    public interface IHashicorpVaultClientFactory
    {
        IHashicorpVaultClient CreateClient(HashicorpVaultContext context);
    }
}
