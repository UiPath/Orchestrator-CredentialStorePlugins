using System.Linq;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace UiPath.Orchestrator.Extensions.SecureStores.CyberArkCCP.Services
{
    internal class HttpClientFactory : IHttpClientFactory
    {
        private readonly X509CertificateManager _x509CertificateManager;

        private string _certificateAuthorityThumbprint;

        public HttpClientFactory(X509CertificateManager x509CertificateManager)
        {
            _x509CertificateManager = x509CertificateManager;
        }

        public HttpClient CreateWithCertificate(X509Certificate2 clientCertificate, string certificateAuthorityThumbprint)
        {
            var httpHandler = new HttpClientHandler();
            if (clientCertificate != null)
            {
                httpHandler.ClientCertificates.Add(clientCertificate);
            }

            if (!string.IsNullOrWhiteSpace(certificateAuthorityThumbprint))
            {
                _certificateAuthorityThumbprint = certificateAuthorityThumbprint;
                httpHandler.ServerCertificateCustomValidationCallback = ServerCertificatePersonalStoreValidation;
            }

            return new HttpClient(httpHandler, disposeHandler: true);
        }

        /// <summary>
        /// In Azure PAAS deployments only personal store can be used to hold certificates.
        /// If a self-signed certificate is used then the Root certificate authority used to sign that certificated
        /// can not be imported in the Root CA store thus the ssl certificate can not be validated.
        /// This method will check if the self-signed Root CA is present in the personal store and will validate
        /// the server certificate only if policy errors are regarded as X509ChainStatusFlags.UntrustedRoot.
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <param name="certificate"></param>
        /// <param name="chain"></param>
        /// <param name="sslPolicyErrors"></param>
        /// <returns></returns>

        private bool ServerCertificatePersonalStoreValidation(
            HttpRequestMessage requestMessage,
            X509Certificate2 certificate,
            X509Chain chain,
            SslPolicyErrors sslPolicyErrors)
        {
            // If there aren't any ssl errors then the certificate is valid.
            if (sslPolicyErrors == SslPolicyErrors.None
                && chain.ChainStatus.All(s => s.Status == X509ChainStatusFlags.NoError))
            {
                return true;
            }

            // If there are other errors apart from untrusted root certificate authority
            // then ssl certificate is regarded as invalid.
            if (chain.ChainStatus.Any(s => s.Status != X509ChainStatusFlags.UntrustedRoot && s.Status != X509ChainStatusFlags.NoError))
            {
                return false;
            }

            var validRootCertificate = _x509CertificateManager.GetPublicCertificate(_certificateAuthorityThumbprint);
            
            // For all chained certificates that are marked as untrusted root check if they exist in the personal store.
            return chain.ChainElements.Cast<X509ChainElement>()
                .All(e => !e.ChainElementStatus.Any()
                          || (e.ChainElementStatus.All(s => s.Status == X509ChainStatusFlags.UntrustedRoot)
                              && validRootCertificate.Thumbprint == e.Certificate.Thumbprint));
        }
    }
}
