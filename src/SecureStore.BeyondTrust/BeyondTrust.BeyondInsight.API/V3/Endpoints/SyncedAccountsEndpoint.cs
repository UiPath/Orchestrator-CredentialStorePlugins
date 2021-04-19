using System.Net.Http;

namespace BeyondTrust.BeyondInsight.PasswordSafe.API.Client.V3
{
    public sealed class SyncedAccountsEndpoint : BaseEndpoint
    {
        internal SyncedAccountsEndpoint(PasswordSafeAPIConnector client)
            : base(client)
        {
        }

        /// <summary>
        /// Returns a list of subscribed/synced Managed Accounts by Managed Account ID.
        /// <para>API: GET ManagedAccounts/{id}/SyncedAccounts</para>
        /// </summary>
        /// <param name="id">ID of the parent Managed Account</param>
        /// <returns></returns>
        public ManagedAccountsResult GetAll(int id)
        {
            HttpResponseMessage response = _conn.Get($"ManagedAccounts/{id}/SyncedAccounts");
            ManagedAccountsResult result = new ManagedAccountsResult(response);
            return result;
        }

        /// <summary>
        /// Subscribes/syncs a Managed Account to the Managed Account referenced by ID.
        /// <para>API: POST ManagedAccounts/{id}/SyncedAccounts/{syncedAccountID}</para>
        /// </summary>
        /// <param name="id">ID of the parent Managed Account</param>
        /// <param name="syncedAccountID">ID of the synced Managed Account</param>
        /// <returns></returns>
        public ManagedAccountResult Post(int id, int syncedAccountID)
        {
            HttpResponseMessage response = _conn.Post($"ManagedAccounts/{id}/SyncedAccounts/{syncedAccountID}");
            ManagedAccountResult result = new ManagedAccountResult(response);
            return result;
        }

        /// <summary>
        /// Unsubscribes/unsyncs all Managed Accounts from the parent Managed Account by ID.
        /// <para>API: DELETE ManagedAccounts/{id}/SyncedAccounts</para>
        /// </summary>
        /// <param name="id">ID of the parent Managed Account</param>
        /// <returns></returns>
        public DeleteResult Delete(int id)
        {
            HttpResponseMessage response = _conn.Delete($"ManagedAccounts/{id}/SyncedAccounts");
            DeleteResult result = new DeleteResult(response);
            return result;
        }

        /// <summary>
        /// Unsubscribes/unsyncs a Managed Account from the Managed Account by ID.
        /// <para>API: DELETE ManagedAccounts/{id}/SyncedAccounts/{syncedAccountID}</para>
        /// </summary>
        /// <param name="id">ID of the parent Managed Account</param>
        /// <param name="syncedAccountID">ID of the synced Managed Account</param>
        /// <returns></returns>
        public DeleteResult Delete(int id, int syncedAccountID)
        {
            HttpResponseMessage response = _conn.Delete($"ManagedAccounts/{id}/SyncedAccounts/{syncedAccountID}");
            DeleteResult result = new DeleteResult(response);
            return result;
        }

    }
}
