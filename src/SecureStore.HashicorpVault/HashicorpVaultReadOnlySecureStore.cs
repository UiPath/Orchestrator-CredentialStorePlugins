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

        public override async Task ValidateContextAsync(string context)
        {
            var ctx = ConvertJsonToContext(context);

            var keyVaultClient = _clientFactory.CreateClient(ctx);
            await keyVaultClient.TestConnection();
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
