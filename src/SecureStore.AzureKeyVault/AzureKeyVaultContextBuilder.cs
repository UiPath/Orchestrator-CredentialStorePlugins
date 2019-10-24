using AzureKeyVault.SecureStore;
using Newtonsoft.Json;
using System;
using UiPath.Orchestrator.Extensibility.SecureStores;

namespace UiPath.Orchestrator.Extensions.SecureStores.AzureKeyVault
{
    public class AzureKeyVaultContextBuilder
    {
        private AzureKeyVaultContext _context;

        public AzureKeyVaultContextBuilder FromJson(string json)
        {
            json = json ?? throw new SecureStoreException(
                SecureStoreException.Type.InvalidConfiguration,
                nameof(Resource.AzureKeyVaultJsonInvalidOrMissing));

            try
            {
                _context = JsonConvert.DeserializeObject<AzureKeyVaultContext>(json);
            }
            catch (Exception)
            {
            }

            _context = _context ?? throw new SecureStoreException(
                SecureStoreException.Type.InvalidConfiguration,
                nameof(Resource.AzureKeyVaultJsonInvalidOrMissing));

            return this;
        }

        public AzureKeyVaultContext Build()
        {
            if (_context == null)
            {
                throw new Exception("Invalid usage");
            }

            if (_context.KeyVaultUri == null || !_context.KeyVaultUri.IsAbsoluteUri)
            {
                throw new SecureStoreException(
                    SecureStoreException.Type.InvalidConfiguration,
                    AzureKeyVaultUtils.GetLocalizedResource(nameof(Resource.AzureKeyVaultSettingInvalidOrMissing), nameof(_context.KeyVaultUri)));
            }

            if (string.IsNullOrEmpty(_context.ClientId))
            {
                throw new SecureStoreException(
                    SecureStoreException.Type.InvalidConfiguration,
                    AzureKeyVaultUtils.GetLocalizedResource(nameof(Resource.AzureKeyVaultSettingInvalidOrMissing), nameof(_context.ClientId)));
            }

            if (string.IsNullOrEmpty(_context.ClientSecret))
            {
                throw new SecureStoreException(
                    SecureStoreException.Type.InvalidConfiguration,
                    AzureKeyVaultUtils.GetLocalizedResource(nameof(Resource.AzureKeyVaultSettingInvalidOrMissing), nameof(_context.ClientSecret)));
            }

            return _context;
        }
    }
}
