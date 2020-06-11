using System;
using System.Net.Http;
using Microsoft.Extensions.Caching.Memory;

namespace UiPath.Orchestrator.Extensions.SecureStores.CyberArkCCP.Services
{
    internal class HttpClientCache
    {
        private readonly MemoryCache _clients;
        private readonly MemoryCacheOptions _cacheOptions;
        private readonly TimeSpan _httpClientExpiration = TimeSpan.FromMinutes(10);

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly X509CertificateManager _x509CertificateManager;

        public HttpClientCache(IHttpClientFactory httpClientFactory, X509CertificateManager x509CertificateManager)
        {
            _httpClientFactory = httpClientFactory;
            _x509CertificateManager = x509CertificateManager;
            
            _cacheOptions = new MemoryCacheOptions
            {
                SizeLimit = 500,
            };
            _clients = new MemoryCache(_cacheOptions);
        }


        public HttpClient GetOrCreateWithCertificate(string clientCertificateThumbprint, string certificateAuthorityThumbprint)
        {
            // Check if the client certificates exists before each use of the http client
            // If the client does not exist GetPrivateCertificate method will throw an exception.
            var clientCertifcate = _x509CertificateManager.GetPrivateCertificate(clientCertificateThumbprint);

            return _clients.GetOrCreate(clientCertificateThumbprint ?? string.Empty + certificateAuthorityThumbprint, e =>
            {
                e.Size = 1;
                e.SlidingExpiration = _httpClientExpiration;
                e.RegisterPostEvictionCallback((k, v, r, s) =>
                {
                    // This delegate will be called when the http client has not been used for 10 minutes.
                    if (v is IDisposable disposable)
                    {
                        try
                        {
                            disposable.Dispose();
                        }
                        catch { }
                    }
                });

                return _httpClientFactory.CreateWithCertificate(clientCertifcate, certificateAuthorityThumbprint);
            });
        }
    }
}
