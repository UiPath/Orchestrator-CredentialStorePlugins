using System.Net.Http;

namespace BeyondTrust.BeyondInsight.PasswordSafe.API.Client.V3
{
    public sealed class NodesEndpoint : BaseEndpoint
    {
        internal NodesEndpoint(PasswordSafeAPIConnector client)
            : base(client)
        {
        }

        /// <summary>
        /// Returns a list of Session Monitoring Agent Nodes.
        /// <para>API: GET Nodes</para>
        /// </summary>
        /// <returns></returns>
        public NodesResult GetAll()
        {
            HttpResponseMessage response = _conn.Get("Nodes");
            NodesResult result = new NodesResult(response);
            return result;
        }

    }
}
