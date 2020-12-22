using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UiPath.Orchestrator.Extensibility.Configuration;
using UiPath.Orchestrator.Extensibility.SecureStores;
using UiPath.Orchestrator.Extensions.SecureStores.HashicorpVault.Resources;
using VaultSharp.Core;

namespace UiPath.Orchestrator.Extensions.SecureStores.HashicorpVault
{
    public class HashicorpVaultSecureStore : ISecureStore
    {
        public const string NameIdentifier = "HashicorpVault";

        private readonly IHashicorpVaultClientFactory _clientFactory;

        public HashicorpVaultSecureStore(
            IHashicorpVaultClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public HashicorpVaultSecureStore()
        {
            _clientFactory = HashicorpVaultClientFactory.Instance;
        }

        public async Task<string> GetValueAsync(string context, string key)
        {
            var ctx = ConvertJsonToContext(context);
            if (key == null)
            {
                return null; // support for null password
            }

            return await ExecuteHashicorpVaultOperation(
                async () =>
                {
                    var client = _clientFactory.CreateClient(ctx);
                    return await client.GetSecretAsync(key);
                },
                "get");
        }

        public async Task RemoveValueAsync(string context, string key)
        {
            var ctx = ConvertJsonToContext(context);
            key = key ?? throw new ArgumentNullException(nameof(key));

            try
            {
                await ExecuteHashicorpVaultOperation(
                    async () =>
                    {
                        var client = _clientFactory.CreateClient(ctx);
                        await client.DeleteSecretAsync(key);
                    },
                    "delete");
            }
            catch (SecureStoreException sse) when (sse.ErrorType == SecureStoreException.Type.SecretNotFound)
            {
                // Ignore SecretNotFound
            }
        }

        public async Task<string> CreateValueAsync(string context, string key, string value)
        {
            var ctx = ConvertJsonToContext(context);

            // key is null for new secret
            value = value ?? throw new ArgumentNullException(nameof(value));

            await ExecuteHashicorpVaultOperation(
                async () =>
                {
                    var client = _clientFactory.CreateClient(ctx);
                    return await client.SetSecretAsync(key, value);
                },
                "set");

            return key;
        }

        public async Task<Credential> GetCredentialsAsync(string context, string key)
        {
            var ctx = ConvertJsonToContext(context);
            key = key ?? throw new SecureStoreException();

            var secret = await ExecuteHashicorpVaultOperation(
                async () =>
                {
                    var client = _clientFactory.CreateClient(ctx);
                    return await client.GetCredentialAsync(key);
                },
                "get");

            return secret;
        }

        public async Task<string> CreateCredentialsAsync(string context, string key, Credential value)
        {
            var ctx = ConvertJsonToContext(context);
            key = key ?? throw new ArgumentNullException(nameof(key));

            await ExecuteHashicorpVaultOperation(
                async () =>
                {
                    var client = _clientFactory.CreateClient(ctx);
                    return await client.SetCredentialAsync(key, value);
                },
                "set");
            return key;
        }

        public async Task<string> UpdateValueAsync(string context, string key, string oldAugumentedKey, string value)
        {
            var ctx = ConvertJsonToContext(context);
            key = key ?? throw new ArgumentNullException(nameof(key));
            oldAugumentedKey = oldAugumentedKey ?? throw new ArgumentNullException(nameof(oldAugumentedKey));
            value = value ?? throw new ArgumentNullException(nameof(value));

            if (key.Equals(oldAugumentedKey, StringComparison.Ordinal))
            {
                await ExecuteHashicorpVaultOperation(
                    async () =>
                    {
                        var client = _clientFactory.CreateClient(ctx);
                        return await client.SetSecretAsync(key, value);
                    },
                    "set");
            }
            else
            {
                await CreateValueAsync(context, key, value);
                await RemoveValueAsync(context, oldAugumentedKey);
            }

            return key;
        }

        public async Task<string> UpdateCredentialsAsync(string context, string key, string oldAugumentedKey, Credential value)
        {
            var ctx = ConvertJsonToContext(context);
            key = key ?? throw new ArgumentNullException(nameof(key));
            oldAugumentedKey = oldAugumentedKey ?? throw new ArgumentNullException(nameof(oldAugumentedKey));
            value = value ?? throw new ArgumentNullException(nameof(value));

            await ExecuteHashicorpVaultOperation(
                async () =>
                {
                    var client = _clientFactory.CreateClient(ctx);
                    return await client.SetCredentialAsync(key, value);
                },
                "set");

            return key;
        }

        public void Initialize(Dictionary<string, string> hostSettings)
        {
            // No-op : current implementation does not have a host level configuration
        }

        public SecureStoreInfo GetStoreInfo()
        {
            return new SecureStoreInfo { Identifier = NameIdentifier, IsReadOnly = false };
        }

        public async Task ValidateContextAsync(string context)
        {
            var ctx = ConvertJsonToContext(context);

            var keyVaultClient = _clientFactory.CreateClient(ctx);

            var secretName = "UIPATH-TEST-SECRET-HASHICORP-VAULT";
            var secretValue = Guid.NewGuid().ToString();

            var storageKey = await ExecuteHashicorpVaultOperation(
                async () =>
                {
                    return await keyVaultClient.SetSecretAsync(secretName, secretValue);
                },
                "set");

            _ = await ExecuteHashicorpVaultOperation(
                async () =>
                {
                    return await keyVaultClient.GetSecretAsync(storageKey);
                },
                "get");

            try
            {
                // We use destroy because we don't want to leave behind key metadata with nothing inside for each edit of the store.
                // If destroy fails (maybe due to policies), we just delete the secret version we created.
                // This is ok to do since we don't need to destroy secrets during normal operation, just for testing.
                await ExecuteHashicorpVaultOperation(
                    async () =>
                    {
                        await keyVaultClient.DeleteSecretAsync(secretName, destroy: true);
                    },
                    "destroy");
            }
            catch
            {
                await ExecuteHashicorpVaultOperation(
                    async () =>
                    {
                        await keyVaultClient.DeleteSecretAsync(secretName, destroy: false);
                    },
                    "delete");
            }
        }

        public IEnumerable<ConfigurationEntry> GetConfiguration()
        {
            return new List<ConfigurationEntry>
            {
                new ConfigurationValue(ConfigurationValueType.String)
                {
                    Key = "VaultUri",
                    DisplayName = HashicorpVaultUtils.GetLocalizedResource(nameof(Resource.SettingVaultUri)),
                    IsMandatory = true,
                },
                new ConfigurationValue(ConfigurationValueType.Choice)
                {
                    Key = "AuthenticationType",
                    DisplayName = HashicorpVaultUtils.GetLocalizedResource(nameof(Resource.SettingAuthenticationType)),
                    IsMandatory = true,
                    PossibleValues = Enum.GetNames(typeof(AuthenticationType)),
                },
                new ConfigurationValue(ConfigurationValueType.Secret)
                {
                    Key = "RoleId",
                    DisplayName = HashicorpVaultUtils.GetLocalizedResource(nameof(Resource.SettingRoleId)),
                    IsMandatory = false,
                },
                new ConfigurationValue(ConfigurationValueType.Secret)
                {
                    Key = "SecretId",
                    DisplayName = HashicorpVaultUtils.GetLocalizedResource(nameof(Resource.SettingSecretId)),
                    IsMandatory = false,
                },
                new ConfigurationValue(ConfigurationValueType.String)
                {
                    Key = "Username",
                    DisplayName = HashicorpVaultUtils.GetLocalizedResource(nameof(Resource.SettingUsername)),
                    IsMandatory = false,
                },
                new ConfigurationValue(ConfigurationValueType.Secret)
                {
                    Key = "Password",
                    DisplayName = HashicorpVaultUtils.GetLocalizedResource(nameof(Resource.SettingPassword)),
                    IsMandatory = false,
                },
                new ConfigurationValue(ConfigurationValueType.Secret)
                {
                    Key = "Token",
                    DisplayName = HashicorpVaultUtils.GetLocalizedResource(nameof(Resource.SettingToken)),
                    IsMandatory = false,
                },
                new ConfigurationValue(ConfigurationValueType.Choice)
                {
                    Key = "SecretsEngine",
                    DisplayName = HashicorpVaultUtils.GetLocalizedResource(nameof(Resource.SettingSecretsEngine)),
                    IsMandatory = true,
                    PossibleValues = Enum.GetNames(typeof(SecretsEngine)),
                },
                new ConfigurationValue(ConfigurationValueType.String)
                {
                    Key = "SecretsEnginePath",
                    DisplayName = HashicorpVaultUtils.GetLocalizedResource(nameof(Resource.SettingSecretsEnginePath)),
                    IsMandatory = false,
                },
                new ConfigurationValue(ConfigurationValueType.String)
                {
                    Key = "DataPath",
                    DisplayName = HashicorpVaultUtils.GetLocalizedResource(nameof(Resource.SettingDataPath)),
                    IsMandatory = false,
                },
                new ConfigurationValue(ConfigurationValueType.String)
                {
                    Key = "Namespace",
                    DisplayName = HashicorpVaultUtils.GetLocalizedResource(nameof(Resource.SettingNamespace)),
                    IsMandatory = false,
                },
            };
        }

        private static HashicorpVaultContext ConvertJsonToContext(string context)
        {
            return new HashicorpVaultContextBuilder().FromJson(context).Build();
        }

        private static async Task<T> ExecuteHashicorpVaultOperation<T>(Func<Task<T>> func, string operation)
        {
            try
            {
                return await func();
            }
            catch (VaultApiException vEx) when (vEx.HttpStatusCode == System.Net.HttpStatusCode.Forbidden)
            {
                throw new SecureStoreException(
                    SecureStoreException.Type.UnauthorizedOperation,
                    HashicorpVaultUtils.GetLocalizedResource(nameof(Resource.SecureStoreOperationNotAuthorizeded), operation),
                    vEx);
            }
            catch (VaultApiException vEx) when (vEx.HttpStatusCode == System.Net.HttpStatusCode.NotFound)
            {
                throw new SecureStoreException(
                    SecureStoreException.Type.SecretNotFound,
                    HashicorpVaultUtils.GetLocalizedResource(nameof(Resource.SecureStoreSecretNotFound)),
                    vEx);
            }
            catch (Exception ex)
            {
                throw new SecureStoreException($"Operation {operation} failed.", ex);
            }
        }

        private static async Task ExecuteHashicorpVaultOperation(Func<Task> func, string operation)
        {
            try
            {
                await func();
            }
            catch (VaultApiException vEx) when (vEx.HttpStatusCode == System.Net.HttpStatusCode.Forbidden)
            {
                throw new SecureStoreException(
                    SecureStoreException.Type.UnauthorizedOperation,
                    HashicorpVaultUtils.GetLocalizedResource(nameof(Resource.SecureStoreOperationNotAuthorizeded), operation),
                    vEx);
            }
            catch (VaultApiException vEx) when (vEx.HttpStatusCode == System.Net.HttpStatusCode.NotFound)
            {
                throw new SecureStoreException(
                    SecureStoreException.Type.SecretNotFound,
                    HashicorpVaultUtils.GetLocalizedResource(nameof(Resource.SecureStoreSecretNotFound)),
                    vEx);
            }
            catch (Exception ex)
            {
                throw new SecureStoreException($"Operation {operation} failed.", ex);
            }
        }
    }
}
