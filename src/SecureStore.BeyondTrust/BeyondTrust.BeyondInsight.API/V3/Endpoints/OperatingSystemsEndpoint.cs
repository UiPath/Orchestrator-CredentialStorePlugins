using System.Net.Http;

namespace BeyondTrust.BeyondInsight.PasswordSafe.API.Client.V3
{
    public sealed class OperatingSystemsEndpoint : BaseEndpoint
    {
        internal OperatingSystemsEndpoint(PasswordSafeAPIConnector client)
            : base(client)
        {
        }

        /// <summary>
        /// Returns a list of Operating Systems.
        /// <para>API: GET OperatingSystems</para>
        /// </summary>
        /// <returns></returns>
        public OperatingSystemsResult GetAll()
        {
            HttpResponseMessage response = _conn.Get("OperatingSystems");
            OperatingSystemsResult result = new OperatingSystemsResult(response);
            return result;
        }

    }
}
