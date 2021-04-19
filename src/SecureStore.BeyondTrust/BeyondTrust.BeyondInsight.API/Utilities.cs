using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;

namespace BeyondTrust.BeyondInsight.PasswordSafe.API.Client
{
    public class Utilities
    {

        #region Serialization

        internal static StringContent SerializeContent(object obj)
        {
            string json = JsonConvert.SerializeObject(obj);
            StringContent content = new StringContent(json);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return content;
        }

        internal static T DeserializeContent<T>(string obj)
        {
            T value = JsonConvert.DeserializeObject<T>(obj);
            return value;
        }

        #endregion

        #region Client Certificates

        /// <summary>
        /// Find the BeyondInsight signed client certificate in the LocalMachine certificate store.
        /// </summary>
        /// <returns></returns>
        public static X509Certificate2 FindBICertificate()
        {
            //const string subFieldName = "CN";
            const string issuedTo = "eEyeEmsClient";

            var certs = GetClientCertificates(StoreLocation.LocalMachine);
            certs = certs.Find(X509FindType.FindBySubjectName, issuedTo, true);

            if (certs.Count > 0)
                return certs[0];

            return null;
        }

        /// <summary>
        /// Finds a client certificate for a User Principal Name, i.e. Smart Card Logon for AD account jdoe@doe-main, in the CurrentUser certificate store.
        /// </summary>
        /// <param name="upnName">The UPN Name of the user (i.e. jdoe@doe-main)</param>
        /// <returns></returns>
        public static X509Certificate2 FindCertificateForUPN(string upnName)
        {
            var certs = GetClientCertificates(StoreLocation.CurrentUser);
            foreach (var cert in certs)
            {
                string upn = cert.GetNameInfo(X509NameType.UpnName, false);
                if (string.Equals(upn, upnName, StringComparison.OrdinalIgnoreCase))
                    return cert;
            }

            return null;
        }

        /// <summary>
        /// Returns Client Authentication certificates from the given StoreLocation.
        /// </summary>
        /// <param name="location">The Store Location to search for certificates.</param>
        /// <returns></returns>
        public static X509Certificate2Collection GetClientCertificates(StoreLocation location)
        {
            X509Store store = new X509Store(StoreName.My, location);
            store.Open(OpenFlags.OpenExistingOnly);

            const string ENHANCED_KEY_USAGE_EXTENSION_CLIENT_AUTHENTICATION = "2.5.29.37";
            var certs = store.Certificates.Find(X509FindType.FindByExtension, ENHANCED_KEY_USAGE_EXTENSION_CLIENT_AUTHENTICATION, true);
            return certs;
        }

        #endregion

    }
}
