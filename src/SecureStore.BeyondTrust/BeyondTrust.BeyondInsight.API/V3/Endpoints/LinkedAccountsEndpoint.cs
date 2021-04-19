using System.Net.Http;

namespace BeyondTrust.BeyondInsight.PasswordSafe.API.Client.V3
{
    public sealed class LinkedAccountsEndpoint : BaseEndpoint
    {
        internal LinkedAccountsEndpoint(PasswordSafeAPIConnector client)
            : base(client)
        {
        }

        /// <summary>
        /// Returns a list of linked Directory Managed Accounts by Managed System ID.
        /// <para>API: GET ManagedSystems/{systemID}/LinkedAccounts</para>
        /// </summary>
        /// <param name="systemID">ID of the Managed System</param>
        /// <returns></returns>
        public ManagedAccountsResult GetAll(int systemID)
        {
            HttpResponseMessage response = _conn.Get($"ManagedSystems/{systemID}/LinkedAccounts");
            ManagedAccountsResult result = new ManagedAccountsResult(response);
            return result;
        }

        /// <summary>
        /// Links a Directory Managed Account to the Managed System referenced by ID.
        /// <para>API: POST ManagedSystems/{systemID}/LinkedAccounts/{accountID}</para>
        /// </summary>
        /// <param name="systemID">ID of the Managed System</param>
        /// <param name="accountID">ID of the Directory Managed Account</param>
        /// <returns></returns>
        public ManagedAccountResult Post(int systemID, int accountID)
        {
            HttpResponseMessage response = _conn.Post($"ManagedSystems/{systemID}/LinkedAccounts/{accountID}");
            ManagedAccountResult result = new ManagedAccountResult(response);
            return result;
        }

        /// <summary>
        /// Unlinks all Directory Managed Accounts from the Managed System by ID.
        /// <para>API: DELETE ManagedSystems/{systemID}/LinkedAccounts</para>
        /// </summary>
        /// <param name="systemID">ID of the Managed System</param>
        /// <returns></returns>
        public DeleteResult Delete(int systemID)
        {
            HttpResponseMessage response = _conn.Delete($"ManagedSystems/{systemID}/LinkedAccounts");
            DeleteResult result = new DeleteResult(response);
            return result;
        }

        /// <summary>
        /// Unlinks a Directory Managed Account from the Managed System by ID.
        /// <para>API: DELETE ManagedSystems/{systemID}/LinkedAccounts/{accountID}</para>
        /// </summary>
        /// <param name="systemID">ID of the Managed System</param>
        /// <param name="accountID">ID of the Directory Managed Account</param>
        /// <returns></returns>
        public DeleteResult Delete(int systemID, int accountID)
        {
            HttpResponseMessage response = _conn.Delete($"ManagedSystems/{systemID}/LinkedAccounts/{accountID}");
            DeleteResult result = new DeleteResult(response);
            return result;
        }

    }
}
