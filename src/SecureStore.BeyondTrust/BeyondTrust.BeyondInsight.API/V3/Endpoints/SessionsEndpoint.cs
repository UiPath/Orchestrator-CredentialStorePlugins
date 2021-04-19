using System.Net.Http;

namespace BeyondTrust.BeyondInsight.PasswordSafe.API.Client.V3
{
    public sealed class SessionsEndpoint : BaseEndpoint
    {
        internal SessionsEndpoint(PasswordSafeAPIConnector client)
            : base(client)
        {
        }

        /// <summary>
        /// Returns a list of Sessions.
        /// <para>API: GET Sessions</para>
        /// </summary>
        /// <returns></returns>
        public SessionsResult GetAll(int? status = null, int? userID = null)
        {
            string queryParams = QueryParameterBuilder.Build(
                  new QueryParameter("status", status)
                , new QueryParameter("userID", userID)
                );

            HttpResponseMessage response = _conn.Get($"Sessions{queryParams}");
            SessionsResult result = new SessionsResult(response);
            return result;
        }

        /// <summary>
        /// Returns a Session by ID.
        /// <para>API: GET Sessions/{id}</para>
        /// </summary>
        /// <param name="id">ID of the Session</param>
        /// <returns></returns>
        public SessionResult Get(int id)
        {
            HttpResponseMessage response = _conn.Get($"Sessions/{id}");
            SessionResult result = new SessionResult(response);
            return result;
        }

        /// <summary>
        /// Create a new SSH session for the given release.
        /// <para>API: POST Requests/{requestID}/Sessions</para>
        /// </summary>
        /// <param name="requestID">ID of the Request</param>
        /// <returns></returns>
        public SessionsPostResult PostSSH(int requestID, string nodeID = null)
        {
            SessionsPostModel model = new SessionsPostModel()
            {
                SessionType = "ssh",
                NodeID = nodeID
            };

            HttpResponseMessage response = _conn.Post($"Requests/{requestID}/Sessions", model);
            SessionsPostResult result = new SessionsPostResult(response);
            return result;
        }

        /// <summary>
        /// Create a new RDP session for the given release.
        /// <para>API: POST Requests/{requestID}/Sessions</para>
        /// </summary>
        /// <param name="requestID">ID of the Request</param>
        /// <returns></returns>
        public SessionsPostResult PostRDP(int requestID, string nodeID = null)
        {
            SessionsPostModel model = new SessionsPostModel()
            {
                SessionType = "rdp",
                NodeID = nodeID
            };

            HttpResponseMessage response = _conn.Post($"Requests/{requestID}/Sessions", model);
            SessionsPostResult result = new SessionsPostResult(response);
            return result;
        }

        /// <summary>
        /// Create a new RDP session for the given release and returns an RDP file that can be executed.
        /// <para>API: POST Requests/{requestID}/Sessions</para>
        /// </summary>
        /// <param name="requestID">ID of the Request</param>
        /// <returns></returns>
        public APIStreamResult PostRDPFile(int requestID, string nodeID = null)
        {
            SessionsPostModel model = new SessionsPostModel()
            {
                SessionType = "rdpfile",
                NodeID = nodeID
            };

            HttpResponseMessage response = _conn.Post($"Requests/{requestID}/Sessions", model);
            APIStreamResult result = new APIStreamResult(response);
            return result;
        }

        /// <summary>
        /// Create a new Application session for the given release.
        /// <para>API: POST Requests/{requestID}/Sessions</para>
        /// </summary>
        /// <param name="requestID">ID of the Request</param>
        /// <returns></returns>
        public SessionsPostResult PostApp(int requestID, string nodeID = null)
        {
            SessionsPostModel model = new SessionsPostModel()
            {
                SessionType = "app",
                NodeID = nodeID
            };

            HttpResponseMessage response = _conn.Post($"Requests/{requestID}/Sessions", model);
            SessionsPostResult result = new SessionsPostResult(response);
            return result;
        }

        /// <summary>
        /// Create a new Application session for the given release and returns an RDP file that can be executed.
        /// <para>API: POST Requests/{requestID}/Sessions</para>
        /// </summary>
        /// <param name="requestID">ID of the Request</param>
        /// <returns></returns>
        public APIStreamResult PostAppFile(int requestID, string nodeID = null)
        {
            SessionsPostModel model = new SessionsPostModel()
            {
                SessionType = "appfile",
                NodeID = nodeID
            };

            HttpResponseMessage response = _conn.Post($"Requests/{requestID}/Sessions", model);
            APIStreamResult result = new APIStreamResult(response);
            return result;
        }

        /// <summary>
        /// Creates a new SSH admin session.
        /// <para>API: POST Sessions/Admin</para>
        /// </summary>
        /// <returns></returns>
        public SessionsPostResult PostAdminSSH(SessionsAdminModel model)
        {
            model.SessionType = "ssh";

            HttpResponseMessage response = _conn.Post("Sessions/Admin", model);
            SessionsPostResult result = new SessionsPostResult(response);
            return result;
        }

        /// <summary>
        /// Creates a new RDP admin session.
        /// <para>API: POST Sessions/Admin</para>
        /// </summary>
        /// <returns></returns>
        public SessionsPostResult PostAdminRDP(SessionsAdminModel model)
        {
            model.SessionType = "rdp";

            HttpResponseMessage response = _conn.Post("Sessions/Admin", model);
            SessionsPostResult result = new SessionsPostResult(response);
            return result;
        }

        /// <summary>
        /// Creates a new RDP admin session and returns an RDP file that can be executed.
        /// <para>API: POST Sessions/Admin</para>
        /// </summary>
        /// <returns></returns>
        public APIStreamResult PostAdminRDPFile(SessionsAdminModel model)
        {
            model.SessionType = "rdpfile";

            HttpResponseMessage response = _conn.Post("Sessions/Admin", model);
            APIStreamResult result = new APIStreamResult(response);
            return result;
        }

        /// <summary>
        /// Locks an active Session.
        /// <para>API: POST Sessions/{id}/Lock</para>
        /// </summary>
        /// <param name="id">ID of the session</param>
        /// <returns></returns>
        public SessionsLockResult Lock(int id)
        {
            HttpResponseMessage response = _conn.Post($"Sessions/{id}/Lock");
            SessionsLockResult result = new SessionsLockResult(response);
            return result;
        }

        /// <summary>
        /// Locks all active Sessions by Managed Account ID.
        /// <para>API: POST ManagedAccounts/{managedAccountID}/Sessions/Lock</para>
        /// </summary>
        /// <param name="managedAccountID">ID of the Managed Account</param>
        /// <returns></returns>
        public SessionsLockResult LockByManagedAccount(int managedAccountID)
        {
            HttpResponseMessage response = _conn.Post($"ManagedAccounts/{managedAccountID}/Sessions/Lock");
            SessionsLockResult result = new SessionsLockResult(response);
            return result;
        }

        /// <summary>
        /// Locks all active Sessions by Managed System ID.
        /// <para>API: POST ManagedSystems/{managedSystemID}/Sessions/Lock</para>
        /// </summary>
        /// <param name="managedSystemID">ID of the Managed System</param>
        /// <returns></returns>
        public SessionsLockResult LockByManagedSystem(int managedSystemID)
        {
            HttpResponseMessage response = _conn.Post($"ManagedSystems/{managedSystemID}/Sessions/Lock");
            SessionsLockResult result = new SessionsLockResult(response);
            return result;
        }

        /// <summary>
        /// Terminates an active Session.
        /// <para>API: POST Sessions/{sessionID}/Terminate</para>
        /// </summary>
        /// <param name="sessionID">ID of the Session to terminate</param>
        /// <returns></returns>
        public NoContentResult Terminate(int sessionID)
        {
            HttpResponseMessage response = _conn.Post($"Sessions/{sessionID}/Terminate");
            NoContentResult result = new NoContentResult(response);
            return result;
        }

        /// <summary>
        /// Terminates all active Sessions by Managed Account ID.
        /// <para>API: POST ManagedAccounts/{managedAccountID}/Sessions/Terminate</para>
        /// </summary>
        /// <param name="managedAccountID">ID of the Managed Account</param>
        /// <returns></returns>
        public NoContentResult TerminateByManagedAccount(int managedAccountID)
        {
            HttpResponseMessage response = _conn.Post($"ManagedAccounts/{managedAccountID}/Sessions/Terminate");
            NoContentResult result = new NoContentResult(response);
            return result;
        }

        /// <summary>
        /// Terminates all active Sessions by Managed System ID.
        /// <para>API: POST ManagedSystems/{managedSystemID}/Sessions/Terminate</para>
        /// </summary>
        /// <param name="managedSystemID">ID of the Managed System</param>
        /// <returns></returns>
        public NoContentResult TerminateByManagedSystem(int managedSystemID)
        {
            HttpResponseMessage response = _conn.Post($"ManagedSystems/{managedSystemID}/Sessions/Terminate");
            NoContentResult result = new NoContentResult(response);
            return result;
        }

    }
}
