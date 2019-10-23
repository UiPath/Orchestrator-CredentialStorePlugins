using System;

namespace UiPath.Orchestrator.AzureKeyVault.SecureStore
{
    public class SecureKeyMetadata
    {
        public DateTime CreateTime { get; set; }

        public string VaultSecretName { get; set; }

        public string ExternalName { get; set; }
    }
}
