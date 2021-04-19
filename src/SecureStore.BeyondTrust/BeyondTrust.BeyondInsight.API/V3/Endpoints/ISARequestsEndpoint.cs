using System.Net.Http;

namespace BeyondTrust.BeyondInsight.PasswordSafe.API.Client.V3
{
    public sealed class ISARequestsEndpoint : BaseEndpoint
    {
        internal ISARequestsEndpoint(PasswordSafeAPIConnector client)
            : base(client)
        {
        }

        /// <summary>
        /// Creates a new Information Systems Administrator (ISA) release request and returns the requested credentials.
        /// <para>API: POST ISARequests</para>
        /// </summary>
        /// <param name="accountID">ID of the Managed Account to request</param>
        /// <param name="systemID">ID of the Managed System to request</param>
        /// <param name="durationInMinutes">The request duration (in minutes). If omitted, uses the value ManagedAccount.ISAReleaseDuration.</param>
        /// <param name="reason">The reason for the request</param>
        /// <returns></returns>
        public ISARequestsResult Post(int accountID, int systemID, int? durationInMinutes, string reason)
        {
            ISARequestPostModel body = new ISARequestPostModel()
            {
                AccountID = accountID,
                SystemID = systemID,
                DurationMinutes = durationInMinutes,
                Reason = reason
            };

            HttpResponseMessage response = _conn.Post("ISARequests", body);
            ISARequestsResult result = new ISARequestsResult(response);
            return result;
        }

        /// <summary>
        /// Creates a new Information Systems Administrator (ISA) release request and returns the requested credentials.
        /// <para>API: POST ISARequests?type={credentialType}</para>
        /// </summary>
        /// <param name="accountID">ID of the Managed Account to request</param>
        /// <param name="systemID">ID of the Managed System to request</param>
        /// <param name="durationInMinutes">The request duration (in minutes). If omitted, uses the value ManagedAccount.ISAReleaseDuration.</param>
        /// <param name="reason">The reason for the request</param>
        /// <param name="type">The type of credentials to retrieve (password, dsskey, passphrase)</param>
        /// <returns></returns>
        public ISARequestsResult Post(int accountID, int systemID, int? durationInMinutes, string reason, string type)
        {
            ISARequestPostModel body = new ISARequestPostModel()
            {
                AccountID = accountID,
                SystemID = systemID,
                DurationMinutes = durationInMinutes,
                Reason = reason
            };

            HttpResponseMessage response = _conn.Post(string.Format("ISARequests?type={0}", type), body);
            ISARequestsResult result = new ISARequestsResult(response);
            return result;
        }

    }
}
