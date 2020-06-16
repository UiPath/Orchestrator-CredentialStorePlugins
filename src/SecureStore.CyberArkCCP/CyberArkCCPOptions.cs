namespace UiPath.Orchestrator.Extensions.SecureStores.CyberArkCCP
{
    public sealed class CyberArkCCPOptions
    {
        /// <summary>
        /// A Cyberark CCP URL.
        /// </summary>
        public string URL { get; set; }

        /// <summary>
        /// A Cyberark authentication certificate thumbprint.
        /// </summary>
        public string ClientCertificateThumbprint { get; set; }

        /// <summary>
        /// A Cyberark login element used by a custom application
        /// </summary>
        public string ApplicationId { get; set; }

        /// <summary>
        /// A Cyberark grouping element
        /// </summary>
        public string Safe { get; set; }

        /// <summary>
        /// A Cyberark sub-group of a Safe.
        /// </summary>
        public string Folder { get; set; }

        /// <summary>
        /// A Cyberark personal store certificate authority thumbprint.
        /// </summary>
        public string CertificateAuthorityThumbprint { get; set; }
    }
}
