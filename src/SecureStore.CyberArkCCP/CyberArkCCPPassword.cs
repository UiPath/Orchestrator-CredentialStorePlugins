namespace UiPath.Orchestrator.Extensions.SecureStores.CyberArkCCP
{
    public sealed class CyberArkCCPPassword
    {
        /// <summary>
        /// A Cyberark response content (the password).
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// A Cyberark response: safe.
        /// </summary>
        public string Safe { get; set; }

        /// <summary>
        /// A Cyberark response: KeyName.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// A Cyberark response: folder.
        /// </summary>
        public string Folder { get; set; }

        /// <summary>
        /// A Cyberark response: is password change in progress?
        /// </summary>
        public string PasswordChangeInProcess { get; set; }
    }
    
}
