using System.Net.Http;

namespace BeyondTrust.BeyondInsight.PasswordSafe.API.Client.V3
{
    public sealed class AccessPoliciesEndpoint : BaseEndpoint
    {
        internal AccessPoliciesEndpoint(PasswordSafeAPIConnector client)
            : base(client)
        {
        }

        /// <summary>
        /// Returns a list of Password Safe Access Policies.
        /// <para>API: GET AccessPolicies</para>
        /// </summary>
        /// <returns></returns>
        public AccessPoliciesGetResult GetAll()
        {
            HttpResponseMessage response = _conn.Get("AccessPolicies");
            AccessPoliciesGetResult result = new AccessPoliciesGetResult(response);
            return result;
        }

        /// <summary>
        /// Tests access to a Managed Account and returns a list of Password Safe Access Policies that are available in the request window.
        /// <para>API: POST AccessPolicies/Test</para>
        /// </summary>
        /// <param name="systemID">The system ID for the request.</param>
        /// <param name="accountID">The account ID for the request.</param>
        /// <param name="durationInMinutes">The duration (in minutes) for the request.</param>
        /// <returns></returns>
        public AccessPoliciesTestResult Test(int systemID, int accountID, int durationInMinutes)
        {
            AccessPolicyTestModel body = new AccessPolicyTestModel()
            {
                AccountID = accountID,
                SystemID = systemID,
                DurationMinutes = durationInMinutes,
            };

            HttpResponseMessage response = _conn.Post("AccessPolicies/Test", body);
            AccessPoliciesTestResult result = new AccessPoliciesTestResult(response);
            return result;
        }

    }
}
