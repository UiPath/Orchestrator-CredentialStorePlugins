namespace UiPath.Orchestrator.Extensions.SecureStores.HashicorpVault
{
    public class HashicorpVaultClientFactory : IHashicorpVaultClientFactory
    {
        public IHashicorpVaultClient CreateClient(HashicorpVaultContext context)
        {
            return new HashicorpVaultClient(context);
        }
    }
}
