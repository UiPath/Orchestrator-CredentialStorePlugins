using Microsoft.Azure.KeyVault;
using Microsoft.Azure.KeyVault.Models;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace UiPath.Orchestrator.Extensions.SecureStores.AzureKeyVault
{
    public class AzureKeyVaultClient : IAzureKeyVaultClient
    {
        private readonly AzureKeyVaultContext _context;

        public AzureKeyVaultClient(AzureKeyVaultContext context)
        {
            _context = context;
        }

        public async Task<string> GetSecretAsync(string secretName, CancellationToken cancellationToken = default)
        {
            SecretBundle secret;
            var keyVaultClient = new KeyVaultClient(GetAccessTokenAsync);
            secret = await keyVaultClient.GetSecretAsync(_context.KeyVaultUri.AbsoluteUri, secretName, cancellationToken);
            return secret?.Value;
        }

        public async Task<string> SetSecretAsync(string secretName, string secretValue, CancellationToken cancellationToken = default)
        {
            SecretBundle result;
            var keyVaultClient = new KeyVaultClient(GetAccessTokenAsync);
            result = await keyVaultClient.SetSecretAsync(_context.KeyVaultUri.AbsoluteUri, secretName, secretValue, cancellationToken: cancellationToken);
            return result?.SecretIdentifier.Name;
        }

        public async Task DeleteSecretAsync(string secretName, CancellationToken cancellationToken = default)
        {
            var keyVaultClient = new KeyVaultClient(GetAccessTokenAsync);
            await keyVaultClient.DeleteSecretAsync(_context.KeyVaultUri.AbsoluteUri, secretName, cancellationToken);
        }

        private async Task<string> GetAccessTokenAsync(string authority, string resource, string scope)
        {
            var authContext = new AuthenticationContext(authority);
            ClientCredential clientCredential = new ClientCredential(_context.ClientId, _context.ClientSecret);
            AuthenticationResult result = await authContext.AcquireTokenAsync(resource, clientCredential);
            if (result == null)
            {
                throw new InvalidOperationException("Failed to retrieve access token for Key Vault");
            }

            return result.AccessToken;
        }
    }
}
