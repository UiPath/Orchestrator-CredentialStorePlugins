using System;
using System.Net;
using System.Net.Http;

namespace BeyondTrust.BeyondInsight.PasswordSafe.API.Client.V3
{
    /// <summary>
    /// Password Safe API v3 Connector.
    /// </summary>
    internal class PasswordSafeAPIConnector
    {
        private readonly string _Url = string.Empty;

        /// <summary>
        /// Constructor for <seealso cref="PasswordSafeAPIConnector"/>.
        /// </summary>
        /// <param name="baseUrl">
        /// The base URL of the Password Safe API.
        /// <para>i.e. <example>https://the-url/BeyondTrust/api/public/v3</example></para>
        /// </param>
        internal PasswordSafeAPIConnector(string baseUrl)
        {
            _Url = baseUrl;
            Reset();
        }

        #region Internal Properties

        /// <summary>
        /// The HttpClient used to communicate with the server.
        /// </summary>
        internal HttpClient HttpClient { get; set; }

        /// <summary>
        /// The model of the authenticated user.  Returns null if not authenticated.
        /// </summary>
        internal SignAppInUserModel User { get; set; }

        #endregion

        #region Init

        /// <summary>
        /// Resets the client.
        /// </summary>
        internal void Reset()
        {
            HttpClient = new HttpClient();
            User = null;
        }

        #endregion

        #region Http Utilities

        /// <summary>
        /// Builds and returns an absolute Uri for an API.
        /// </summary>
        /// <param name="api">The name of the API for the Uri.</param>
        /// <returns></returns>
        private Uri BuildUri(string api)
        {
            string suri = string.Format("{0}/{1}", _Url, api);
            Uri uri = new Uri(suri);
            return uri;
        }

        /// <summary>
        /// Send a GET request to the given API.
        /// </summary>
        /// <param name="api">The name of the GET API.</param>
        /// <returns></returns>
        internal HttpResponseMessage Get(string api)
        {
            Uri uri = BuildUri(api);
            HttpResponseMessage response = HttpClient.GetAsync(uri).Result;

            // if unauthorized, reset
            if (response.StatusCode == HttpStatusCode.Unauthorized)
                Reset();

            return response;
        }

        /// <summary>
        /// Send a POST request to the given API with no post content.
        /// </summary>
        /// <param name="api">The name of the POST API.</param>
        /// <returns></returns>
        internal HttpResponseMessage Post(string api)
        {
            return Post(api, null);
        }

        /// <summary>
        /// Send a POST request to the given API, serializing the postContent.
        /// </summary>
        /// <param name="api">The name of the POST API.</param>
        /// <param name="postContent">The content to serialize for the POST request.</param>
        /// <returns></returns>
        internal HttpResponseMessage Post(string api, object postContent)
        {
            Uri uri = BuildUri(api);
            StringContent content = Utilities.SerializeContent(postContent);
            HttpResponseMessage response = HttpClient.PostAsync(uri, content).Result;

            // if unauthorized, reset
            if (response.StatusCode == HttpStatusCode.Unauthorized)
                Reset();

            return response;
        }

        /// <summary>
        /// Send a PUT request to the given API, serializing the putContent.
        /// </summary>
        /// <param name="api">The name of the PUT API.</param>
        /// <param name="postContent">The content to serialize for the PUT request.</param>
        /// <returns></returns>
        internal HttpResponseMessage Put(string api, object putContent)
        {
            Uri uri = BuildUri(api);
            StringContent content = Utilities.SerializeContent(putContent);
            HttpResponseMessage response = HttpClient.PutAsync(uri, content).Result;

            // if unauthorized, reset
            if (response.StatusCode == HttpStatusCode.Unauthorized)
                Reset();

            return response;
        }

        /// <summary>
        /// Send a DELETE request to the given API.
        /// </summary>
        /// <param name="api">The name of the DELETE API.</param>
        /// <returns></returns>
        internal HttpResponseMessage Delete(string api)
        {
            Uri uri = BuildUri(api);
            HttpResponseMessage response = HttpClient.DeleteAsync(uri).Result;

            // if unauthorized, reset
            if (response.StatusCode == HttpStatusCode.Unauthorized)
                Reset();

            return response;
        }

        #endregion

    }

}
