using System.Net.Http;

namespace BeyondTrust.BeyondInsight.PasswordSafe.API.Client.V3
{
    public sealed class ConfigurationEndpoint : BaseEndpoint
    {
        internal ConfigurationEndpoint(PasswordSafeAPIConnector client)
            : base(client)
        {
        }

        /// <summary>
        /// Returns the current system version.
        /// <para>API: GET Configuration/Version</para>
        /// </summary>
        public ConfigurationVersionResult GetVersion()
        {
            HttpResponseMessage response = _conn.Get("Configuration/Version");
            ConfigurationVersionResult result = new ConfigurationVersionResult(response);
            return result;
        }

    }
}
