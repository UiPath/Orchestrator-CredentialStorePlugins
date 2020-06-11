using System;
using System.Security.Cryptography.X509Certificates;
using UiPath.Orchestrator.Extensibility.SecureStores;
using UiPath.Orchestrator.Extensions.SecureStores.CyberArkCCP.Resources;

namespace UiPath.Orchestrator.Extensions.SecureStores.CyberArkCCP.Services
{
    internal class X509CertificateManager
    {
        public X509CertificateManager()
        {

        }

        public X509Certificate2 GetPrivateCertificate(string thumbprint)
        {
            var certificate = GetPublicCertificate(thumbprint);

            if (certificate?.HasPrivateKey == false)
            {
                throw new SecureStoreException(
                    SecureStoreException.Type.InvalidConfiguration,
                    SecureStoresUtil.GetLocalizedResource(nameof(Resource.SecureStoreCert)));
            }

            return certificate;
        }

        public X509Certificate2 GetPublicCertificate(string thumbprint)
        {
            if (string.IsNullOrWhiteSpace(thumbprint))
            {
                return null;
            }

            var certCollection = GetCertificateCollection(thumbprint);

            if (certCollection.Count != 1)
            {
                throw new SecureStoreException(
                    SecureStoreException.Type.InvalidConfiguration,
                    SecureStoresUtil.GetLocalizedResource(nameof(Resource.SecureStoreCert)));
            }

            return certCollection[0];
        }

        private X509Certificate2Collection GetCertificateCollection(string thumbprint)
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
