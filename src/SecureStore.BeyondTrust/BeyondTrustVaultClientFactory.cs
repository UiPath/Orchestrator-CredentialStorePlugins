using Newtonsoft.Json;
using System.Collections.Generic;

namespace UiPath.Orchestrator.Extensions.SecureStores.BeyondTrust
{
    public class BeyondTrustVaultClientFactory
    {
        private static BeyondTrustVaultClient client;

        public static BeyondTrustVaultClient GetClient(string context)
        {
            if (client == null)
            {
                var config = JsonConvert.DeserializeObject<Dictionary<string, object>>(context);
                client = new BeyondTrustVaultClient(config);
            }
            return client;
        }
    }
}
