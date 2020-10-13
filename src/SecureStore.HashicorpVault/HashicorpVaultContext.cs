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
    }
}
