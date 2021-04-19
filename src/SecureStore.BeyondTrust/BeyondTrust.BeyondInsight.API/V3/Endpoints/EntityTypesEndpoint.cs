using System.Net.Http;

namespace BeyondTrust.BeyondInsight.PasswordSafe.API.Client.V3
{
    public sealed class EntityTypesEndpoint : BaseEndpoint
    {
        internal EntityTypesEndpoint(PasswordSafeAPIConnector client)
            : base(client)
        {
        }

        /// <summary>
        /// Returns a list of Entity Types.
        /// <para>API: GET EntityTypes</para>
        /// </summary>
        /// <returns></returns>
        public EntityTypesResult GetAll()
        {
            HttpResponseMessage response = _conn.Get("EntityTypes");
            EntityTypesResult result = new EntityTypesResult(response);
            return result;
        }

    }
}
