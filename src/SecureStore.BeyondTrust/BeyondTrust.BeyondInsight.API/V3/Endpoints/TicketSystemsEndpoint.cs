using System.Net.Http;

namespace BeyondTrust.BeyondInsight.PasswordSafe.API.Client.V3
{
    public sealed class TicketSystemsEndpoint : BaseEndpoint
    {
        internal TicketSystemsEndpoint(PasswordSafeAPIConnector client)
            : base(client)
        {
        }

        /// <summary>
        /// List registered Ticket Systems.
        /// <para>API: GET TicketSystems</para>
        /// </summary>
        /// <returns></returns>
        public TicketSystemsResult GetAll()
        {
            HttpResponseMessage response = _conn.Get("TicketSystems");
            TicketSystemsResult result = new TicketSystemsResult(response);
            return result;
        }
        
    }
}
