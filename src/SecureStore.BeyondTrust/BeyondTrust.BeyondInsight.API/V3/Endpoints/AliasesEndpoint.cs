using System.Net.Http;

namespace BeyondTrust.BeyondInsight.PasswordSafe.API.Client.V3
{
    public sealed class AliasesEndpoint : BaseEndpoint
    {
        internal AliasesEndpoint(PasswordSafeAPIConnector client)
            : base(client)
        {
        }

        /// <summary>
        /// Returns a list of requestable Managed Account Aliases.
        /// <para>API: GET Aliases</para>
        /// </summary>
        /// <returns></returns>
        public AliasesResult GetAll(string state = null)
        {
            HttpResponseMessage response = string.IsNullOrEmpty(state)
                ? _conn.Get("Aliases")
                : _conn.Get($"Aliases?state={state}");
            AliasesResult result = new AliasesResult(response);
            return result;
        }

        /// <summary>
        /// Returns a requestable Managed Account Alias by ID.
        /// <para>API: GET Aliases/{id}</para>
        /// </summary>
        /// <returns></returns>
        public AliasResult Get(int id)
        {
            HttpResponseMessage response = _conn.Get($"Aliases/{id}");
            AliasResult result = new AliasResult(response);
            return result;
        }

        /// <summary>
        /// Returns a requestable Managed Account Alias by name.
        /// <para>API: GET Aliases?name={name}</para>
        /// </summary>
        /// <returns></returns>
        public AliasResult Get(string name)
        {
            HttpResponseMessage response = _conn.Get($"Aliases?name={name}");
            AliasResult result = new AliasResult(response);
            return result;
        }

    }
}
