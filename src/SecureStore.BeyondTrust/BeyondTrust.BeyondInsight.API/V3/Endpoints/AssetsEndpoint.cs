using System.Net.Http;

namespace BeyondTrust.BeyondInsight.PasswordSafe.API.Client.V3
{
    public sealed class AssetsEndpoint : BaseEndpoint
    {
        internal AssetsEndpoint(PasswordSafeAPIConnector client)
            : base(client)
        {
        }

        /// <summary>
        /// Returns a list of Assets by Workgroup ID.
        /// <para>API: GET Workgroups/{workgroupID}/Assets</para>
        /// </summary>
        /// <param name="workgroupID">ID of the Workgroup</param>
        /// <returns></returns>
        public AssetsResult GetAll(int workgroupID)
        {
            HttpResponseMessage response = _conn.Get($"Workgroups/{workgroupID}/Assets");
            AssetsResult result = new AssetsResult(response);
            return result;
        }

        /// <summary>
        /// Returns a list of Assets by Workgroup name.
        /// <para>API: GET Workgroups/{workgroupName}/Assets</para>
        /// </summary>
        /// <param name="workgroupName">Name of the Workgroup</param>
        /// <returns></returns>
        public AssetsResult GetAll(string workgroupName)
        {
            HttpResponseMessage response = _conn.Get($"Workgroups/{workgroupName}/Assets");
            AssetsResult result = new AssetsResult(response);
            return result;
        }

        /// <summary>
        /// Returns a paged list of Assets by Workgroup ID.
        /// <para>API: GET Workgroups/{workgroupID}/Assets?limit={limit}&offset={offset}</para>
        /// </summary>
        /// <param name="workgroupID">ID of the Workgroup</param>
        /// <param name="limit">Number of records to return</param>
        /// <param name="offset">Number of records to skip before returning <paramref name="limit"/> records</param>
        /// <returns></returns>
        public AssetsPagedResult GetAll(int workgroupID, int limit, int? offset = null)
        {
            string queryParams = QueryParameterBuilder.Build(
                  new QueryParameter("limit", limit)
                , new QueryParameter("offset", offset)
                );

            HttpResponseMessage response = _conn.Get($"Workgroups/{workgroupID}/Assets{queryParams}");
            AssetsPagedResult result = new AssetsPagedResult(response);
            return result;
        }

        /// <summary>
        /// Returns a paged list of Assets by Workgroup name.
        /// <para>API: GET Workgroups/{workgroupName}/Assets?limit={limit}&offset={offset}</para>
        /// </summary>
        /// <param name="workgroupName">Name of the Workgroup</param>
        /// <param name="limit">Number of records to return</param>
        /// <param name="offset">Number of records to skip before returning <paramref name="limit"/> records</param>
        /// <returns></returns>
        public AssetsPagedResult GetAll(string workgroupName, int limit, int? offset = null)
        {
            string queryParams = QueryParameterBuilder.Build(
                  new QueryParameter("limit", limit)
                , new QueryParameter("offset", offset)
                );

            HttpResponseMessage response = _conn.Get($"Workgroups/{workgroupName}/Assets{queryParams}");
            AssetsPagedResult result = new AssetsPagedResult(response);
            return result;
        }

        /// <summary>
        /// Returns an Asset by ID.
        /// <para>API: GET Assets/{id}</para>
        /// </summary>
        /// <param name="id">ID of the Asset</param>
        /// <returns></returns>
        public AssetResult Get(int id)
        {
            HttpResponseMessage response = _conn.Get($"Assets/{id}");
            AssetResult result = new AssetResult(response);
            return result;
        }
        
        /// <summary>
        /// Returns an Asset by Workgroup name and Asset name.
        /// <para>API: GET Workgroups/{workgroupName}/Assets?name={assetName}</para>
        /// </summary>
        /// <param name="workgroupName">Name of the Workgroup</param>
        /// <param name="assetName">Name of the Asset</param>
        /// <returns></returns>
        public AssetResult Get(string workgroupName, string assetName)
        {
            HttpResponseMessage response = _conn.Get($"Workgroups/{workgroupName}/Assets?name={assetName}");
            AssetResult result = new AssetResult(response);
            return result;
        }

        /// <summary>
        /// Returns a list of Assets by Smart Rule ID.
        /// <para>API: GET SmartRules/{id}/Assets</para>
        /// </summary>
        /// <param name="smartRuleID">ID of the Smart Rule</param>
        /// <returns></returns>
        public AssetsResult GetAllBySmartRule(int smartRuleID)
        {
            HttpResponseMessage response = _conn.Get($"SmartRules/{smartRuleID}/Assets");
            AssetsResult result = new AssetsResult(response);
            return result;
        }

        /// <summary>
        /// Returns a paged list of Assets by Smart Rule ID.
        /// <para>API: GET SmartRules/{id}/Assets?limit={limit}&offset={offset}</para>
        /// </summary>
        /// <param name="smartRuleID">ID of the Smart Rule</param>
        /// <param name="limit">Number of records to return</param>
        /// <param name="offset">Number of records to skip before returning <paramref name="limit"/> records</param>
        /// <returns></returns>
        public AssetsPagedResult GetAllBySmartRule(int smartRuleID, int limit, int? offset = null)
        {
            string queryParams = QueryParameterBuilder.Build(
                  new QueryParameter("limit", limit)
                , new QueryParameter("offset", offset)
                );

            HttpResponseMessage response = _conn.Get($"SmartRules/{smartRuleID}/Assets{queryParams}");
            AssetsPagedResult result = new AssetsPagedResult(response);
            return result;
        }

        /// <summary>
        /// Creates a new Asset in the Workgroup referenced by ID.
        /// <para>API: POST Workgroups/{workgroupID}/Assets</para>
        /// </summary>
        /// <param name="workgroupID">ID of the Workgroup</param>
        /// <returns></returns>
        public AssetResult Post(int workgroupID, AssetPostModel model)
        {
            HttpResponseMessage response = _conn.Post($"Workgroups/{workgroupID}/Assets", model);
            AssetResult result = new AssetResult(response);
            return result;
        }

        /// <summary>
        /// Creates a new Asset in the Workgroup referenced by Name.
        /// <para>API: POST Workgroups/{workgroupName}/Assets</para>
        /// </summary>
        /// <param name="workgroupName">Name of the Workgroup</param>
        /// <returns></returns>
        public AssetResult Post(string workgroupName, AssetPostModel model)
        {
            HttpResponseMessage response = _conn.Post($"Workgroups/{workgroupName}/Assets", model);
            AssetResult result = new AssetResult(response);
            return result;
        }

        /// <summary>
        /// Updates an existing Asset by ID.
        /// <para>API: PUT Assets/{id}</para>
        /// </summary>
        /// <param name="id">ID of the Asset</param>
        /// <returns></returns>
        public AssetResult Put(int id, AssetPutModel model)
        {
            HttpResponseMessage response = _conn.Put($"Assets/{id}", model);
            AssetResult result = new AssetResult(response);
            return result;
        }

        /// <summary>
        /// Deletes an Asset by ID.
        /// <para>API: DELETE Assets/{id}</para>
        /// </summary>
        /// <param name="id">ID of the Asset</param>
        /// <returns></returns>
        public DeleteResult Delete(int id)
        {
            HttpResponseMessage response = _conn.Delete($"Assets/{id}");
            DeleteResult result = new DeleteResult(response);
            return result;
        }

        /// <summary>
        /// Deletes an Asset by Workgroup name and Asset name.
        /// <para>API: DELETE Workgroups/{workgroupName}/Assets?name={assetName}</para>
        /// </summary>
        /// <param name="workgroupName">Name of the Workgroup</param>
        /// <param name="assetName">Name of the Asset</param>
        /// <returns></returns>
        public DeleteResult Delete(string workgroupName, string assetName)
        {
            HttpResponseMessage response = _conn.Delete($"Workgroups/{workgroupName}/Assets?name={assetName}");
            DeleteResult result = new DeleteResult(response);
            return result;
        }

        /// <summary>
        /// Returns a list of Assets that match the given search criteria.
        /// <para>API: POST Assets/Search</para>
        /// </summary>
        /// <returns></returns>
        public AssetsResult Search(AssetModel model)
        {
            HttpResponseMessage response = _conn.Post("Assets/Search", model);
            AssetsResult result = new AssetsResult(response);
            return result;
        }

        /// <summary>
        /// Returns a paged list of Assets that match the given search criteria.
        /// <para>API: POST Assets/Search?limit={limit}&offset={offset}</para>
        /// </summary>
        /// <param name="limit">Number of records to return</param>
        /// <param name="offset">Number of records to skip before returning <paramref name="limit"/> records</param>
        /// <returns></returns>
        public AssetsPagedResult Search(AssetModel model, int limit, int? offset = null)
        {
            string queryParams = QueryParameterBuilder.Build(
                  new QueryParameter("limit", limit)
                , new QueryParameter("offset", offset)
                );

            HttpResponseMessage response = _conn.Post($"Assets/Search{queryParams}", model);
            AssetsPagedResult result = new AssetsPagedResult(response);
            return result;
        }

    }
}
