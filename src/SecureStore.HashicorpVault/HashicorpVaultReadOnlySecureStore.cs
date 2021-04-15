using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UiPath.Orchestrator.Extensibility.Configuration;
using UiPath.Orchestrator.Extensibility.SecureStores;

namespace UiPath.Orchestrator.Extensions.SecureStores.HashicorpVault
{
    public class HashicorpVaultReadOnlySecureStore : HashicorpVaultSecureStore
    {
        private const string NameIdentifier = "Hashicorp Vault - Read Only";

        public HashicorpVaultReadOnlySecureStore(IHashicorpVaultClientFactory clientFactory)
        : base(clientFactory)
        {
        }

        public HashicorpVaultReadOnlySecureStore()
        {
        }

        public override Task ValidateContextAsync(string context)
        {
            _ = ConvertJsonToContext(context);

            // Doing a health check would require a policy to read `sys/health`.
            // We don't want to require more than strictly necessary, so we only do 
            return Task.CompletedTask;
        }

        public override SecureStoreInfo GetStoreInfo()
        {
            return new SecureStoreInfo { Identifier = NameIdentifier, IsReadOnly = true };
        }

        public override IEnumerable<ConfigurationEntry> GetConfiguration()
        {
            var baseConfiguration = base.GetConfiguration();

            foreach (var configurationEntry in baseConfiguration)
            {
                if (configurationEntry.Key == nameof(SecretsEngine) && configurationEntry is ConfigurationValue configValue)
                {
                    configValue.PossibleValues = configValue.PossibleValues.Concat(new[] { SecretsEngine.ActiveDirectory.ToString() });
                }

                yield return configurationEntry;
            }
        }
    }
}
