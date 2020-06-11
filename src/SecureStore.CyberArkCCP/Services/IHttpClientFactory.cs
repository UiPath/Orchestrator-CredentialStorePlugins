using System.Net.Http;
using System.Security.Cryptography.X509Certificates;

namespace UiPath.Orchestrator.Extensions.SecureStores.CyberArkCCP.Services
{
    internal interface IHttpClientFactory
    {
        HttpClient CreateWithCertificate(X509Certificate2 clientCertificate, string certificateAuthorityThumbprint);
    }
}
