namespace UiPath.Samples.SecureStores.SqlPasswordStore
{
    public class SqlPassContext
    {
        public string Driver { get; set; }

        public string Server { get; set; }

        public string UserId { get; set; }

        public string Password { get; set; }

        public bool? TrustedConnection { get; set; }

        public string Database { get; set; }

        public string TableName { get; set; }

        public bool? IsEncrypted { get; set; }
    }
}
