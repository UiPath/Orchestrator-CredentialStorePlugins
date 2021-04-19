using System.Net.Http;

namespace BeyondTrust.BeyondInsight.PasswordSafe.API.Client.V3
{
    public sealed class PlatformsEndpoint : BaseEndpoint
    {
        internal PlatformsEndpoint(PasswordSafeAPIConnector client)
            : base(client)
        {
        }

        /// <summary>
        /// Returns a list of Platforms for Managed Systems.
        /// <para>API: GET Platforms</para>
        /// </summary>
        /// <returns></returns>
        public PlatformsResult GetAll()
        {
            HttpResponseMessage response = _conn.Get("Platforms");
            PlatformsResult result = new PlatformsResult(response);
            return result;
        }

        /// <summary>
        /// Returns a Platform by ID.
        /// <para>API: GET Platforms/{id}</para>
        /// </summary>
        /// <param name="id">ID of the Platform</param>
        /// <returns></returns>
        public PlatformResult Get(int id)
        {
            HttpResponseMessage response = _conn.Get($"Platforms/{id}");
            PlatformResult result = new PlatformResult(response);
            return result;
        }

        /// <summary>
        /// Returns a list of Platforms by Entity Type ID.
        /// <para>API: GET EntityTypes/{id}/Platforms</para>
        /// </summary>
        /// <param name="id">ID of the Entity Type</param>
        /// <returns></returns>
        public PlatformsResult GetByEntityType(int id)
        {
            HttpResponseMessage response = _conn.Get($"EntityTypes/{id}/Platforms");
            PlatformsResult result = new PlatformsResult(response);
            return result;
        }

    }
}
