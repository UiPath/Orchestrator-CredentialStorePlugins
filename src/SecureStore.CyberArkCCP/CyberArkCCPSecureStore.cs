using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using UiPath.Orchestrator.Extensibility.Configuration;
using UiPath.Orchestrator.Extensibility.SecureStores;
using UiPath.Orchestrator.Extensions.SecureStores.CyberArkCCP.Resources;

namespace UiPath.Orchestrator.Extensions.SecureStores.CyberArkCCP
{
    public class CyberArkSecureStoreCCP : ISecureStore
    {
        private const string NameIdentifier = "CyberArkCCP";
        private readonly HttpClientHandler _httpClientHandler;
        private readonly HttpClient _httpClient;
        private string _thumbprint;

        public CyberArkSecureStoreCCP()
        {
            _httpClientHandler = new HttpClientHandler();

            //this will check if the root cert is in the personal store as you don't have access in the Root store in Azure Web App
            _httpClientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) =>
            {
                //Console.WriteLine($"sslPolicyErrors: {sslPolicyErrors}, cert.Verify: {cert.Verify()}");
                if (!cert.Verify() && sslPolicyErrors == System.Net.Security.SslPolicyErrors.RemoteCertificateChainErrors)
                {
                    using (X509Store currentUserStore = new X509Store(StoreName.My, StoreLocation.CurrentUser))
                    {
                        currentUserStore.Open(OpenFlags.ReadOnly);
                        foreach (var storeCertificate in currentUserStore.Certificates)
                        {
                            foreach (var chainElement in chain.ChainElements)
                            {
                                if (storeCertificate.Thumbprint == chainElement.Certificate.Thumbprint)
                                {
                                    currentUserStore.Close();
                                    return true;
                                }
                            }
                        }
                        currentUserStore.Close();
                    }
                }
                return false;
            };
            _httpClient = new HttpClient(_httpClientHandler);
        }

        public CyberArkSecureStoreCCP(HttpClientHandler httpClientHandler, HttpClient httpClient)
        {
            _httpClientHandler = httpClientHandler;
            _httpClient = httpClient;
        }

        public SecureStoreInfo GetStoreInfo() =>
            new SecureStoreInfo { Identifier = NameIdentifier, IsReadOnly = true };

        public async Task<string> GetValueAsync(string context, string key)
        {
            CyberArkCCPPassword value = await ReadFromCyberArkCCP(context, key);
            return value.Content;
        }

        public async Task<Credential> GetCredentialsAsync(string context, string key)
        {
            CyberArkCCPPassword value = await ReadFromCyberArkCCP(context, key);

            return new Credential
            {
                Username = value.Name,
                Password = value.Content,
            };
        }

        public Task RemoveValueAsync(string context, string key) =>
            throw new SecureStoreException(
                SecureStoreException.Type.UnsupportedOperation,
                SecureStoresUtil.GetLocalizedResource(nameof(Resource.CyberArkReadOnly)));

        public Task<string> CreateValueAsync(string context, string key, string value) =>
            throw new SecureStoreException(
                SecureStoreException.Type.UnsupportedOperation,
                SecureStoresUtil.GetLocalizedResource(nameof(Resource.CyberArkReadOnly)));

        public Task<string> CreateCredentialsAsync(string context, string key, Credential value) =>
            throw new SecureStoreException(
                SecureStoreException.Type.UnsupportedOperation,
                SecureStoresUtil.GetLocalizedResource(nameof(Resource.CyberArkReadOnly)));

        public Task<string> UpdateValueAsync(string context, string key, string oldAugumentedKey, string value) =>
            throw new SecureStoreException(
                SecureStoreException.Type.UnsupportedOperation,
                SecureStoresUtil.GetLocalizedResource(nameof(Resource.CyberArkReadOnly)));

        public Task<string> UpdateCredentialsAsync(string context, string key, string oldAugumentedKey, Credential value) =>
            throw new SecureStoreException(
                SecureStoreException.Type.UnsupportedOperation,
                SecureStoresUtil.GetLocalizedResource(nameof(Resource.CyberArkReadOnly)));

        public IEnumerable<ConfigurationEntry> GetConfiguration()
        {
            return new List<ConfigurationEntry>
            {
                new ConfigurationValue(ConfigurationValueType.String)
                {
                    Key = "URL",
                    DisplayName = SecureStoresUtil.GetLocalizedResource(nameof(Resource.SettingURL)),
                    IsMandatory = true,
                },
                new ConfigurationValue(ConfigurationValueType.String)
                {
                    Key = "Thumbprint",
                    DisplayName = SecureStoresUtil.GetLocalizedResource(nameof(Resource.SettingThumbprint)),
                    IsMandatory = false,
                },
                new ConfigurationValue(ConfigurationValueType.String)
                {
                    Key = "ApplicationId",
                    DisplayName = SecureStoresUtil.GetLocalizedResource(nameof(Resource.SettingNameApplicationID)),
                    IsMandatory = true,
                },
                new ConfigurationValue(ConfigurationValueType.String)
                {
                    Key = "Safe",
                    DisplayName = SecureStoresUtil.GetLocalizedResource(nameof(Resource.SettingNameSafe)),
                    IsMandatory = true,
                },
                new ConfigurationValue(ConfigurationValueType.String)
                {
                    Key = "Folder",
                    DisplayName = SecureStoresUtil.GetLocalizedResource(nameof(Resource.SettingNameFolder)),
                    IsMandatory = false,
                },
            };
        }

        public void Initialize(Dictionary<string, string> hostSettings)
        {
            // No-op : current implementation of CyberArkCCP does not have a host level configuration
        }

        public Task ValidateContextAsync(string context)
        {
            /// just check the required configurations
            ThrowIfInvalidContext(context);

            return Task.CompletedTask;
        }

        private static CyberArkCCPOptions ThrowIfInvalidContext(string context)
        {
            Exception normalizationException = null;
            if (context != null)
            {
                try
                {
                    var ctx = JsonConvert.DeserializeObject<CyberArkCCPOptions>(context);

                    if (!string.IsNullOrWhiteSpace(ctx.ApplicationId) &&
                        !string.IsNullOrWhiteSpace(ctx.Safe) &&
                        !string.IsNullOrWhiteSpace(ctx.URL) &&
                        Uri.IsWellFormedUriString(ctx.URL, UriKind.Absolute))
                    {
                        return ctx;
                    }
                }
                catch (Exception ex)
                {
                    normalizationException = ex;
                }
            }

            throw new SecureStoreException(
                    SecureStoreException.Type.InvalidConfiguration,
                    SecureStoresUtil.GetLocalizedResource(nameof(Resource.SecureStore)),
                    normalizationException);
        }

        private Task<CyberArkCCPPassword> ReadFromCyberArkCCP(string context, string key)
        {
            SecureStoresUtil.ThrowIfNull(key);
            var ctx = ThrowIfInvalidContext(context);

            var certCollection = new X509Certificate2Collection();
            if (_thumbprint != ctx.Thumbprint)
            {
                _httpClientHandler.ClientCertificates.Clear();
            }

            if (_httpClientHandler.ClientCertificates.Count == 0 && !string.IsNullOrEmpty(ctx.Thumbprint))
            {
                certCollection = GetCertificateForCCPAuth(ctx.Thumbprint);
                Exception normalizationException = null;

                if (certCollection.Count != 1)
                {
                    throw new SecureStoreException(
                        SecureStoreException.Type.InvalidConfiguration,
                        SecureStoresUtil.GetLocalizedResource(nameof(Resource.SecureStoreCert)),
                        normalizationException);
                }
                if (!certCollection[0].HasPrivateKey)
                {
                    throw new SecureStoreException(
                        SecureStoreException.Type.InvalidConfiguration,
                        SecureStoresUtil.GetLocalizedResource(nameof(Resource.SecureStoreCert)),
                        normalizationException);
                }
                _httpClientHandler.ClientCertificates.Add(certCollection[0]);
                _thumbprint = ctx.Thumbprint;
            }
            return GetPasswordFromCCP(ctx, key);
        }

        private async Task<CyberArkCCPPassword> GetPasswordFromCCP(CyberArkCCPOptions ctx, string key)
        {
            Exception normalizationException = null;
            try
            {
                string obj;
                key = key.Replace('\\', '-');
                if (string.IsNullOrEmpty(ctx.Folder))
                {
                    obj = $"{key}";
                }
                else
                {
                    obj = $"{ctx.Folder}\\{key}";
                }

                var uri = new Uri(ctx.URL + $"/AIMWebService/api/Accounts?appid={ctx.ApplicationId}&safe={ctx.Safe}&object={obj}");
                var response = await _httpClient.GetAsync(uri);
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<CyberArkCCPPassword>(content);

                if (!string.IsNullOrEmpty(result.PasswordChangeInProcess))
                {
                    if (result.PasswordChangeInProcess == "True")
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            response = await _httpClient.GetAsync(uri);
                            content = await response.Content.ReadAsStringAsync();
                            result = JsonConvert.DeserializeObject<CyberArkCCPPassword>(content);
                            if (result.PasswordChangeInProcess == "False")
                            {
                                break;
                            }
                            await Task.Delay(1500);
                        }
                    }
                }

                if (string.IsNullOrEmpty(result.Content))
                {
                    var error = JsonConvert.DeserializeObject<CyberArkCCPError>(content);
                    throw new SecureStoreException(
                    SecureStoreException.Type.InvalidConfiguration,
                    $"{error.ErrorCode} - {error.ErrorMsg}",
                    normalizationException);
                }

                return result;
            }
            catch (Exception ex)
            {
                normalizationException = ex;
            }

            throw new SecureStoreException(
                    SecureStoreException.Type.InvalidConfiguration,
                    SecureStoresUtil.GetLocalizedResource(nameof(Resource.InvalidSecureStoreContext)),
                    normalizationException);
        }

        private X509Certificate2Collection GetCertificateForCCPAuth(string thumbprint)
        {
            Exception normalizationException = null;

            var certCollection = new X509Certificate2Collection();
            try
            {
                using (X509Store localMachineStore = new X509Store(StoreName.My, StoreLocation.LocalMachine))
                {
                    localMachineStore.Open(OpenFlags.ReadOnly);
                    //`validOnly` arg is set to false so it can work with self-signed without CA certs
                    certCollection = localMachineStore.Certificates.Find(X509FindType.FindByThumbprint, thumbprint, false);
                    localMachineStore.Close();
                }
                if (certCollection.Count == 0)
                {
                    using (X509Store currentUserStore = new X509Store(StoreName.My, StoreLocation.CurrentUser))
                    {
                        currentUserStore.Open(OpenFlags.ReadOnly);
                        //`validOnly` arg is set to false so it can work with self-signed without CA certs
                        certCollection = currentUserStore.Certificates.Find(X509FindType.FindByThumbprint, thumbprint, false);
                        currentUserStore.Close();
                    }
                }
                return certCollection;
            }
            catch (Exception ex)
            {
                normalizationException = ex;
            }

            throw new SecureStoreException(
                    SecureStoreException.Type.InvalidConfiguration,
                    SecureStoresUtil.GetLocalizedResource(nameof(Resource.SecureStoreCert)),
                    normalizationException);
        }
    }
}
