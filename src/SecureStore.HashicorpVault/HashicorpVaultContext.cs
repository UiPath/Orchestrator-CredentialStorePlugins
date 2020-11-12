using System;

namespace UiPath.Orchestrator.Extensions.SecureStores.HashicorpVault
{
    public enum AuthenticationType
    {
        None = 0,
        AppRole,
        UsernamePassword,
        Ldap,
        ClientCertificate,
        Token,
    }

    public enum SecretsEngine
    {
        None = 0,
        KeyValueV1,
        KeyValueV2,
        ActiveDirectory,
        Cubbyhole,
    }

    public class HashicorpVaultContext
    {
        public Uri VaultUri { get; set; }

        public AuthenticationType AuthenticationType { get; set; }

        public string RoleId { get; set; }

        public string SecretId { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Certificate { get; set; }

        public string CertificatePassword { get; set; }

        public string Token { get; set; }

        public SecretsEngine SecretsEngine { get; set; }

        public string Namespace { get; set; }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (VaultUri != null ? VaultUri.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (int) AuthenticationType;
                hashCode = (hashCode * 397) ^ (RoleId != null ? RoleId.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (SecretId != null ? SecretId.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Username != null ? Username.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Password != null ? Password.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Certificate != null ? Certificate.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (CertificatePassword != null ? CertificatePassword.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Token != null ? Token.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (int) SecretsEngine;
                hashCode = (hashCode * 397) ^ (Namespace != null ? Namespace.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}
