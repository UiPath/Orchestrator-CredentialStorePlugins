using System.Collections.Generic;
using System.Net.Http;

namespace BeyondTrust.BeyondInsight.PasswordSafe.API.Client.V3
{
    public sealed class ManagedSystemsEndpoint : BaseEndpoint
    {
        internal ManagedSystemsEndpoint(PasswordSafeAPIConnector client)
            : base(client)
        {
        }

        /// <summary>
        /// List of Managed System Account Name formats.
        /// </summary>
        public readonly IReadOnlyDictionary<int, string> AccountNameFormats = new Dictionary<int, string>() {
            { 0, "Domain & Account" },
            { 1, "UPN" },
            { 2, "SAM" }
        };

        /// <summary>
        /// Returns a list of Managed Systems.
        /// <para>API: GET ManagedSystems</para>
        /// </summary>
        /// <returns></returns>
        public ManagedSystemsResult GetAll(int? type = null, string name = null)
        {
            string queryParams = QueryParameterBuilder.Build(
                  new QueryParameter("type", type)
                , new QueryParameter("name", name)
                );

            HttpResponseMessage response = _conn.Get($"ManagedSystems{queryParams}");
            ManagedSystemsResult result = new ManagedSystemsResult(response);
            return result;
        }

        /// <summary>
        /// Returns a paged list of Managed Systems.
        /// <para>API: GET ManagedSystems?limit={limit}&offset={offset}</para>
        /// </summary>
        /// <returns></returns>
        public ManagedSystemsPagedResult GetAll(int limit, int? offset = null, int? type = null, string name = null)
        {
            string queryParams = QueryParameterBuilder.Build(
                  new QueryParameter("limit", limit)
                , new QueryParameter("offset", offset)
                , new QueryParameter("type", type)
                , new QueryParameter("name", name)
                );

            HttpResponseMessage response = _conn.Get($"ManagedSystems{queryParams}");
            ManagedSystemsPagedResult result = new ManagedSystemsPagedResult(response);
            return result;
        }

        /// <summary>
        /// Returns a Managed System by ID.
        /// <para>API: GET ManagedSystems/{id}</para>
        /// </summary>
        /// <param name="id">ID of the Managed System</param>
        /// <returns></returns>
        public ManagedSystemResult Get(int id)
        {
            HttpResponseMessage response = _conn.Get($"ManagedSystems/{id}");
            ManagedSystemResult result = new ManagedSystemResult(response);
            return result;
        }

        /// <summary>
        /// Returns a Managed System for the Asset referenced by ID.
        /// <para>API: GET Assets/{assetId}/ManagedSystems</para>
        /// </summary>
        /// <param name="assetId">ID of the Asset</param>
        /// <returns></returns>
        public ManagedSystemResult GetByAsset(int assetId)
        {
            HttpResponseMessage response = _conn.Get($"Assets/{assetId}/ManagedSystems");
            ManagedSystemResult result = new ManagedSystemResult(response);
            return result;
        }

        /// <summary>
        /// Returns a Managed System for the Database referenced by ID.
        /// <para>API: GET Databases/{databaseID}/ManagedSystems</para>
        /// </summary>
        /// <param name="databaseID">ID of the Database</param>
        /// <returns></returns>
        public ManagedSystemResult GetByDatabase(int databaseID)
        {
            HttpResponseMessage response = _conn.Get($"Databases/{databaseID}/ManagedSystems");
            ManagedSystemResult result = new ManagedSystemResult(response);
            return result;
        }

        /// <summary>
        /// Returns a Managed System for the Directory referenced by ID.
        /// <para>API: GET Directories/{directoryID}/ManagedSystems</para>
        /// </summary>
        /// <param name="directoryID">ID of the Directory</param>
        /// <returns></returns>
        public ManagedSystemResult GetByDirectory(int directoryID)
        {
            HttpResponseMessage response = _conn.Get($"Directories/{directoryID}/ManagedSystems");
            ManagedSystemResult result = new ManagedSystemResult(response);
            return result;
        }

        /// <summary>
        /// Returns a list of Managed Systems Auto-Managed by a Functional Account.
        /// <para>API: GET FunctionalAccounts/{accountId}/ManagedSystems</para>
        /// </summary>
        /// <param name="accountId">ID of the Functional Account</param>
        /// <returns></returns>
        public ManagedSystemsResult GetByFunctionalAccount(int accountId)
        {
            HttpResponseMessage response = _conn.Get($"FunctionalAccounts/{accountId}/ManagedSystems");
            ManagedSystemsResult result = new ManagedSystemsResult(response);
            return result;
        }

        /// <summary>
        /// Returns a paged list of Managed Systems Auto-Managed by a Functional Account.
        /// <para>API: GET FunctionalAccounts/{accountId}/ManagedSystems</para>
        /// </summary>
        /// <param name="accountId">ID of the Functional Account</param>
        /// <returns></returns>
        public ManagedSystemsPagedResult GetByFunctionalAccount(int accountId, int limit, int? offset = null)
        {
            string queryParams = QueryParameterBuilder.Build(
                  new QueryParameter("limit", limit)
                , new QueryParameter("offset", offset)
                );

            HttpResponseMessage response = _conn.Get($"FunctionalAccounts/{accountId}/ManagedSystems{queryParams}");
            ManagedSystemsPagedResult result = new ManagedSystemsPagedResult(response);
            return result;
        }

        /// <summary>
        /// Returns a list of Managed Systems by Smart Rule ID.
        /// <para>API: GET SmartRules/{id}/ManagedSystems</para>
        /// </summary>
        public ManagedSystemsResult GetBySmartRule(int smartRuleID)
        {
            HttpResponseMessage response = _conn.Get($"SmartRules/{smartRuleID}/ManagedSystems");
            ManagedSystemsResult result = new ManagedSystemsResult(response);
            return result;
        }

        /// <summary>
        /// Returns a list of Managed Systems by Workgroup ID.
        /// <para>API: GET Workgroups/{id}/ManagedSystems</para>
        /// </summary>
        public ManagedSystemsResult GetByWorkgroup(int workgroupID)
        {
            HttpResponseMessage response = _conn.Get($"Workgroups/{workgroupID}/ManagedSystems");
            ManagedSystemsResult result = new ManagedSystemsResult(response);
            return result;
        }

        /// <summary>
        /// Creates a Managed System in the Workgroup referenced by ID.
        /// <para>API: POST Workgroups/{id}/ManagedSystems</para>
        /// </summary>
        /// <param name="workgroupID">ID of the Workgroup</param>
        /// <param name="model">The Managed System model</param>
        /// <returns></returns>
        public ManagedSystemResult PostInWorkgroup(int workgroupID, ManagedSystemModel model)
        {
            HttpResponseMessage response = _conn.Post($"Workgroups/{workgroupID}/ManagedSystems", model);
            ManagedSystemResult result = new ManagedSystemResult(response);
            return result;
        }

        /// <summary>
        /// Creates a Managed System for the Asset referenced by ID.
        /// <para>API: POST Assets/{assetId}/ManagedSystems</para>
        /// </summary>
        /// <param name="assetId">ID of the Asset</param>
        /// <param name="model">The Managed System model</param>
        /// <returns></returns>
        public ManagedSystemResult Post(int assetId, ManagedSystemModel model)
        {
            HttpResponseMessage response = _conn.Post($"Assets/{assetId}/ManagedSystems", model);
            ManagedSystemResult result = new ManagedSystemResult(response);
            return result;
        }

        /// <summary>
        /// Creates a Managed System for the Database referenced by ID.
        /// <para>API: POST Databases/{databaseID}/ManagedSystems</para>
        /// </summary>
        /// <param name="databaseID">ID of the Database</param>
        /// <param name="model">The Managed System model</param>
        /// <returns></returns>
        public ManagedSystemResult PostByDatabase(int databaseID, ManagedSystemModel model)
        {
            HttpResponseMessage response = _conn.Post($"Databases/{databaseID}/ManagedSystems", model);
            ManagedSystemResult result = new ManagedSystemResult(response);
            return result;
        }

        /// <summary>
        /// Returns a Managed System for the Directory referenced by ID.
        /// <para>API: POST Directories/{databaseID}/ManagedSystems</para>
        /// </summary>
        /// <param name="directoryID">ID of the Directory</param>
        /// <returns></returns>
        public ManagedSystemResult PostByDirectory(int directoryID)
        {
            HttpResponseMessage response = _conn.Post($"Directories/{directoryID}/ManagedSystems");
            ManagedSystemResult result = new ManagedSystemResult(response);
            return result;
        }

        /// <summary>
        /// Updates an existing Managed System by ID.
        /// <para>API: PUT ManagedSystems/{id</para>
        /// </summary>
        /// <param name="id">ID of the Managed System</param>
        /// <returns></returns>
        public ManagedSystemResult Put(int id, ManagedSystemModel model)
        {
            HttpResponseMessage response = _conn.Put($"ManagedSystems/{id}", model);
            ManagedSystemResult result = new ManagedSystemResult(response);
            return result;
        }

        /// <summary>
        /// Deletes a Managed System (unmanages the associated Asset) by ID.
        /// <para>API: DELETE ManagedSystems/{id}</para>
        /// </summary>
        /// <param name="id">ID of the Managed System</param>
        /// <returns></returns>
        public DeleteResult Delete(int id)
        {
            HttpResponseMessage response = _conn.Delete($"ManagedSystems/{id}");
            DeleteResult result = new DeleteResult(response);
            return result;
        }

    }
}
