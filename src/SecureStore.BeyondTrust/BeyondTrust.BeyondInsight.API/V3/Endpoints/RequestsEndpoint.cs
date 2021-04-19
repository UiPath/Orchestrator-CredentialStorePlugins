using System;
using System.Net.Http;

namespace BeyondTrust.BeyondInsight.PasswordSafe.API.Client.V3
{
    public sealed class RequestsEndpoint : BaseEndpoint
    {
        internal RequestsEndpoint(PasswordSafeAPIConnector client)
            : base(client)
        {
        }

        #region User APIs

        /// <summary>
        /// Gets Requests for User.
        /// <para>API: GET Requests</para>
        /// </summary>
        /// <param name="status">Optional - filter requests by status (all, active, pending)</param>
        /// <param name="queue">Optional - filter requests by queue type (req, app)</param>
        /// <returns></returns>
        public RequestsResult GetAll(string status = "all", string queue = "req")
        {
            HttpResponseMessage response = _conn.Get(string.Format("Requests?status={0}&queue={1}", status, queue));
            RequestsResult result = new RequestsResult(response);
            return result;
        }

        /// <summary>
        /// Gets Active Requests for Requestor.
        /// <para>API: GET Requests?status=active&queue=req</para>
        /// </summary>
        /// <returns></returns>
        public RequestsResult GetActive()
        {
            return GetAll("active");
        }

        /// <summary>
        /// Gets Pending Requests for Requestor.
        /// <para>API: GET Requests?status=pending&queue=req</para>
        /// </summary>
        /// <returns></returns>
        public RequestsResult GetPending()
        {
            return GetAll("pending");
        }

        /// <summary>
        /// Gets Active Requests for Approver.
        /// <para>API: GET Requests?status=active&queue=app</para>
        /// </summary>
        /// <returns></returns>
        public RequestsResult GetActiveForApprover()
        {
            return GetAll("active", "app");
        }

        /// <summary>
        /// Gets Pending Requests for Approver.
        /// <para>API: GET Requests?status=pending&queue=app</para>
        /// </summary>
        /// <returns></returns>
        public RequestsResult GetPendingForApprover()
        {
            return GetAll("pending", "app");
        }

        /// <summary>
        /// Creates a new release request with 'View' access.
        /// <para>API: POST Requests</para>
        /// </summary>
        /// <param name="accountID">ID of the Managed Account to request</param>
        /// <param name="systemID">ID of the Managed System to request</param>
        /// <param name="durationInMinutes">The request duration (in minutes)</param>
        /// <param name="reason">The reason for the request</param>
        /// <returns></returns>
        public RequestsPostResult Post(int accountID, int systemID, int durationInMinutes, string reason)
        {
            RequestPostModel body = new RequestPostModel()
            {
                AccessType = "View",
                AccountId = accountID,
                SystemId = systemID,
                DurationMinutes = durationInMinutes,
                Reason = reason
            };

            HttpResponseMessage response = _conn.Post("Requests", body);
            RequestsPostResult result = new RequestsPostResult(response);
            return result;
        }

        /// <summary>
        /// Creates a new release request.
        /// <para>API: POST Requests</para>
        /// </summary>
        /// <param name="accessType">The type of access requested (View, RDP, SSH, App)</param>
        /// <param name="accountID">ID of the Managed Account to request</param>
        /// <param name="systemID">ID of the Managed System to request</param>
        /// <param name="applicationID">ID of the Application for an Application-based request</param>
        /// <param name="durationInMinutes">The request duration (in minutes)</param>
        /// <param name="reason">The reason for the request</param>
        /// <param name="accessPolicyScheduleID">The Schedule ID of an Access Policy to use for the request. If omitted, automatically selects the best schedule.</param>
        /// <param name="conflictOption">The conflict resolution option to use if an existing request is found for the same user, system, and account (reuse, renew).</param>
        /// <param name="ticketSystemID">ID of the ticket system. If omitted then default ticket system will be used</param>
        /// <param name="ticketNumber">Number of associated ticket. Can be required if ticket system is marked as required in the global options</param>
        /// <returns></returns>
        public RequestsPostResult Post(string accessType, int accountID, int systemID, int? applicationID, int durationInMinutes, string reason, int? accessPolicyScheduleID, string conflictOption, int? ticketSystemID, string ticketNumber)
        {
            RequestPostModel body = new RequestPostModel()
            {
                AccessType = accessType,
                AccountId = accountID,
                SystemId = systemID,
                ApplicationID = applicationID,
                DurationMinutes = durationInMinutes,
                Reason = reason,
                AccessPolicyScheduleID = accessPolicyScheduleID,
                ConflictOption = conflictOption,
                TicketSystemID = ticketSystemID,
                TicketNumber = ticketNumber,
            };

            HttpResponseMessage response = _conn.Post("Requests", body);
            RequestsPostResult result = new RequestsPostResult(response);
            return result;
        }

        /// <summary>
        /// Requests account credentials by Alias.
        /// <para>API: POST Aliases/{id}/Requests</para>
        /// </summary>
        /// <param name="aliasID">The alias ID for the request.</param>
        /// <param name="durationInMinutes">The duration (in minutes) for the request.</param>
        /// <param name="reason">The reason for the request.</param>
        /// <param name="ticketSystemID">ID of the ticket system. If omitted then default ticket system will be used</param>
        /// <param name="ticketNumber">Number of associated ticket. Can be required if ticket system is marked as required in the global options</param>
        /// <returns></returns>
        public RequestAliasResult Post(int aliasID, int durationInMinutes, string reason, int? ticketSystemID, string ticketNumber)
        {
            RequestPostModel passwordRequest = new RequestPostModel()
            {
                DurationMinutes = durationInMinutes,
                Reason = reason,
                TicketSystemID = ticketSystemID,
                TicketNumber = ticketNumber,
            };

            HttpResponseMessage response = _conn.Post(string.Format("Aliases/{0}/Requests", aliasID), passwordRequest);
            RequestAliasResult result = new RequestAliasResult(response);
            return result;
        }

        /// <summary>
        /// Release a request without a comment.
        /// <para>API: PUT Requests/Release</para>
        /// </summary>
        /// <param name="requestID">ID of the request to release.</param>
        /// <returns></returns>
        [Obsolete("Use Checkin(int requestID) instead")]
        public RequestCheckinResult Release(int requestID)
        {
            return Checkin(requestID);
        }

        /// <summary>
        /// Release a password with a comment.
        /// <para>API: PUT Requests/Release</para>
        /// </summary>
        /// <param name="requestID">ID of the request to release.</param>
        /// <param name="releaseComment">The comment for the release.</param>
        /// <returns></returns>
        [Obsolete("Use Checkin(int requestID, string releaseComment) instead")]
        public RequestCheckinResult Release(int requestID, string releaseComment)
        {
            return Checkin(requestID, releaseComment);
        }

        /// <summary>
        /// Checks-in/releases a request before it has expired without a comment.
        /// <para>API: PUT Requests/{id}/Checkin</para>
        /// </summary>
        /// <param name="requestID">The ID of the request to release.</param>
        /// <returns></returns>
        public RequestCheckinResult Checkin(int requestID)
        {
            return Checkin(requestID, null);
        }

        /// <summary>
        /// Checks-in/releases a request before it has expired with a comment.
        /// <para>API: PUT Requests/{id}/Checkin</para>
        /// </summary>
        /// <param name="requestID">ID of the request to release.</param>
        /// <param name="comment">The comment for the release.</param>
        /// <returns></returns>
        public RequestCheckinResult Checkin(int requestID, string comment)
        {
            RequestCheckinModel model = new RequestCheckinModel() { Reason = comment };

            HttpResponseMessage response = _conn.Put(string.Format("Requests/{0}/Checkin", requestID), model);
            RequestCheckinResult result = new RequestCheckinResult(response);
            return result;
        }

        #endregion

        #region Approver APIs

        /// <summary>
        /// Approves a pending request.
        /// <para>API: PUT Requests/{id}/Approve</para>
        /// </summary>
        /// <param name="requestID">ID of the request to approve.</param>
        /// <param name="approvalComment">A reason or comment why the request is being approved.</param>
        /// <returns></returns>
        public RequestApproveResult Approve(int requestID, string approvalComment)
        {
            RequestApproveModel model = new RequestApproveModel() { Reason = approvalComment };

            HttpResponseMessage response = _conn.Put(string.Format("Requests/{0}/Approve", requestID), model);
            RequestApproveResult result = new RequestApproveResult(response);
            return result;
        }

        /// <summary>
        /// Denies/cancels an active or pending request.
        /// <para>API: PUT Requests/{id}/Deny</para>
        /// </summary>
        /// <param name="requestID">ID of the request to deny.</param>
        /// <param name="denialComment">A reason or comment why the request is being denied/cancelled.</param>
        /// <returns></returns>
        public RequestDenyResult Deny(int requestID, string denialComment)
        {
            RequestDenyModel model = new RequestDenyModel() { Reason = denialComment };

            HttpResponseMessage response = _conn.Put(string.Format("Requests/{0}/Deny", requestID), model);
            RequestDenyResult result = new RequestDenyResult(response);
            return result;
        }

        #endregion

        #region Administrative APIs

        /// <summary>
        /// Terminates all active Requests by Managed Account ID.
        /// <para>API: POST ManagedAccounts/{managedAccountID}/Requests/Terminate</para>
        /// </summary>
        /// <param name="managedAccountID">ID of the Managed Account</param>
        /// <param name="comment">A reason or comment why the requests are being terminated.</param>
        /// <returns></returns>
        public NoContentResult TerminateByManagedAccount(int managedAccountID, string comment = "")
        {
            RequestTerminateModel model = new RequestTerminateModel() { Reason = comment };

            HttpResponseMessage response = _conn.Post($"ManagedAccounts/{managedAccountID}/Requests/Terminate", model);
            NoContentResult result = new NoContentResult(response);
            return result;
        }

        /// <summary>
        /// Terminates all active Requests by Managed System ID.
        /// <para>API: POST ManagedSystems/{managedSystemID}/Requests/Terminate</para>
        /// </summary>
        /// <param name="managedSystemID">ID of the Managed System</param>
        /// <param name="comment">A reason or comment why the requests are being terminated.</param>
        /// <returns></returns>
        public NoContentResult TerminateByManagedSystem(int managedSystemID, string comment = "")
        {
            RequestTerminateModel model = new RequestTerminateModel() { Reason = comment };

            HttpResponseMessage response = _conn.Post($"ManagedSystems/{managedSystemID}/Requests/Terminate");
            NoContentResult result = new NoContentResult(response);
            return result;
        }

        /// <summary>
        /// Terminates all active Requests by Requestor User ID.
        /// <para>API: POST Users/{userID}/Requests/Terminate</para>
        /// </summary>
        /// <param name="userID">ID of the Requestor User</param>
        /// <param name="comment">A reason or comment why the requests are being terminated.</param>
        /// <returns></returns>
        public NoContentResult TerminateByUser(int userID, string comment = "")
        {
            RequestTerminateModel model = new RequestTerminateModel() { Reason = comment };

            HttpResponseMessage response = _conn.Post($"Users/{userID}/Requests/Terminate");
            NoContentResult result = new NoContentResult(response);
            return result;
        }

        #endregion

    }
}
