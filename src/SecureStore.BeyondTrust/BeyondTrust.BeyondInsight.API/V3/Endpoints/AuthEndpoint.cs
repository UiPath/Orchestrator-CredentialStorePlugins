using System.Net.Http;
using System.Security.Cryptography.X509Certificates;

namespace BeyondTrust.BeyondInsight.PasswordSafe.API.Client.V3
{
    public sealed class AuthEndpoint : BaseEndpoint
    {
        internal AuthEndpoint(PasswordSafeAPIConnector client)
            : base(client)
        {
        }

        /// <summary>
        /// The model of the authenticated user.  If IsAuthenticated is false, returns null.
        /// </summary>
        public SignAppInUserModel User
        {
            get { return _conn.User; }
            internal set { _conn.User = value; }
        }

        /// <summary>
        /// Returns whether the client is authenticated with the server.
        /// </summary>
        public bool IsAuthenticated
        {
            get { return User != null; }
        }

        /// <summary>
        /// Authenticates with the server using the given Application API Key and username.
        /// <para>API: POST Auth/SignAppin</para>
        /// </summary>
        /// <param name="apiKey">The Application API Key.</param>
        /// <param name="username">The username of the user.</param>
        /// <returns></returns>
        public AuthenticationResult SignAppIn(string apiKey, string username)
        {
            return SignAppIn(apiKey, username, null, false);
        }

        /// <summary>
        /// Authenticates with the server using the given Application API Key, username, and client certificate.
        /// <para>API: POST Auth/SignAppin</para>
        /// </summary>
        /// <param name="apiKey">The Application API Key.</param>
        /// <param name="username">The username of the user.</param>
        /// <param name="clientCert">The client certificate to use for authentication.</param>
        /// <returns></returns>
        public AuthenticationResult SignAppIn(string apiKey, string username, X509Certificate2 clientCert)
        {
            return SignAppIn(apiKey, username, clientCert, false);
        }

        /// <summary>
        /// Authenticates with the server using the given Application API Key and username, optionally ignoring any SSL warnings.
        /// <para>API: POST Auth/SignAppin</para>
        /// </summary>
        /// <param name="apiKey">The Application API Key.</param>
        /// <param name="username">The username of the user.</param>
        /// <param name="ignoreSSLWarning"></param>
        /// <returns></returns>
        public AuthenticationResult SignAppIn(string apiKey, string username, bool ignoreSSLWarning)
        {
            return SignAppIn(apiKey, username, null, ignoreSSLWarning);
        }

        /// <summary>
        /// Authenticates with the server using the given Application API Key, username, and client certificate, optionally ignoring any SSL warnings.
        /// <para>API: POST Auth/SignAppin</para>
        /// </summary>
        /// <param name="apiKey">The Application API Key.</param>
        /// <param name="username">The username of the user.</param>
        /// <param name="clientCert">The client certificate to use for authentication.</param>
        /// <param name="ignoreSSLWarning">True to ignore SSL warnings, otherwise false.</param>
        /// <returns></returns>
        public AuthenticationResult SignAppIn(string apiKey, string username, X509Certificate2 clientCert, bool ignoreSSLWarning)
        {
            HttpClientHandler handler = new HttpClientHandler();

            if (clientCert != null)
                handler.ClientCertificates.Add(clientCert);

            if (ignoreSSLWarning)
                handler.ServerCertificateCustomValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

            _conn.HttpClient = new HttpClient(handler);


            // Use App API Key to Authenticate User
            _conn.HttpClient.AddDefaultRequestHeader("Authorization", string.Format("PS-Auth key={0}; runas={1};", apiKey, username));

            HttpResponseMessage response = _conn.Post("Auth/SignAppin");
            return ProcessAuthenticationResult(response);
        }
        
        /// <summary>
        /// Transforms the HttpResponseMessage into an AuthenticationResult and sets local properties on success.
        /// </summary>
        /// <param name="response">The response of the request.</param>
        private AuthenticationResult ProcessAuthenticationResult(HttpResponseMessage response)
        {
            AuthenticationResult result = new AuthenticationResult(response);
            if (result.IsSuccess)
                User = result.Value;

            return result;
        }

        /// <summary>
        /// Signs out of an authenticated connection.
        /// <para>API: Auth/Signout</para>
        /// </summary>
        /// <returns></returns>
        public SignOutResult SignOut()
        {
            HttpResponseMessage response = _conn.Post("Auth/Signout");
            SignOutResult result = new SignOutResult(response);

            // if successful, user is no longer authenticated
            if (result.IsSuccess)
                User = null;

            return result;
        }

    }
}
