using AzureKeyVault.SecureStore;
using Microsoft.Azure.KeyVault.Models;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UiPath.Orchestrator.AzureKeyVault.SecureStore;
using UiPath.Orchestrator.Extensibility.Configuration;
using UiPath.Orchestrator.Extensibility.SecureStores;

namespace UiPath.Orchestrator.Extensions.SecureStores.AzureKeyVault
{
    public class AzureKeyVaultSecureStore : ISecureStore
    {
        public const string NameIdentifier = "AzureKeyVault";

        private readonly IAzureKeyVaultClientFactory _clientFactory;

        public AzureKeyVaultSecureStore(
            IAzureKeyVaultClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public AzureKeyVaultSecureStore()
        {
            _clientFactory = new AzureKeyVaultClientFactory();
        }

        public async Task<string> GetValueAsync(string context, string key)
        {
            var ctx = ConvertJsonToContext(context);
            key = key ?? throw new SecureStoreException();
            var passwordKey = key.GetExistingMetadata();

            return await ExecuteAzureKeyVaultOperation(
                async () =>
                {
                    IAzureKeyVaultClient keyVaultClient = _clientFactory.CreateClient(ctx);
                    return await keyVaultClient.GetSecretAsync(passwordKey.VaultSecretName);
                },
                "get");
        }

        public async Task RemoveValueAsync(string context, string key)
        {
            var ctx = ConvertJsonToContext(context);
            key = key ?? throw new ArgumentNullException(nameof(key));
            var passwordKey = key.GetExistingMetadata();

            try
            {
                await ExecuteAzureKeyVaultOperation(
                    async () =>
                    {
                        IAzureKeyVaultClient keyVaultClient = _clientFactory.CreateClient(ctx);
                        await keyVaultClient.DeleteSecretAsync(passwordKey.VaultSecretName);
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
            var passwordKey = key.GetWriteMetadata(null);

            await ExecuteAzureKeyVaultOperation(
                async () =>
                {
                    IAzureKeyVaultClient keyVaultClient = _clientFactory.CreateClient(ctx);
                    return await keyVaultClient.SetSecretAsync(passwordKey.VaultSecretName, value);
                },
                "set");
            return JsonConvert.SerializeObject(passwordKey);
        }

        public async Task<Credential> GetCredentialsAsync(string context, string key)
        {
            var ctx = ConvertJsonToContext(context);
            key = key ?? throw new SecureStoreException();
            var passwordKey = key.GetExistingMetadata();

            var secret = await ExecuteAzureKeyVaultOperation(
                async () =>
                {
                    IAzureKeyVaultClient keyVaultClient = _clientFactory.CreateClient(ctx);
                    return await keyVaultClient.GetSecretAsync(passwordKey.VaultSecretName);
                },
                "get");

            return JsonConvert.DeserializeObject<Credential>(secret);
        }

        public async Task<string> CreateCredentialsAsync(string context, string key, Credential value)
        {
            var ctx = ConvertJsonToContext(context);
            key = key ?? throw new ArgumentNullException(nameof(key));
            var serializedCredential = JsonConvert.SerializeObject(value);

            var passwordKey = key.GetWriteMetadata(null);

            await ExecuteAzureKeyVaultOperation(
                async () =>
                {
                    IAzureKeyVaultClient keyVaultClient = _clientFactory.CreateClient(ctx);
                    return await keyVaultClient.SetSecretAsync(passwordKey.VaultSecretName, serializedCredential);
                },
                "set");
            return JsonConvert.SerializeObject(passwordKey);
        }


        public async Task<string> UpdateValueAsync(string context, string key, string oldAugumentedKey, string value)
        {
            var ctx = ConvertJsonToContext(context);
            key = key ?? throw new ArgumentNullException(nameof(key));
            oldAugumentedKey = oldAugumentedKey ?? throw new ArgumentNullException(nameof(oldAugumentedKey));
            value = value ?? throw new ArgumentNullException(nameof(value));

            var passwordKey = key.GetWriteMetadata(oldAugumentedKey);

            await ExecuteAzureKeyVaultOperation(
                async () =>
                {
                    IAzureKeyVaultClient keyVaultClient = _clientFactory.CreateClient(ctx);
                    return await keyVaultClient.SetSecretAsync(passwordKey.VaultSecretName, value);
                },
                "set");
            return JsonConvert.SerializeObject(passwordKey);
        }

        public async Task<string> UpdateCredentialsAsync(string context, string key, string oldAugumentedKey, Credential value)
        {
            var ctx = ConvertJsonToContext(context);
            key = key ?? throw new ArgumentNullException(nameof(key));
            oldAugumentedKey = oldAugumentedKey ?? throw new ArgumentNullException(nameof(oldAugumentedKey));
            value = value ?? throw new ArgumentNullException(nameof(value));
            var serializedCredential = JsonConvert.SerializeObject(value);

            var passwordKey = key.GetWriteMetadata(oldAugumentedKey);

            await ExecuteAzureKeyVaultOperation(
                async () =>
                {
                    IAzureKeyVaultClient keyVaultClient = _clientFactory.CreateClient(ctx);
                    return await keyVaultClient.SetSecretAsync(passwordKey.VaultSecretName, serializedCredential);
                },
                "set");
            return JsonConvert.SerializeObject(passwordKey);
        }

        public void Initialize(Dictionary<string, string> hostSettings)
        {
            // No-op : current implementation of AzureKeyVault does not have a host level configuration
        }

        public SecureStoreInfo GetStoreInfo()
        {
            return new SecureStoreInfo { Identifier = NameIdentifier, IsReadOnly = false };
        }

        public async Task ValidateContextAsync(string context)
        {
            var ctx = ConvertJsonToContext(context);

            var keyVaultClient = _clientFactory.CreateClient(ctx);
            var secretName = "UIPATH-TEST-SECRET";
            var secretValue = "SECRET";

            var storageKey = await ExecuteAzureKeyVaultOperation(
                async () =>
                {
                    return await keyVaultClient.SetSecretAsync(secretName, secretValue);
                },
                "set");

            _ = await ExecuteAzureKeyVaultOperation(
                async () =>
                {
                    return await keyVaultClient.GetSecretAsync(storageKey);
                },
                "get");

            await ExecuteAzureKeyVaultOperation(
                async () =>
                {
                    await keyVaultClient.DeleteSecretAsync(secretName);
                },
                "delete");
        }

        public IEnumerable<ConfigurationEntry> GetConfiguration()
        {
            return new List<ConfigurationEntry>
            {
                new ConfigurationValue(ConfigurationValueType.String)
                {
                    Key = "KeyVaultUri",
                    DisplayName = AzureKeyVaultUtils.GetLocalizedResource(nameof(Resource.SettingKeyVaultUri)),
                    IsMandatory = true,
                },
                new ConfigurationValue(ConfigurationValueType.String)
                {
                    Key = "ClientId",
                    DisplayName = AzureKeyVaultUtils.GetLocalizedResource(nameof(Resource.SettingClientId)),
                    IsMandatory = true,
                },
                new ConfigurationValue(ConfigurationValueType.String)
                {
                    Key = "ClientSecret",
                    DisplayName = AzureKeyVaultUtils.GetLocalizedResource(nameof(Resource.SettingClientSecret)),
                    IsMandatory = true,
                },
            };
        }

        private AzureKeyVaultContext ConvertJsonToContext(string context)
        {
            return new AzureKeyVaultContextBuilder().FromJson(context).Build();
        }

        private async Task<T> ExecuteAzureKeyVaultOperation<T>(Func<Task<T>> func, string operation)
        {
            try
            {
                return await func();
            }
            catch (AdalServiceException asex)
            {
                throw new SecureStoreException(
                    SecureStoreException.Type.InvalidCredential,
                    AzureKeyVaultUtils.GetLocalizedResource(nameof(Resource.InvalidSecureStoreCredentials)),
                    asex);
            }
            catch (KeyVaultErrorException kvee) when (kvee.Response?.StatusCode == System.Net.HttpStatusCode.Forbidden)
            {
                throw new SecureStoreException(
                    SecureStoreException.Type.UnauthorizedOperation,
                    AzureKeyVaultUtils.GetLocalizedResource(nameof(Resource.SecureStoreOperationNotAuthorizeded), operation),
                    kvee);
            }
            catch (KeyVaultErrorException kvee) when (kvee.Response?.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                throw new SecureStoreException(
                    SecureStoreException.Type.SecretNotFound,
                    AzureKeyVaultUtils.GetLocalizedResource(nameof(Resource.SecureStoreSecretNotFound)),
                    kvee);
            }
            catch (Exception ex)
            {
                throw new SecureStoreException($"Operation {operation} failed.", ex);
            }
        }

        private async Task ExecuteAzureKeyVaultOperation(Func<Task> func, string operation)
        {
            try
            {
                await func();
            }
            catch (AdalServiceException asex)
            {
                throw new SecureStoreException(
                    SecureStoreException.Type.InvalidCredential,
                    AzureKeyVaultUtils.GetLocalizedResource(nameof(Resource.InvalidSecureStoreCredentials)),
                    asex);
            }
            catch (KeyVaultErrorException kvee) when (kvee.Response?.StatusCode == System.Net.HttpStatusCode.Forbidden)
            {
                throw new SecureStoreException(
                    SecureStoreException.Type.UnauthorizedOperation,
                    AzureKeyVaultUtils.GetLocalizedResource(nameof(Resource.SecureStoreOperationNotAuthorizeded), operation),
                    kvee);
            }
            catch (KeyVaultErrorException kvee) when (kvee.Response?.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                throw new SecureStoreException(
                    SecureStoreException.Type.SecretNotFound,
                    AzureKeyVaultUtils.GetLocalizedResource(nameof(Resource.SecureStoreSecretNotFound)),
                    kvee);
            }
            catch (Exception ex)
            {
                throw new SecureStoreException($"Operation {operation} failed.", ex);
            }
        }
    }
}
