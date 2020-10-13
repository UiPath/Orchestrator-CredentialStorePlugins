using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UiPath.Orchestrator.Extensibility.Configuration;
using UiPath.Orchestrator.Extensibility.SecureStores;
using UiPath.Orchestrator.Extensions.SecureStores.HashicorpVault.Resources;

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
            _clientFactory = new HashicorpVaultClientFactory();
        }

        public async Task<string> GetValueAsync(string context, string key)
        {
            var ctx = ConvertJsonToContext(context);
            if (key == null)
            {
                return null; // support for null password
            }

            var passwordKey = key.GetExistingMetadata();

            return await ExecuteHashicorpVaultOperation(
                async () =>
                {
                    IHashicorpVaultClient keyVaultClient = _clientFactory.CreateClient(ctx);
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
                await ExecuteHashicorpVaultOperation(
                    async () =>
                    {
                        IHashicorpVaultClient keyVaultClient = _clientFactory.CreateClient(ctx);
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

            await ExecuteHashicorpVaultOperation(
                async () =>
                {
                    IHashicorpVaultClient keyVaultClient = _clientFactory.CreateClient(ctx);
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

            var secret = await ExecuteHashicorpVaultOperation(
                async () =>
                {
                    IHashicorpVaultClient keyVaultClient = _clientFactory.CreateClient(ctx);
                    return await keyVaultClient.GetCredentialAsync(passwordKey.VaultSecretName);
                },
                "get");

            return secret;
        }

        public async Task<string> CreateCredentialsAsync(string context, string key, Credential value)
        {
            var ctx = ConvertJsonToContext(context);
            key = key ?? throw new ArgumentNullException(nameof(key));

            var passwordKey = key.GetWriteMetadata(null);

            await ExecuteHashicorpVaultOperation(
                async () =>
                {
                    IHashicorpVaultClient keyVaultClient = _clientFactory.CreateClient(ctx);
                    return await keyVaultClient.SetCredentialAsync(passwordKey.VaultSecretName, value);
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

            await ExecuteHashicorpVaultOperation(
                async () =>
                {
                    IHashicorpVaultClient keyVaultClient = _clientFactory.CreateClient(ctx);
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

            var passwordKey = key.GetWriteMetadata(oldAugumentedKey);

            await ExecuteHashicorpVaultOperation(
                async () =>
                {
                    IHashicorpVaultClient keyVaultClient = _clientFactory.CreateClient(ctx);
                    return await keyVaultClient.SetCredentialAsync(passwordKey.VaultSecretName, value);
                },
                "set");
            return JsonConvert.SerializeObject(passwordKey);
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

            // The secret name is supposed to be deleted after the validation
            // However, if the delete operation fails, and the azure key vault is set to soft-delete policy.
            // Then the same name can not be used until the secret is purged.
            var secretName = "UIPATH-TEST-SECRET" + Guid.NewGuid();
            var secretValue = "SECRET";

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

            await ExecuteHashicorpVaultOperation(
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
                new ConfigurationSection
                {
                    Key = nameof(AuthenticationType.AppRole),
                    DisplayName = nameof(AuthenticationType.AppRole),
                    Configurations = new[]
                    {
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
                    },
                },
                new ConfigurationSection
                {
                    Key = $"{nameof(AuthenticationType.UsernamePassword)}/{nameof(AuthenticationType.Ldap)}",
                    DisplayName = nameof(AuthenticationType.ClientCertificate),
                    Configurations = new[]
                    {
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
                    },
                },
                new ConfigurationSection
                {
                    Key = nameof(AuthenticationType.ClientCertificate),
                    DisplayName = nameof(AuthenticationType.ClientCertificate),
                    Configurations = new[]
                    {
                        new ConfigurationValue(ConfigurationValueType.Secret)
                        {
                            Key = "Certificate",
                            DisplayName = HashicorpVaultUtils.GetLocalizedResource(nameof(Resource.SettingCertificate)),
                            IsMandatory = false,
                        },
                        new ConfigurationValue(ConfigurationValueType.Secret)
                        {
                            Key = "CertificatePassword",
                            DisplayName = HashicorpVaultUtils.GetLocalizedResource(nameof(Resource.SettingCertificatePassword)),
                            IsMandatory = false,
                        },
                    },
                },
                new ConfigurationSection
                {
                    Key = nameof(AuthenticationType.Token),
                    DisplayName = nameof(AuthenticationType.Token),
                    Configurations = new[]
                    {
                        new ConfigurationValue(ConfigurationValueType.Secret)
                        {
                            Key = "Token",
                            DisplayName = HashicorpVaultUtils.GetLocalizedResource(nameof(Resource.SettingToken)),
                            IsMandatory = false,
                        },
                    },
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
            //catch (AdalServiceException asex)
            //{
            //    throw new SecureStoreException(
            //        SecureStoreException.Type.InvalidCredential,
            //        HashicorpVaultUtils.GetLocalizedResource(nameof(Resource.InvalidSecureStoreCredentials)),
            //        asex);
            //}
            //catch (KeyVaultErrorException kvee) when (kvee.Response?.StatusCode == System.Net.HttpStatusCode.Forbidden)
            //{
            //    throw new SecureStoreException(
            //        SecureStoreException.Type.UnauthorizedOperation,
            //        HashicorpVaultUtils.GetLocalizedResource(nameof(Resource.SecureStoreOperationNotAuthorizeded), operation),
            //        kvee);
            //}
            //catch (KeyVaultErrorException kvee) when (kvee.Response?.StatusCode == System.Net.HttpStatusCode.NotFound)
            //{
            //    throw new SecureStoreException(
            //        SecureStoreException.Type.SecretNotFound,
            //        HashicorpVaultUtils.GetLocalizedResource(nameof(Resource.SecureStoreSecretNotFound)),
            //        kvee);
            //}
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
            //catch (AdalServiceException asex)
            //{
            //    throw new SecureStoreException(
            //        SecureStoreException.Type.InvalidCredential,
            //        AzureKeyVaultUtils.GetLocalizedResource(nameof(Resource.InvalidSecureStoreCredentials)),
            //        asex);
            //}
            //catch (KeyVaultErrorException kvee) when (kvee.Response?.StatusCode == System.Net.HttpStatusCode.Forbidden)
            //{
            //    throw new SecureStoreException(
            //        SecureStoreException.Type.UnauthorizedOperation,
            //        AzureKeyVaultUtils.GetLocalizedResource(nameof(Resource.SecureStoreOperationNotAuthorizeded), operation),
            //        kvee);
            //}
            //catch (KeyVaultErrorException kvee) when (kvee.Response?.StatusCode == System.Net.HttpStatusCode.NotFound)
            //{
            //    throw new SecureStoreException(
            //        SecureStoreException.Type.SecretNotFound,
            //        AzureKeyVaultUtils.GetLocalizedResource(nameof(Resource.SecureStoreSecretNotFound)),
            //        kvee);
            //}
            catch (Exception ex)
            {
                throw new SecureStoreException($"Operation {operation} failed.", ex);
            }
        }
    }
}
