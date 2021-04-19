using System.Net.Http;

namespace BeyondTrust.BeyondInsight.PasswordSafe.API.Client.V3
{
    public sealed class VulnerabilitiesEndpoint : BaseEndpoint
    {
        internal VulnerabilitiesEndpoint(PasswordSafeAPIConnector client)
            : base(client)
        {
        }

        /// <summary>
        /// Returns a list of Vulnerabilities by Asset ID, optionally including Temporal Metrics related to the Asset and Smart Rule referenced by ID.
        /// <para>API: GET Assets/{id}/Vulnerabilities?smartRuleID={srID}</para>
        /// </summary>
        /// <param name="id">ID of the Asset</param>
        /// <param name="smartRuleID"></param>
        /// <param name="delta"></param>
        /// <param name="includeReferences"></param>
        /// <returns></returns>
        public VulnerabilitiesResult Get(int id, int? smartRuleID = null, string delta = null, bool? includeReferences = null)
        {
            string queryParams = QueryParameterBuilder.Build(
              new QueryParameter("smartRuleID", smartRuleID)
            , new QueryParameter("delta", delta)
            , new QueryParameter("includeReferences", includeReferences)
            );

            HttpResponseMessage response = _conn.Get($"Assets/{id}/Vulnerabilities{queryParams}");
            VulnerabilitiesResult result = new VulnerabilitiesResult(response);
            return result;
        }
        
    }
}
