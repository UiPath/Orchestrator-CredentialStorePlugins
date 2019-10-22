using System.Threading;
using System.Threading.Tasks;

namespace UiPath.Orchestrator.Extensions.SecureStores.AzureKeyVault
{
    public interface IAzureKeyVaultClient
    {
        Task<string> GetSecretAsync(string secretName, CancellationToken cancellationToken = default);

        Task<string> SetSecretAsync(string secretName, string secretValue, CancellationToken cancellationToken = default);

        Task DeleteSecretAsync(string secretName, CancellationToken cancellationToken = default);
    }
}
