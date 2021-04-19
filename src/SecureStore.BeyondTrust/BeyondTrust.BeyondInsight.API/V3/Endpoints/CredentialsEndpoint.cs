using System.Net.Http;

namespace BeyondTrust.BeyondInsight.PasswordSafe.API.Client.V3
{
    public sealed class CredentialsEndpoint : BaseEndpoint
    {
        internal CredentialsEndpoint(PasswordSafeAPIConnector client)
            : base(client)
        {
        }

        /// <summary>
        /// Retrieves the credentials for an approved and active (not expired) credentials release request.
        /// <para>API: GET Credentials/{requestID}</para>
        /// </summary>
        /// <param name="requestID">ID of the Request for which to retrieve the credentials.</param>
        /// <returns></returns>
        public CredentialsResult Get(int requestID)
        {
            HttpResponseMessage response = _conn.Get(string.Format("Credentials/{0}", requestID));
            CredentialsResult result = new CredentialsResult(response);
            return result;
        }

        /// <summary>
        /// Retrieves the credentials for an approved and active (not expired) credentials release request, explicitly providing the type of credentials to retrieve.
        /// <para>API: GET Credentials/{requestID}?type={credentialType}</para>
        /// </summary>
        /// <param name="requestID">ID of the Request for which to retrieve the credentials.</param>
        /// <param name="type">The type of credentials to retrieve.</param>
        /// <returns></returns>
        public CredentialsResult Get(int requestID, string type)
        {
            HttpResponseMessage response = _conn.Get(string.Format("Credentials/{0}?type={1}", requestID, type));
            CredentialsResult result = new CredentialsResult(response);
            return result;
        }

        /// <summary>
        /// Retrieves the credentials and alias details for an approved and active (not expired) credentials release request for an Alias.
        /// <para>API: GET Aliases/{aliasID}/Credentials/{requestID}</para>
        /// </summary>
        /// <param name="aliasID">Alias ID</param>
        /// <param name="requestID">The Request ID for which to return a password.</param>
        /// <returns></returns>
        public AliasCredentialsResult Get(int aliasID, int requestID)
        {
            HttpResponseMessage response = _conn.Get(string.Format("Aliases/{0}/Credentials/{1}", aliasID, requestID));
            AliasCredentialsResult result = new AliasCredentialsResult(response);
            return result;
        }

        /// <summary>
        /// Updates the Managed Account credentials with a randomly generated password, optionally applying the change to the Managed System.
        /// <para>API: PUT ManagedAccounts/{id}/Credentials</para>
        /// </summary>
        /// <param name="accountID">ID of the account for which to set the credentials.</param>
        /// <param name="updateSystem">If true, sets the Credentials on the managed host.</param>
        /// <returns></returns>
        public CredentialsPutResult PutRandomPassword(int accountID, bool updateSystem)
        {
            return PutPassword(accountID, string.Empty, updateSystem);
        }

        /// <summary>
        /// Updates the Managed Account password, optionally applying the change to the Managed System.
        /// <para>API: PUT ManagedAccounts/{id}/Credentials</para>
        /// </summary>
        /// <param name="accountID">ID of the account for which to set the credentials.</param>
        /// <param name="password">The password to set.</param>
        /// <param name="updateSystem">If true, sets the Credentials on the managed host.</param>
        /// <returns></returns>
        public CredentialsPutResult PutPassword(int accountID, string password, bool updateSystem)
        {
            CredentialsPutModel model = new CredentialsPutModel()
            {
                Password = password,
                UpdateSystem = updateSystem
            };

            return Put(accountID, model);
        }

        /// <summary>
        /// Updates the Managed Account credentials with an unencrypted Private Key without applying the change on the Managed System.
        /// <para>API: PUT ManagedAccounts/{id}/Credentials</para>
        /// </summary>
        /// <param name="accountID">ID of the account for which to set the credentials.</param>
        /// <param name="privateKey">The plain text/not encrypted Private Key.</param>
        /// <returns></returns>
        public CredentialsPutResult PutDSSKey(int accountID, string privateKey)
        {
            return PutDSSKey(accountID, string.Empty, privateKey, string.Empty);
        }

        /// <summary>
        /// Updates the Managed Account credentials with an encrypted Private Key (and corresponding Passphrase) without applying the change to the Managed System.
        /// <para>API: PUT ManagedAccounts/{id}/Credentials</para>
        /// </summary>
        /// <param name="accountID">ID of the account for which to set the credentials.</param>
        /// <param name="privateKey">The encrypted Private Key portion of the DSS Key.</param>
        /// <param name="passphrase">The passphrase to set.</param>
        /// <returns></returns>
        public CredentialsPutResult PutDSSKey(int accountID, string privateKey, string passphrase)
        {
            return PutDSSKey(accountID, string.Empty, privateKey, passphrase);
        }

        /// <summary>
        /// Updates Managed Account credentials with an encrypted Private Key (and corresponding Passphrase), applying the Public Key to the Managed System.
        /// <para>API: PUT ManagedAccounts/{id}/Credentials</para>
        /// </summary>
        /// <param name="accountID">ID of the account for which to set the credentials.</param>
        /// <param name="publicKey">The Public Key portion of the DSS Key.</param>
        /// <param name="privateKey">The encrypted Private Key portion of the DSS Key.</param>
        /// <param name="passphrase">The Passphrase used to decrypt <paramref name="privateKey"/>.</param>
        /// <returns></returns>
        public CredentialsPutResult PutDSSKey(int accountID, string publicKey, string privateKey, string passphrase)
        {
            CredentialsPutModel model = new CredentialsPutModel()
            {
                PublicKey = publicKey,
                PrivateKey = privateKey,
                Passphrase = passphrase,
                UpdateSystem = !string.IsNullOrEmpty(publicKey)
            };

            return Put(accountID, model);
        }

        /// <summary>
        /// Updates the Credentials for a Managed Account using the given model.
        /// <para>API: PUT ManagedAccounts/{id}/Credentials</para>
        /// </summary>
        /// <param name="accountID">ID of the account for which to set the credentials.</param>
        /// <param name="model"></param>
        /// <returns></returns>
        public CredentialsPutResult Put(int accountID, CredentialsPutModel model)
        {
            HttpResponseMessage response = _conn.Put(string.Format("ManagedAccounts/{0}/Credentials", accountID), model);
            CredentialsPutResult result = new CredentialsPutResult(response);
            return result;
        }

        /// <summary>
        /// Updates the Credentials for a Managed Account using the given model.
        /// <para>API: PUT Credentials?workgroupName={workgroupName}&assetName={assetName}&accountName={accountName}</para>
        /// </summary>
        /// <param name="workgroupName">Name of the Workgroup</param>
        /// <param name="assetName">Name of the Asset</param>
        /// <param name="accountName">Name of the Managed Account for which to set the credentials</param>
        /// <param name="model"></param>
        /// <returns></returns>
        public CredentialsPutResult Put(string workgroupName, string assetName, string accountName, CredentialsPutModel model)
        {
            HttpResponseMessage response = _conn.Put($"Credentials?workgroupName={workgroupName}&assetName={assetName}&accountName={accountName}", model);
            CredentialsPutResult result = new CredentialsPutResult(response);
            return result;
        }

        /// <summary>
        /// Tests the current credentials of a Managed Account.
        /// <para>API: POST ManagedAccounts/{accountId}/Credentials/Test</para>
        /// </summary>
        /// <param name="accountID">ID of the Managed Account</param>
        /// <returns></returns>
        public CredentialsTestResult Test(int accountID)
        {
            HttpResponseMessage response = _conn.Post(string.Format("ManagedAccounts/{0}/Credentials/Test", accountID));
            CredentialsTestResult result = new CredentialsTestResult(response);
            return result;
        }

        /// <summary>
        /// Changes the current Credentials of a Managed Account.
        /// <para>API: POST ManagedAccounts/{accountId}/Credentials/Change</para>
        /// </summary>
        /// <param name="accountID">ID of the Managed Account</param>
        /// <param name="queue">True to queue the change for background processing, otherwise false. When Queue is false the credentials change is immediate.</param>
        /// <returns></returns>
        public NoContentResult Change(int accountID, bool queue)
        {
            object model = new { Queue = queue };

            HttpResponseMessage response = _conn.Post($"ManagedAccounts/{accountID}/Credentials/Change", model);
            NoContentResult result = new NoContentResult(response);
            return result;
        }

        /// <summary>
        /// Queues Credential changes for all active Managed Accounts for a Managed System.
        /// <para>API: POST ManagedSystems/{systemId}/ManagedAccounts/Credentials/Change</para>
        /// </summary>
        /// <param name="systemId">ID of the Managed System</param>
        /// <returns></returns>
        public NoContentResult ChangeAllByManagedSystemID(int systemId)
        {
            HttpResponseMessage response = _conn.Post($"ManagedSystems/{systemId}/ManagedAccounts/Credentials/Change", null);
            NoContentResult result = new NoContentResult(response);
            return result;
        }

    }
}
