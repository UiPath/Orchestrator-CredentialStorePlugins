using System.Threading.Tasks;
using UiPath.Orchestrator.Extensibility.SecureStores;

namespace UiPath.Orchestrator.Extensions.SecureStores.HashicorpVault
{
    public interface IHashicorpVaultClient
    {
        Task<string> GetSecretAsync(string secretName);

        Task<string> SetSecretAsync(string secretName, string secretValue);

        Task DeleteSecretAsync(string secretName, bool destroy = false);

        Task<Credential> GetCredentialAsync(string secretName);

        Task<string> SetCredentialAsync(string secretName, Credential credential);

        Task DeleteCredentialAsync(string secretName, bool destroy = false);
    }
}
