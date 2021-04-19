using System.Net.Http;

namespace BeyondTrust.BeyondInsight.PasswordSafe.API.Client.V3
{
    public sealed class DSSKeyRulesEndpoint : BaseEndpoint
    {
        internal DSSKeyRulesEndpoint(PasswordSafeAPIConnector client)
            : base(client)
        {
        }

        /// <summary>
        /// Returns a list of DSS Key Rules.
        /// <para>API: GET DSSKeyRules</para>
        /// </summary>
        /// <returns></returns>
        public DSSKeyRulesResult GetAll()
        {
            HttpResponseMessage response = _conn.Get("DSSKeyRules");
            DSSKeyRulesResult result = new DSSKeyRulesResult(response);
            return result;
        }

        /// <summary>
        /// Returns a DSS Key Rule by ID.
        /// <para>API: GET DSSKeyRules/{id}</para>
        /// </summary>
        /// <param name="id">ID of the DSS Key Rule</param>
        /// <returns></returns>
        public DSSKeyRuleResult Get(int id)
        {
            HttpResponseMessage response = _conn.Get(string.Format("DSSKeyRules/{0}", id));
            DSSKeyRuleResult result = new DSSKeyRuleResult(response);
            return result;
        }

    }
}
