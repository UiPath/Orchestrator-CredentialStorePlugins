namespace BeyondTrust.BeyondInsight.PasswordSafe.API.Client.V3
{
    public abstract class BaseEndpoint
    {
        internal readonly PasswordSafeAPIConnector _conn;
        internal BaseEndpoint(PasswordSafeAPIConnector client)
        {
            _conn = client;
        }
    }
}
