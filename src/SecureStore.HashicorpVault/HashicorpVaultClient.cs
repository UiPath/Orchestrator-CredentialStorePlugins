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
using VaultSharp.V1.SecretsEngines;

namespace UiPath.Orchestrator.Extensions.SecureStores.HashicorpVault
{
    public class HashicorpVaultClient : IHashicorpVaultClient
    {
        private readonly HashicorpVaultContext _context;
        private readonly Lazy<IVaultClient> _vaultClient;

        public HashicorpVaultClient(HashicorpVaultContext context)
        {
            _context = context;
            _vaultClient = new Lazy<IVaultClient>(GetVaultClient);
        }

        public async Task<string> GetSecretAsync(string secretName)
        {
            var vaultClient = _vaultClient.Value;
            var path = _context.DataPath + "/" + secretName;
            switch (_context.SecretsEngine)
            {
                case SecretsEngine.KeyValueV1:
                    var kvv1Secret = await vaultClient.V1.Secrets.KeyValue.V1.ReadSecretAsync<Dictionary<string, string>>(path, mountPoint: _context.SecretsEnginePath ?? SecretsEngineDefaultPaths.KeyValueV1);
                    return kvv1Secret.Data[secretName];
                case SecretsEngine.KeyValueV2:
                    var kvv2Secret = await vaultClient.V1.Secrets.KeyValue.V2.ReadSecretAsync<Dictionary<string, string>>(path, mountPoint: _context.SecretsEnginePath ?? SecretsEngineDefaultPaths.KeyValueV2);
                    return kvv2Secret.Data.Data[secretName];
                case SecretsEngine.Cubbyhole:
                    var cubbySecret = await vaultClient.V1.Secrets.Cubbyhole.ReadSecretAsync(path);
                    return cubbySecret.Data[secretName].ToString();
                default:
                    throw new NotSupportedException($"Secrets engine '{_context.SecretsEngine}' is not supported.");
            }
        }

        public async Task<string> SetSecretAsync(string secretName, string secretValue)
        {
            var vaultClient = _vaultClient.Value;
            var path = _context.DataPath + "/" + secretName;
            var secretToSave = new Dictionary<string, object> { [secretName] = secretValue };
            switch (_context.SecretsEngine)
            {
                case SecretsEngine.KeyValueV1:
                    await vaultClient.V1.Secrets.KeyValue.V1.WriteSecretAsync(path, secretToSave, mountPoint: _context.SecretsEnginePath ?? SecretsEngineDefaultPaths.KeyValueV1);
                    return secretName;
                case SecretsEngine.KeyValueV2:
                    await vaultClient.V1.Secrets.KeyValue.V2.WriteSecretAsync(path, secretToSave, mountPoint: _context.SecretsEnginePath ?? SecretsEngineDefaultPaths.KeyValueV2);
                    return secretName;
                case SecretsEngine.ActiveDirectory:
                    throw new NotSupportedException("Writing secrets is not supported for Engine ActiveDirectory");
                case SecretsEngine.Cubbyhole:
                    await vaultClient.V1.Secrets.Cubbyhole.WriteSecretAsync(path, secretToSave);
                    return secretName;
                default:
                    throw new NotSupportedException($"Secrets engine '{_context.SecretsEngine}' is not supported.");
            }
        }

        public async Task DeleteSecretAsync(string secretName, bool destroy = false)
        {
            var vaultClient = _vaultClient.Value;
            var path = _context.DataPath + "/" + secretName;
            switch (_context.SecretsEngine)
            {
                case SecretsEngine.KeyValueV1:
                    await vaultClient.V1.Secrets.KeyValue.V1.DeleteSecretAsync(path, mountPoint: _context.SecretsEnginePath ?? SecretsEngineDefaultPaths.KeyValueV1);
                    break;
                case SecretsEngine.KeyValueV2:
                    if (destroy)
                    {
                        await vaultClient.V1.Secrets.KeyValue.V2.DeleteMetadataAsync(path, mountPoint: _context.SecretsEnginePath ?? SecretsEngineDefaultPaths.KeyValueV2);
                    }
                    else
                    {
                        await vaultClient.V1.Secrets.KeyValue.V2.DeleteSecretAsync(path, mountPoint: _context.SecretsEnginePath ?? SecretsEngineDefaultPaths.KeyValueV2);
                    }

                    break;
                case SecretsEngine.ActiveDirectory:
                    throw new NotSupportedException("Writing secrets is not supported for Engine ActiveDirectory");
                case SecretsEngine.Cubbyhole:
                    await vaultClient.V1.Secrets.Cubbyhole.DeleteSecretAsync(path);
                    break;
                default:
                    throw new NotSupportedException($"Secrets engine '{_context.SecretsEngine}' is not supported.");
            }
        }

        public async Task<Credential> GetCredentialAsync(string secretName)
        {
            var vaultClient = _vaultClient.Value;
            var path = _context.DataPath + "/" + secretName;
            switch (_context.SecretsEngine)
            {
                case SecretsEngine.KeyValueV1:
                    var kvv1Secret = await vaultClient.V1.Secrets.KeyValue.V1.ReadSecretAsync<Credential>(path, mountPoint: _context.SecretsEnginePath ?? SecretsEngineDefaultPaths.KeyValueV1);
                    return kvv1Secret?.Data;
                case SecretsEngine.KeyValueV2:
                    var kvv2Secret = await vaultClient.V1.Secrets.KeyValue.V2.ReadSecretAsync<Credential>(path, mountPoint: _context.SecretsEnginePath ?? SecretsEngineDefaultPaths.KeyValueV2);
                    return kvv2Secret?.Data.Data;
                case SecretsEngine.ActiveDirectory:
                    var adSecret = await vaultClient.V1.Secrets.ActiveDirectory.GetCredentialsAsync(secretName, mountPoint: _context.SecretsEnginePath ?? SecretsEngineDefaultPaths.ActiveDirectory);
                    return new Credential
                    {
                        Username = adSecret.Data.Username,
                        Password = adSecret.Data.CurrentPassword,
                    };
                case SecretsEngine.Cubbyhole:
                    var cubbySecret = await vaultClient.V1.Secrets.Cubbyhole.ReadSecretAsync(path);
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
            var vaultClient = _vaultClient.Value;
            var path = _context.DataPath + "/" + secretName;
            switch (_context.SecretsEngine)
            {
                case SecretsEngine.KeyValueV1:
                    await vaultClient.V1.Secrets.KeyValue.V1.WriteSecretAsync(path, credential, mountPoint: _context.SecretsEnginePath ?? SecretsEngineDefaultPaths.KeyValueV1);
                    return secretName;
                case SecretsEngine.KeyValueV2:
                    await vaultClient.V1.Secrets.KeyValue.V2.WriteSecretAsync(path, credential, mountPoint: _context.SecretsEnginePath ?? SecretsEngineDefaultPaths.KeyValueV2);
                    return secretName;
                case SecretsEngine.ActiveDirectory:
                    throw new NotSupportedException("Writing secrets is not supported for Engine ActiveDirectory");
                case SecretsEngine.Cubbyhole:
                    var secretToSave = new Dictionary<string, object>
                    {
                        ["Username"] = credential.Username,
                        ["Password"] = credential.Password,
                    };
                    await vaultClient.V1.Secrets.Cubbyhole.WriteSecretAsync(path, secretToSave);
                    return secretName;
                default:
                    throw new NotSupportedException($"Secrets engine '{_context.SecretsEngine}' is not supported.");
            }
        }

        public Task DeleteCredentialAsync(string secretName, bool destroy = false) => DeleteSecretAsync(secretName, destroy);

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