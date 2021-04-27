using System.Collections.Generic;
using BeyondTrust.BeyondInsight.PasswordSafe.API.Client.V3;
using UiPath.Orchestrator.Extensibility.SecureStores;

namespace UiPath.Orchestrator.BeyondTrust
{
    public sealed class BeyondTrustVaultClient : PasswordSafeAPIClient
    {
        private readonly Dictionary<string, object> config;

        public BeyondTrustVaultClient(Dictionary<string, object> config) : base(config["Hostname"].ToString() + "/BeyondTrust/api/public/v3/") => this.config = config;

        public void SignIn()
        {
            var signInResult = Auth.SignAppIn(config["AuthKey"].ToString(), config["RunAs"].ToString(), !(bool)config["SSLEnabled"]);
            if (!signInResult.IsSuccess)
            {
                throw new SecureStoreException(SecureStoreException.Type.UnauthorizedOperation, "Authentication error");
            }
        }

        public void SignOut()
        {
            Auth.SignOut();
        }

        public void TestConnection()
        {
            SignIn();
            SignOut();
        }
    }
}
