using Newtonsoft.Json;
using System;
using UiPath.Orchestrator.Extensibility.SecureStores;
using UiPath.Orchestrator.Extensions.SecureStores.HashicorpVault.Resources;

namespace UiPath.Orchestrator.Extensions.SecureStores.HashicorpVault
{
    public class HashicorpVaultContextBuilder
    {
        private HashicorpVaultContext _context;

        public HashicorpVaultContextBuilder FromJson(string json)
        {
            json = json ?? throw new SecureStoreException(
                SecureStoreException.Type.InvalidConfiguration,
                nameof(Resource.HashicorpVaultJsonInvalidOrMissing));

            try
            {
                _context = JsonConvert.DeserializeObject<HashicorpVaultContext>(json);
            }
            catch (Exception)
            {
                // Ignore, we handle wrong json lower
            }

            _context = _context ?? throw new SecureStoreException(
                SecureStoreException.Type.InvalidConfiguration,
                nameof(Resource.HashicorpVaultJsonInvalidOrMissing));

            return this;
        }

        public HashicorpVaultContext Build()
        {
            if (_context == null)
            {
                throw new Exception("Invalid usage");
            }

            if (_context.VaultUri == null || !_context.VaultUri.IsAbsoluteUri)
            {
                throw new SecureStoreException(
                    SecureStoreException.Type.InvalidConfiguration,
                    HashicorpVaultUtils.GetLocalizedResource(nameof(Resource.HashicorpVaultSettingInvalidOrMissing), nameof(_context.VaultUri)));
            }

            switch (_context.AuthenticationType)
            {
                case AuthenticationType.None:
                    throw new SecureStoreException(
                        SecureStoreException.Type.InvalidConfiguration,
                        HashicorpVaultUtils.GetLocalizedResource(nameof(Resource.HashicorpVaultSettingInvalidOrMissing), nameof(_context.AuthenticationType)));
                case AuthenticationType.AppRole:
                    if (string.IsNullOrEmpty(_context.RoleId))
                    {
                        throw new SecureStoreException(
                            SecureStoreException.Type.InvalidConfiguration,
                            HashicorpVaultUtils.GetLocalizedResource(nameof(Resource.HashicorpVaultSettingInvalidOrMissing), nameof(_context.RoleId)));
                    }

                    if (string.IsNullOrEmpty(_context.SecretId))
                    {
                        throw new SecureStoreException(
                            SecureStoreException.Type.InvalidConfiguration,
                            HashicorpVaultUtils.GetLocalizedResource(nameof(Resource.HashicorpVaultSettingInvalidOrMissing), nameof(_context.SecretId)));
                    }

                    break;
                case AuthenticationType.UsernamePassword:
                case AuthenticationType.Ldap:
                    if (string.IsNullOrEmpty(_context.Username))
                    {
                        throw new SecureStoreException(
                            SecureStoreException.Type.InvalidConfiguration,
                            HashicorpVaultUtils.GetLocalizedResource(nameof(Resource.HashicorpVaultSettingInvalidOrMissing), nameof(_context.Username)));
                    }

                    if (string.IsNullOrEmpty(_context.Password))
                    {
                        throw new SecureStoreException(
                            SecureStoreException.Type.InvalidConfiguration,
                            HashicorpVaultUtils.GetLocalizedResource(nameof(Resource.HashicorpVaultSettingInvalidOrMissing), nameof(_context.Password)));
                    }

                    break;
                case AuthenticationType.Token:

                    if (string.IsNullOrEmpty(_context.Token))
                    {
                        throw new SecureStoreException(
                            SecureStoreException.Type.InvalidConfiguration,
                            HashicorpVaultUtils.GetLocalizedResource(nameof(Resource.HashicorpVaultSettingInvalidOrMissing), nameof(_context.Token)));
                    }

                    break;
                default:
                    throw new SecureStoreException(
                        SecureStoreException.Type.InvalidConfiguration,
                        HashicorpVaultUtils.GetLocalizedResource(nameof(Resource.HashicorpVaultSettingInvalidOrMissing), nameof(_context.AuthenticationType)));
            }

            if (_context.SecretsEngine == SecretsEngine.None)
            {
                throw new SecureStoreException(
                    SecureStoreException.Type.InvalidConfiguration,
                    HashicorpVaultUtils.GetLocalizedResource(nameof(Resource.HashicorpVaultSettingInvalidOrMissing), nameof(_context.SecretsEngine)));
            }

            return _context;
        }
    }
}
