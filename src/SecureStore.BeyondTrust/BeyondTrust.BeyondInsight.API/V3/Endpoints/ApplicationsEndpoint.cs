using System.Net.Http;

namespace BeyondTrust.BeyondInsight.PasswordSafe.API.Client.V3
{
    public sealed class ApplicationsEndpoint : BaseEndpoint
    {
        internal ApplicationsEndpoint(PasswordSafeAPIConnector client)
            : base(client)
        {
        }

        #region Application Definitions

        /// <summary>
        /// Returns a list of Applications.
        /// <para>API: GET Applications</para>
        /// </summary>
        /// <returns></returns>
        public ApplicationsResult GetAll()
        {
            HttpResponseMessage response = _conn.Get("Applications");
            ApplicationsResult result = new ApplicationsResult(response);
            return result;
        }

        /// <summary>
        /// Returns an Application by ID.
        /// <para>API: GET Applications/{id}</para>
        /// </summary>
        /// <param name="id">ID of the Application</param>
        /// <returns></returns>
        public ApplicationResult Get(int id)
        {
            HttpResponseMessage response = _conn.Get($"Applications/{id}");
            ApplicationResult result = new ApplicationResult(response);
            return result;
        }

        #endregion

        #region Managed Account Applications

        /// <summary>
        /// Returns a list of Applications assigned to a Managed Account.
        /// <para>API: GET ManagedAccounts/{accountID}/Applications</para>
        /// </summary>
        /// <param name="accountID">ID of the Managed Account</param>
        /// <returns></returns>
        public ApplicationsResult GetByManagedAccountID(int accountID)
        {
            HttpResponseMessage response = _conn.Get($"ManagedAccounts/{accountID}/Applications");
            ApplicationsResult result = new ApplicationsResult(response);
            return result;
        }

        /// <summary>
        /// Assigns an Application to a Managed Account.
        /// <para>API: POST ManagedAccounts/{accountID}/Applications/{applicationID}</para>
        /// </summary>
        /// <param name="accountID">ID of the Managed Account</param>
        /// <param name="applicationID">ID of the Application</param>
        /// <returns></returns>
        public ApplicationResult Post(int accountID, int applicationID)
        {
            HttpResponseMessage response = _conn.Post($"ManagedAccounts/{accountID}/Applications/{applicationID}");
            ApplicationResult result = new ApplicationResult(response);
            return result;
        }

        /// <summary>
        /// Unassigns an Application from a Managed Account by Managed Account ID and Application ID.
        /// <para>API: DELETE ManagedAccounts/{accountID}/Applications/{applicationID}</para>
        /// </summary>
        /// <param name="accountID">ID of the Managed Account</param>
        /// <param name="applicationID">ID of the Application</param>
        /// <returns></returns>
        public DeleteResult Delete(int accountID, int applicationID)
        {
            HttpResponseMessage response = _conn.Delete($"ManagedAccounts/{accountID}/Applications/{applicationID}");
            DeleteResult result = new DeleteResult(response);
            return result;
        }

        /// <summary>
        /// Unassigns all Managed Account Applications by Managed Account ID.
        /// <para>API: DELETE ManagedAccounts/{accountID}/Applications</para>
        /// </summary>
        /// <param name="accountID">ID of the Managed Account</param>
        /// <param name="applicationID">ID of the Application</param>
        /// <returns></returns>
        public DeleteResult Delete(int accountID)
        {
            HttpResponseMessage response = _conn.Delete($"ManagedAccounts/{accountID}/Applications");
            DeleteResult result = new DeleteResult(response);
            return result;
        }

        #endregion

    }
}
