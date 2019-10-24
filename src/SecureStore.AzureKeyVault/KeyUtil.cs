using Newtonsoft.Json;
using System;
using System.Globalization;
using System.Numerics;
using System.Text;
using UiPath.Orchestrator.Extensibility.SecureStores;
using UiPath.Orchestrator.Extensions.SecureStores.AzureKeyVault;

namespace UiPath.Orchestrator.AzureKeyVault.SecureStore
{
    public static class KeyUtil
    {
        // Azure Key Vault names support only strings matching "^[0-9a-zA-Z-]{3-24}$" regex
        // https://docs.microsoft.com/en-us/azure/key-vault/about-keys-secrets-and-certificates
        private const string SupportedSecretCharacterSet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIZKLMNOPQRSTUVWXYZ0123456789-";

        /// <summary>
        /// Construct a new name based on a GUID represented with the valid characters for a Azure Key Vault
        /// </summary>
        /// <returns>new unique name for a secret</returns>
        public static string GetNewSecretName()
        {
            var id = Guid.NewGuid();
            BigInteger bigIntId = new BigInteger(id.ToByteArray());
            bigIntId = BigInteger.Abs(bigIntId);
            StringBuilder sb = new StringBuilder();
            BigInteger supportedCharCount = SupportedSecretCharacterSet.Length;
            while (!bigIntId.IsZero)
            {
                int indexChar = (int)(bigIntId % supportedCharCount);
                sb.Append(SupportedSecretCharacterSet[indexChar]);
                bigIntId /= supportedCharCount;
            }

            return sb.ToString();
        }

        public static SecureKeyMetadata GetWriteMetadata(this string externalName, string lastPasswordKey)
        {
            if (lastPasswordKey == null)
            {
                return new SecureKeyMetadata
                {
                    CreateTime = DateTime.UtcNow,
                    VaultSecretName = KeyUtil.GetNewSecretName(),
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
                   AzureKeyVaultUtils.GetLocalizedResource(nameof(Resource.InvalidSecureStoreSecretName)));
            }

            return result;
        }
    }
}
