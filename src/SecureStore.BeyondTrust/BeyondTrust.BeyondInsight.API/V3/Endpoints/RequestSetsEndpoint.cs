using System.Collections.Generic;
using System.Net.Http;

namespace BeyondTrust.BeyondInsight.PasswordSafe.API.Client.V3
{
    public sealed class RequestSetsEndpoint : BaseEndpoint
    {
        internal RequestSetsEndpoint(PasswordSafeAPIConnector client)
            : base(client)
        {
        }

        /// <summary>
        /// Lists request sets for the current user.
        /// <para>API: GET RequestSets</para>
        /// </summary>
        /// <param name="status">status (optional, default: all) – the status of request sets to return (all, active, pending)</param>
        /// <returns></returns>
        public RequestSetsResult GetAll(string status = "all")
        {
            HttpResponseMessage response = _conn.Get(string.Format("RequestSets?status={0}", status));
            RequestSetsResult result = new RequestSetsResult(response);
            return result;
        }

        /// <summary>
        /// Gets Active RequestSets for User.
        /// <para>API: GET RequestSets?status=active</para>
        /// </summary>
        /// <returns></returns>
        public RequestSetsResult GetActive()
        {
            return GetAll("active");
        }

        /// <summary>
        /// Gets Pending RequestSets for User.
        /// <para>API: GET RequestSets?status=pending</para>
        /// </summary>
        /// <returns></returns>
        public RequestSetsResult GetPending()
        {
            return GetAll("pending");
        }

        /// <summary>
        /// Requests account credentials.
        /// <para>API: POST RequestSets</para>
        /// </summary>
        /// <param name="accessTypes">A list of the types of access requested (View, RDP, SSH, App)</param>
        /// <param name="accountID">ID of the Managed Account to request</param>
        /// <param name="systemID">ID of the Managed System to request</param>
        /// <param name="durationInMinutes">The request duration (in minutes)</param>
        /// <param name="reason">The reason for the request</param>
        /// <param name="ticketSystemID">ID of the ticket system. If omitted then default ticket system will be used</param>
        /// <param name="ticketNumber">Number of associated ticket. Can be required if ticket system is marked as required in the global options</param>
        /// <returns></returns>
        public RequestSetResult Post(List<string> accessTypes, int accountID, int systemID, int durationInMinutes, string reason, int? ticketSystemID, string ticketNumber)//, int? accessPolicyScheduleID)
        {
            RequestSetPostModel req = new RequestSetPostModel()
            {
                AccessTypes = accessTypes,
                AccountId = accountID,
                SystemId = systemID,
                DurationMinutes = durationInMinutes,
                Reason = reason,
                //AccessPolicyScheduleID = accessPolicyScheduleID
                TicketSystemID = ticketSystemID,
                TicketNumber = ticketNumber,
            };

            HttpResponseMessage response = _conn.Post("RequestSets", req);
            RequestSetResult result = new RequestSetResult(response);
            return result;
        }
        
    }
}
