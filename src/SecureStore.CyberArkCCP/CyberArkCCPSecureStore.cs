using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UiPath.Orchestrator.Extensibility.Configuration;
using UiPath.Orchestrator.Extensibility.SecureStores;
using UiPath.Orchestrator.Extensions.SecureStores.CyberArkCCP.Resources;
using UiPath.Orchestrator.Extensions.SecureStores.CyberArkCCP.Services;

namespace UiPath.Orchestrator.Extensions.SecureStores.CyberArkCCP
{
    public class CyberArkSecureStoreCCP : ISecureStore
    {
        private const string NameIdentifier = "CyberArkCCP";
        private const int PasswordChangeInProgressRetryCount = 4;
        private const int PasswordChangeDelayMS = 1500;

        private readonly HttpClientCache _httpClientCache;

        public CyberArkSecureStoreCCP()
        {
            var x509CertificateManager = new X509CertificateManager();
            _httpClientCache = new HttpClientCache(new HttpClientFactory(x509CertificateManager), x509CertificateManager);
        }

        internal CyberArkSecureStoreCCP(IHttpClientFactory httpClientFactory, X509CertificateManager x509CertificateManager)
        {
            _httpClientCache = new HttpClientCache(httpClientFactory, x509CertificateManager);
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
                new ConfigurationValue(ConfigurationValueType.String)
                {
                    Key = "ClientCertificateThumbprint",
                    DisplayName = SecureStoresUtil.GetLocalizedResource(nameof(Resource.SettingThumbprint)),
                    IsMandatory = false,
                },
                new ConfigurationValue(ConfigurationValueType.String)
                {
                    Key = "CertificateAuthorityThumbprint",
                    DisplayName = SecureStoresUtil.GetLocalizedResource(nameof(Resource.SettingPersonalStoreCAThumbprint)),
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

        private async Task<CyberArkCCPPassword> ReadFromCyberArkCCP(string context, string key)
        {
            SecureStoresUtil.ThrowIfNull(key);
            var ctx = ThrowIfInvalidContext(context);
            Exception normalizationException = null;
            try
            {
                var uri = GetAimServiceUri(key, ctx);
                
                var (cyberArkPassword, content) = await GetCyberArkPasswordAsync(uri,
                    ctx.ClientCertificateThumbprint,
                    ctx.CertificateAuthorityThumbprint);

                if (string.IsNullOrEmpty(cyberArkPassword.Content))
                {
                    var error = JsonConvert.DeserializeObject<CyberArkCCPError>(content);
                    throw new SecureStoreException(
                        SecureStoreException.Type.InvalidConfiguration,
                        $"{error.ErrorCode} - {error.ErrorMsg}",
                        normalizationException);
                }

                return cyberArkPassword;
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

        private async Task<(CyberArkCCPPassword, string)> GetCyberArkPasswordAsync(
            Uri uri,
            string clientCertificateThumbprint,
            string certificateAuthorityThumbprint)
        {
            for (int i = 0; i < PasswordChangeInProgressRetryCount; i++)
            {
                var httpClient = _httpClientCache.GetOrCreateWithCertificate(clientCertificateThumbprint, certificateAuthorityThumbprint);
                
                using (var response = await httpClient.GetAsync(uri))
                {
                    response.EnsureSuccessStatusCode();
                    var content = await response.Content.ReadAsStringAsync();
                    var cyberArkPassword = JsonConvert.DeserializeObject<CyberArkCCPPassword>(content);

                    if (cyberArkPassword.PasswordChangeInProcess != "True")
                    {
                        return (cyberArkPassword, content);
                    }
                }

                await Task.Delay(PasswordChangeDelayMS);
            }
            
            throw new SecureStoreException(
                        SecureStoreException.Type.UnsupportedOperation,
                        SecureStoresUtil.GetLocalizedResource(nameof(Resource.CyberArkPasswordChangeInProgress)));
        }

        private static Uri GetAimServiceUri(string key, CyberArkCCPOptions ctx)
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
            return uri;
        }
    }
}
