using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UiPath.Orchestrator.Extensibility.SecureStores;
using VaultSharp;
using VaultSharp.V1.AuthMethods;
using VaultSharp.V1.AuthMethods.AppRole;
using VaultSharp.V1.AuthMethods.LDAP;
using VaultSharp.V1.AuthMethods.Token;
using VaultSharp.V1.AuthMethods.UserPass;

namespace UiPath.Orchestrator.Extensions.SecureStores.HashicorpVault
{
    public class HashicorpVaultClient : IHashicorpVaultClient
    {
        private readonly HashicorpVaultContext _context;

        public HashicorpVaultClient(HashicorpVaultContext context)
        {
            _context = context;
        }

        public async Task<string> GetSecretAsync(string secretName)
        {
            var keyVaultClient = GetVaultClient();
            switch (_context.SecretsEngine)
            {
                case SecretsEngine.KeyValueV1:
                    var kvv1Secret = await keyVaultClient.V1.Secrets.KeyValue.V1.ReadSecretAsync<string>(secretName);
                    return kvv1Secret.Data;
                case SecretsEngine.KeyValueV2:
                    var kvv2Secret = await keyVaultClient.V1.Secrets.KeyValue.V2.ReadSecretAsync<string>(secretName);
                    return kvv2Secret.Data.Data;
                case SecretsEngine.Cubbyhole:
                    var cubbySecret = await keyVaultClient.V1.Secrets.Cubbyhole.ReadSecretAsync(secretName);
                    return cubbySecret.Data[secretName].ToString();
                default:
                    throw new NotSupportedException($"Secrets engine '{_context.SecretsEngine}' is not supported.");
            }
        }

        public async Task<string> SetSecretAsync(string secretName, string secretValue)
        {
            var keyVaultClient = GetVaultClient();
            switch (_context.SecretsEngine)
            {
                case SecretsEngine.KeyValueV1:
                    var kvv1Secret = await keyVaultClient.V1.Secrets.KeyValue.V1.WriteSecretAsync(secretName, secretValue);
                    return kvv1Secret?.WrapInfo.CreationPath;
                case SecretsEngine.KeyValueV2:
                    var kvv2Secret = await keyVaultClient.V1.Secrets.KeyValue.V2.WriteSecretAsync(secretName, secretValue);
                    return kvv2Secret?.WrapInfo.CreationPath;
                case SecretsEngine.ActiveDirectory:
                    throw new NotSupportedException("Writing secrets is not supported for Engine ActiveDirectory");
                case SecretsEngine.Cubbyhole:
                    var secretToSave = new Dictionary<string, object> { [secretName] = secretValue };
                    await keyVaultClient.V1.Secrets.Cubbyhole.WriteSecretAsync(secretName, secretToSave);
                    return secretName;
                default:
                    throw new NotSupportedException($"Secrets engine '{_context.SecretsEngine}' is not supported.");
            }
        }

        public async Task DeleteSecretAsync(string secretName)
        {
            var keyVaultClient = GetVaultClient();
            switch (_context.SecretsEngine)
            {
                case SecretsEngine.KeyValueV1:
                    await keyVaultClient.V1.Secrets.KeyValue.V1.DeleteSecretAsync(secretName);
                    break;
                case SecretsEngine.KeyValueV2:
                    await keyVaultClient.V1.Secrets.KeyValue.V2.DeleteSecretAsync(secretName);
                    break;
                case SecretsEngine.ActiveDirectory:
                    throw new NotSupportedException("Writing secrets is not supported for Engine ActiveDirectory");
                case SecretsEngine.Cubbyhole:
                    await keyVaultClient.V1.Secrets.Cubbyhole.DeleteSecretAsync(secretName);
                    break;
                default:
                    throw new NotSupportedException($"Secrets engine '{_context.SecretsEngine}' is not supported.");
            }
        }

        public async Task<Credential> GetCredentialAsync(string secretName)
        {
            var keyVaultClient = GetVaultClient();
            switch (_context.SecretsEngine)
            {
                case SecretsEngine.KeyValueV1:
                    var kvv1Secret = await keyVaultClient.V1.Secrets.KeyValue.V1.ReadSecretAsync<Credential>(secretName);
                    return kvv1Secret?.Data;
                case SecretsEngine.KeyValueV2:
                    var kvv2Secret = await keyVaultClient.V1.Secrets.KeyValue.V2.ReadSecretAsync<Credential>(secretName);
                    return kvv2Secret?.Data.Data;
                case SecretsEngine.ActiveDirectory:
                    var adSecret = await keyVaultClient.V1.Secrets.ActiveDirectory.GetCredentialsAsync(secretName);
                    return new Credential
                    {
                        Username = adSecret.Data.Username,
                        Password = adSecret.Data.CurrentPassword,
                    };
                case SecretsEngine.Cubbyhole:
                    var cubbySecret = await keyVaultClient.V1.Secrets.Cubbyhole.ReadSecretAsync(secretName);
                    return new Credential
                    {
                        Username = cubbySecret.Data["Username"].ToString(),
                        Password = cubbySecret.Data["Password"].ToString(),
                    };
                default:
                    throw new NotSupportedException($"Secrets engine '{_context.SecretsEngine}' is not supported.");
            }
        }

        public async Task<string> SetCredentialAsync(string secretName, Credential credential)
        {
            var keyVaultClient = GetVaultClient();
            switch (_context.SecretsEngine)
            {
                case SecretsEngine.KeyValueV1:
                    await keyVaultClient.V1.Secrets.KeyValue.V1.WriteSecretAsync(secretName, credential);
                    return secretName;
                case SecretsEngine.KeyValueV2:
                    await keyVaultClient.V1.Secrets.KeyValue.V2.WriteSecretAsync(secretName, credential);
                    return secretName;
                case SecretsEngine.ActiveDirectory:
                    throw new NotSupportedException("Writing secrets is not supported for Engine ActiveDirectory");
                case SecretsEngine.Cubbyhole:
                    var secretToSave = new Dictionary<string, object>
                    {
                        ["Username"] = credential.Username,
                        ["Password"] = credential.Password,
                    };
                    await keyVaultClient.V1.Secrets.Cubbyhole.WriteSecretAsync(secretName, secretToSave);
                    return secretName;
                default:
                    throw new NotSupportedException($"Secrets engine '{_context.SecretsEngine}' is not supported.");
            }
        }

        public Task DeleteCredentialAsync(string secretName) => DeleteSecretAsync(secretName);

        private IVaultClient GetVaultClient()
        {
            IAuthMethodInfo authMethod;

            switch (_context.AuthenticationType)
            {
                case AuthenticationType.AppRole:
                    authMethod = new AppRoleAuthMethodInfo(_context.RoleId, _context.SecretId);
                    break;
                case AuthenticationType.UsernamePassword:
                    authMethod = new UserPassAuthMethodInfo(_context.Username, _context.Password);
                    break;
                case AuthenticationType.Ldap:
                    authMethod = new LDAPAuthMethodInfo(_context.Username, _context.Password);
                    break;
                case AuthenticationType.Token:
                    authMethod = new TokenAuthMethodInfo(_context.Token);
                    break;
                default:
                    throw new NotSupportedException($"Authentication type '{_context.AuthenticationType}' is not supported.");
            }

            var vaultClientSettings = new VaultClientSettings(_context.VaultUri.ToString(), authMethod)
            {
                VaultServiceTimeout = TimeSpan.FromSeconds(30)
            };

            if (!string.IsNullOrWhiteSpace(_context.Namespace))
            {
                vaultClientSettings.Namespace = _context.Namespace;
            }

            return new VaultClient(vaultClientSettings);
        }
    }
}