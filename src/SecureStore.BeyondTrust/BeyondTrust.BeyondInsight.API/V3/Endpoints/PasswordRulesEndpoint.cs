using System.Net.Http;

namespace BeyondTrust.BeyondInsight.PasswordSafe.API.Client.V3
{
    public sealed class PasswordRulesEndpoint : BaseEndpoint
    {
        internal PasswordRulesEndpoint(PasswordSafeAPIConnector client)
            : base(client)
        {
        }

        /// <summary>
        /// Returns a list of Password Rules.
        /// <para>API: GET PasswordRules</para>
        /// </summary>
        /// <returns></returns>
        public PasswordRulesResult GetAll()
        {
            HttpResponseMessage response = _conn.Get("PasswordRules");
            PasswordRulesResult result = new PasswordRulesResult(response);
            return result;
        }

        /// <summary>
        /// Returns a Password Rule by ID.
        /// <para>API: GET PasswordRules/{id}</para>
        /// </summary>
        /// <param name="id">ID of the Password Rule</param>
        /// <returns></returns>
        public PasswordRuleResult Get(int id)
        {
            HttpResponseMessage response = _conn.Get(string.Format("PasswordRules/{0}", id));
            PasswordRuleResult result = new PasswordRuleResult(response);
            return result;
        }

    }
}
