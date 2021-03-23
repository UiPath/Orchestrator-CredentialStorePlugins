using Newtonsoft.Json;
using System;
using UiPath.Orchestrator.Extensibility.SecureStores;
using UiPath.Orchestrator.Extensions.SecureStores.HashicorpVault.Resources;

namespace UiPath.Orchestrator.Extensions.SecureStores.HashicorpVault
{
    public static class KeyUtil
    {
        /// <summary>
        /// Construct a new name based on a GUID
        /// </summary>
        /// <returns>new unique name for a secret</returns>
        public static string GetNewSecretName()
        {
            return Guid.NewGuid().ToString();
        }

        public static SecureKeyMetadata GetWriteMetadata(this string externalName, string lastPasswordKey)
        {
            if (lastPasswordKey == null)
            {
                return new SecureKeyMetadata
                {
                    CreateTime = DateTime.UtcNow,
                    VaultSecretName = GetNewSecretName(),
                    ExternalName = externalName,
                };
            }
            else
            {
                var lastMetadata = GetExistingMetadata(lastPasswordKey);
                lastMetadata.ExternalName = externalName;
                return lastMetadata;
            }
        }

        public static SecureKeyMetadata GetExistingMetadata(this string passwordKey)
        {
            SecureKeyMetadata result = JsonConvert.DeserializeObject<SecureKeyMetadata>(passwordKey);
            if (string.IsNullOrEmpty(result.VaultSecretName))
            {
                throw new SecureStoreException(
                   SecureStoreException.Type.Unknown,
                   HashicorpVaultUtils.GetLocalizedResource(nameof(Resource.InvalidSecureStoreSecretName)));
            }

            return result;
        }
    }
}
