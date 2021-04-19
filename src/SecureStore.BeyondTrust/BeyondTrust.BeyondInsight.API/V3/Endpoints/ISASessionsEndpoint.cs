using System.Net.Http;

namespace BeyondTrust.BeyondInsight.PasswordSafe.API.Client.V3
{
    public sealed class ISASessionsEndpoint : BaseEndpoint
    {
        internal ISASessionsEndpoint(PasswordSafeAPIConnector client)
            : base(client)
        {
        }

        /// <summary>
        /// Create a new SSH session for an Information Systems Administrator (ISA).
        /// <para>API: POST ISASessions</para>
        /// </summary>
        /// <param name="accountID">ID of the Managed Account to request</param>
        /// <param name="systemID">ID of the Managed System to request</param>
        /// <param name="durationInMinutes">The request duration (in minutes). If omitted, uses the value ManagedAccount.ISAReleaseDuration.</param>
        /// <param name="reason">The reason for the request</param>
        /// <returns></returns>
        public SessionsPostResult PostSSH(int accountID, int systemID, int? durationInMinutes, string reason)
        {
            ISASessionPostModel model = new ISASessionPostModel()
            {
                SessionType = "ssh",
                AccountID = accountID,
                SystemID = systemID,
                DurationMinutes = durationInMinutes,
                Reason = reason
            };

            HttpResponseMessage response = _conn.Post("ISASessions", model);
            ISASessionsResult result = new ISASessionsResult(response);
            return result;
        }

        /// <summary>
        /// Create a new RDP session for an Information Systems Administrator (ISA).
        /// <para>API: POST ISASessions</para>
        /// </summary>
        /// <param name="accountID">ID of the Managed Account to request</param>
        /// <param name="systemID">ID of the Managed System to request</param>
        /// <param name="durationInMinutes">The request duration (in minutes). If omitted, uses the value ManagedAccount.ISAReleaseDuration.</param>
        /// <param name="reason">The reason for the request</param>
        /// <returns></returns>
        public SessionsPostResult PostRDP(int accountID, int systemID, int? durationInMinutes, string reason)
        {
            ISASessionPostModel model = new ISASessionPostModel()
            {
                SessionType = "rdp",
                AccountID = accountID,
                SystemID = systemID,
                DurationMinutes = durationInMinutes,
                Reason = reason
            };

            HttpResponseMessage response = _conn.Post("ISASessions", model);
            ISASessionsResult result = new ISASessionsResult(response);
            return result;
        }

        /// <summary>
        /// Create a new RDP session for an Information Systems Administrator (ISA) and returns an RDP file that can be executed.
        /// <para>API: POST ISASessions</para>
        /// </summary>
        /// <param name="accountID">ID of the Managed Account to request</param>
        /// <param name="systemID">ID of the Managed System to request</param>
        /// <param name="durationInMinutes">The request duration (in minutes). If omitted, uses the value ManagedAccount.ISAReleaseDuration.</param>
        /// <param name="reason">The reason for the request</param>
        /// <returns></returns>
        public APIStreamResult PostRDPFile(int accountID, int systemID, int? durationInMinutes, string reason)
        {
            ISASessionPostModel model = new ISASessionPostModel()
            {
                SessionType = "rdpfile",
                AccountID = accountID,
                SystemID = systemID,
                DurationMinutes = durationInMinutes,
                Reason = reason
            };

            HttpResponseMessage response = _conn.Post("ISASessions", model);
            APIStreamResult result = new APIStreamResult(response);
            return result;
        }

    }
}
