using System.Net.Http;

namespace BeyondTrust.BeyondInsight.PasswordSafe.API.Client.V3
{
    public sealed class FunctionalAccountsEndpoint : BaseEndpoint
    {
        internal FunctionalAccountsEndpoint(PasswordSafeAPIConnector client)
            : base(client)
        {
        }

        /// <summary>
        /// Returns a list of Functional Accounts.
        /// <para>API: GET FunctionalAccounts</para>
        /// </summary>
        /// <returns></returns>
        public FunctionalAccountsResult GetAll()
        {
            HttpResponseMessage response = _conn.Get("FunctionalAccounts");
            FunctionalAccountsResult result = new FunctionalAccountsResult(response);
            return result;
        }

        /// <summary>
        /// Returns a Functional Account by ID.
        /// <para>API: GET FunctionalAccounts/{id}</para>
        /// </summary>
        /// <param name="id">ID of the Functional Account</param>
        /// <returns></returns>
        public FunctionalAccountResult Get(int id)
        {
            HttpResponseMessage response = _conn.Get(string.Format("FunctionalAccounts/{0}", id));
            FunctionalAccountResult result = new FunctionalAccountResult(response);
            return result;
        }

        /// <summary>
        /// Creates a new Functional Account.
        /// <para>API: POST FunctionalAccounts</para>
        /// </summary>
        /// <returns></returns>
        public FunctionalAccountResult Post(FunctionalAccountPostModel model)
        {
            HttpResponseMessage response = _conn.Post("FunctionalAccounts", model);
            FunctionalAccountResult result = new FunctionalAccountResult(response);
            return result;
        }

        /// <summary>
        /// Deletes a Functional Account by ID.
        /// <para>API: DELETE FunctionalAccounts/{id}</para>
        /// </summary>
        /// <param name="id">ID of the Functional Account</param>
        /// <returns></returns>
        public DeleteResult Delete(int id)
        {
            HttpResponseMessage response = _conn.Delete(string.Format("FunctionalAccounts/{0}", id));
            DeleteResult result = new DeleteResult(response);
            return result;
        }

    }
}
