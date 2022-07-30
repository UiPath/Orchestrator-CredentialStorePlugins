﻿using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using UiPath.Orchestrator.Extensibility.Configuration;
using UiPath.Orchestrator.Extensibility.SecureStores;

namespace UiPath.Orchestrator.Extensions.SecureStores.BeyondTrust
{
    public class BeyondTrustSingleSystemSecureStore : ISecureStore
    {
        public void Initialize(Dictionary<string, string> hostSettings)
        {
            // No host level settings
        }

        public Task ValidateContextAsync(string context)
        {
            BeyondTrustVaultClientFactory.GetClient(context).TestConnection();
            return Task.CompletedTask;
        }

        public IEnumerable<ConfigurationEntry> GetConfiguration()
        {
            return new List<ConfigurationEntry>
            {
                new ConfigurationValue(ConfigurationValueType.String)
                {
                    Key = ConfigurationConstants.Hostname,
                    DisplayName = "BeyondTrust Host URL",
                    IsMandatory = true,
                },
                new ConfigurationValue(ConfigurationValueType.String)
                {
                    Key = ConfigurationConstants.AuthKey,
                    DisplayName = "API Authentication Key",
                    IsMandatory = true,
                },
                new ConfigurationValue(ConfigurationValueType.String)
                {
                    Key = ConfigurationConstants.RunAs,
                    DisplayName = "API Run As",
                    IsMandatory = true,
                },
                new ConfigurationValue(ConfigurationValueType.Boolean)
                {
                    Key = ConfigurationConstants.SSLEnabled,
                    DisplayName = "Use SSL certificate",
                    IsMandatory = true,
                },
                new ConfigurationValue(ConfigurationValueType.String)
                {
                    Key = ConfigurationConstants.ManagedSystemName,
                    DisplayName = "Managed System Name",
                    IsMandatory = true,
                },
                new ConfigurationValue(ConfigurationValueType.Choice)
                {
                    Key = ConfigurationConstants.ManagedAccountType,
                    DisplayName = "Managed Account Type",
                    IsMandatory = true,
                    PossibleValues = new List<string> { ConfigurationConstants.SystemAccountType, ConfigurationConstants.DomainAccountType }
                },
            };
        }

        public SecureStoreInfo GetStoreInfo()
        {
            return new SecureStoreInfo { Identifier = "BeyondTrust-SingleSystem-ReadOnly", IsReadOnly = true };
        }

        public async Task<string> CreateCredentialsAsync(string context, string key, Credential value)
        {
            throw new SecureStoreException(SecureStoreException.Type.UnsupportedOperation, "This secure store is read-only");
        }

        public async Task<Credential> GetCredentialsAsync(string context, string key)
        {
            var config = JsonConvert.DeserializeObject<Dictionary<string, object>>(context);
            var client = BeyondTrustVaultClientFactory.GetClient(context);
            var semaphore = SemaphoreFactory.SemaphoreSlim;
            await semaphore.WaitAsync();
            try
            {
                try
                {
                    client.SignIn();
                    var managedAccountResult = client.ManagedAccounts.GetRequestable(config[ConfigurationConstants.ManagedSystemName].ToString(), key, config[ConfigurationConstants.ManagedAccountType].ToString());
                    if (!managedAccountResult.IsSuccess)
                    {
                        if (managedAccountResult.StatusCode.Equals(HttpStatusCode.NotFound))
                        {
                            throw new SecureStoreException(SecureStoreException.Type.UnsupportedOperation, "Managed Account not found");
                        }
                        else
                        {
                            throw new SecureStoreException(SecureStoreException.Type.UnsupportedOperation, "Managed account retreival failed");
                        }
                    }

                    var isaRequestResult = client.ISARequests.Post(managedAccountResult.Value.AccountId, managedAccountResult.Value.SystemId, 1, "UiPath Credential Store request");
                    if (!isaRequestResult.IsSuccess)
                    {
                        throw new SecureStoreException(SecureStoreException.Type.UnsupportedOperation, "ISA request failed with code " + isaRequestResult.StatusCode + " and message: " + isaRequestResult.Message);
                    }
                    return new Credential { Username = managedAccountResult.Value.AccountName, Password = isaRequestResult.Value };
                }
                finally
                {
                    client.SignOut();
                }
            }
            finally
            {
                semaphore.Release();
            }
        }

        public async Task<string> UpdateCredentialsAsync(string context, string key, string oldAugumentedKey, Credential value)
        {
            throw new SecureStoreException(SecureStoreException.Type.UnsupportedOperation, "This secure store is read-only");
        }

        public async Task<string> CreateValueAsync(string context, string key, string value)
        {
            throw new SecureStoreException(SecureStoreException.Type.UnsupportedOperation, "This secure store is read-only");
        }

        public async Task<string> GetValueAsync(string context, string key)
        {
            Credential credential = await GetCredentialsAsync(context, key);
            return credential.Password;
        }

        public Task<string> UpdateValueAsync(string context, string key, string oldAugumentedKey, string value)
        {
            throw new SecureStoreException(SecureStoreException.Type.UnsupportedOperation, "This secure store is read-only");
        }

        public async Task RemoveValueAsync(string context, string key)
        {
            throw new SecureStoreException(SecureStoreException.Type.UnsupportedOperation, "This secure store is read-only");
        }
    }
}
