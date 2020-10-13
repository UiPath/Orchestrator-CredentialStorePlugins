using System;

namespace UiPath.Orchestrator.Extensions.SecureStores.HashicorpVault
{
    public class SecureKeyMetadata
    {
        public DateTime CreateTime { get; set; }

        public string VaultSecretName { get; set; }

        public string ExternalName { get; set; }
    }
}
