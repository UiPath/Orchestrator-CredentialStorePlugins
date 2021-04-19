namespace BeyondTrust.BeyondInsight.PasswordSafe.API.Client.V3
{
    /// <summary>
    /// BeyondInsight and Password Safe (v6.4.4+) API v3 Client.
    /// </summary>
    public class PasswordSafeAPIClient
    {
        private readonly PasswordSafeAPIConnector _connector = null;

        /// <summary>
        /// Constructor for <seealso cref="PasswordSafeAPIClient"/>.
        /// </summary>
        /// <param name="baseUrl">
        /// The base URL of the Password Safe API, including an explicit placeholder for version number.
        /// <para>i.e. <example>https://the-url/BeyondTrust/api/public/v{0}</example></para>
        /// </param>
        public PasswordSafeAPIClient(string baseUrl)
        {
            BaseUrl = string.Format(baseUrl, this.APIVersion);
            _connector = new PasswordSafeAPIConnector(BaseUrl);

            Auth = new AuthEndpoint(_connector);
            Configuration = new ConfigurationEndpoint(_connector);
            AccessLevels = new AccessLevelsEndpoint(_connector);
            AccessPolicies = new AccessPoliciesEndpoint(_connector);
            AddressGroups = new AddressGroupsEndpoint(_connector);
            Addresses = new AddressesEndpoint(_connector);
            Aliases = new AliasesEndpoint(_connector);
            Applications = new ApplicationsEndpoint(_connector);
            Assets = new AssetsEndpoint(_connector);
            AttributeTypes = new AttributeTypesEndpoint(_connector);
            Attributes = new AttributesEndpoint(_connector);
            Credentials = new CredentialsEndpoint(_connector);
            Databases = new DatabasesEndpoint(_connector);
            Directories = new DirectoriesEndpoint(_connector);
            DSSKeyRules = new DSSKeyRulesEndpoint(_connector);
            EntityTypes = new EntityTypesEndpoint(_connector);
            FunctionalAccounts = new FunctionalAccountsEndpoint(_connector);
            Imports = new ImportsEndpoint(_connector);
            ISARequests = new ISARequestsEndpoint(_connector);
            ISASessions = new ISASessionsEndpoint(_connector);
            Keystrokes = new KeystrokesEndpoint(_connector);
            LinkedAccounts = new LinkedAccountsEndpoint(_connector);
            ManagedAccounts = new ManagedAccountsEndpoint(_connector);
            ManagedSystems = new ManagedSystemsEndpoint(_connector);
            Nodes = new NodesEndpoint(_connector);
            OperatingSystems = new OperatingSystemsEndpoint(_connector);
            OracleInternetDirectories = new OracleInternetDirectoriesEndpoint(_connector);
            PasswordRules = new PasswordRulesEndpoint(_connector);
            Permissions = new PermissionsEndpoint(_connector);
            Platforms = new PlatformsEndpoint(_connector);
            QuickRules = new QuickRulesEndpoint(_connector);
            Replay = new ReplayEndpoint(_connector);
            Requests = new RequestsEndpoint(_connector);
            RequestSets = new RequestSetsEndpoint(_connector);
            Roles = new RolesEndpoint(_connector);
            Sessions = new SessionsEndpoint(_connector);
            SmartRules = new SmartRulesEndpoint(_connector);
            SyncedAccounts = new SyncedAccountsEndpoint(_connector);
            TicketSystems = new TicketSystemsEndpoint(_connector);
            UserGroups = new UserGroupsEndpoint(_connector);
            Users = new UsersEndpoint(_connector);
            Vulnerabilities = new VulnerabilitiesEndpoint(_connector);
            Workgroups = new WorkgroupsEndpoint(_connector);
        }

        #region Public Properties

        /// <summary>
        /// The base URL of the API.
        /// </summary>
        public string BaseUrl { get; private set; }

        /// <summary>
        /// Returns the version of the Password Safe API supported by this class.
        /// </summary>
        public int APIVersion
        {
            get { return 3; }
        }

        #endregion

        #region Endpoints

        /// <summary>
        /// Auth
        /// </summary>
        public AuthEndpoint Auth { get; private set; }

        /// <summary>
        /// Configuration
        /// </summary>
        public ConfigurationEndpoint Configuration { get; private set; }

        /// <summary>
        /// AccessLevels
        /// </summary>
        public AccessLevelsEndpoint AccessLevels { get; private set; }

        /// <summary>
        /// AccessPolicies
        /// </summary>
        public AccessPoliciesEndpoint AccessPolicies { get; private set; }

        /// <summary>
        /// AddressGroups
        /// </summary>
        public AddressGroupsEndpoint AddressGroups { get; private set; }

        /// <summary>
        /// Addresses
        /// </summary>
        public AddressesEndpoint Addresses { get; private set; }

        /// <summary>
        /// Aliases
        /// </summary>
        public AliasesEndpoint Aliases { get; private set; }

        /// <summary>
        /// Applications
        /// </summary>
        public ApplicationsEndpoint Applications { get; private set; }

        /// <summary>
        /// Assets
        /// </summary>
        public AssetsEndpoint Assets { get; private set; }

        /// <summary>
        /// AttributeTypes
        /// </summary>
        public AttributeTypesEndpoint AttributeTypes { get; private set; }

        /// <summary>
        /// Attributes
        /// </summary>
        public AttributesEndpoint Attributes { get; private set; }

        /// <summary>
        /// Credentials
        /// </summary>
        public CredentialsEndpoint Credentials { get; private set; }

        /// <summary>
        /// Databases
        /// </summary>
        public DatabasesEndpoint Databases { get; private set; }

        /// <summary>
        /// Directories
        /// </summary>
        public DirectoriesEndpoint Directories { get; private set; }

        /// <summary>
        /// DSSKeyRules
        /// </summary>
        public DSSKeyRulesEndpoint DSSKeyRules { get; private set; }

        /// <summary>
        /// EntityTypes
        /// </summary>
        public EntityTypesEndpoint EntityTypes { get; private set; }

        /// <summary>
        /// FunctionalAccounts
        /// </summary>
        public FunctionalAccountsEndpoint FunctionalAccounts { get; private set; }

        /// <summary>
        /// Imports
        /// </summary>
        public ImportsEndpoint Imports { get; private set; }

        /// <summary>
        /// ISARequests
        /// </summary>
        public ISARequestsEndpoint ISARequests { get; private set; }

        /// <summary>
        /// ISASessions
        /// </summary>
        public ISASessionsEndpoint ISASessions { get; private set; }

        /// <summary>
        /// Keystrokes
        /// </summary>
        public KeystrokesEndpoint Keystrokes { get; private set; }

        /// <summary>
        /// LinkedAccounts
        /// </summary>
        public LinkedAccountsEndpoint LinkedAccounts { get; private set; }

        /// <summary>
        /// ManagedAccounts
        /// </summary>
        public ManagedAccountsEndpoint ManagedAccounts { get; private set; }

        /// <summary>
        /// ManagedSystems
        /// </summary>
        public ManagedSystemsEndpoint ManagedSystems { get; private set; }

        /// <summary>
        /// Nodes
        /// </summary>
        public NodesEndpoint Nodes { get; private set; }

        /// <summary>
        /// OperatingSystems
        /// </summary>
        public OperatingSystemsEndpoint OperatingSystems { get; private set; }

        /// <summary>
        /// OracleInternetDirectories
        /// </summary>
        public OracleInternetDirectoriesEndpoint OracleInternetDirectories { get; private set; }

        /// <summary>
        /// PasswordRules
        /// </summary>
        public PasswordRulesEndpoint PasswordRules { get; private set; }

        /// <summary>
        /// Permissions
        /// </summary>
        public PermissionsEndpoint Permissions { get; private set; }

        /// <summary>
        /// Platforms
        /// </summary>
        public PlatformsEndpoint Platforms { get; private set; }

        /// <summary>
        /// QuickRules
        /// </summary>
        public QuickRulesEndpoint QuickRules { get; private set; }

        /// <summary>
        /// pbsm/Replay
        /// </summary>
        public ReplayEndpoint Replay { get; private set; }

        /// <summary>
        /// Requests
        /// </summary>
        public RequestsEndpoint Requests { get; private set; }

        /// <summary>
        /// RequestSets
        /// </summary>
        public RequestSetsEndpoint RequestSets { get; private set; }

        /// <summary>
        /// Roles
        /// </summary>
        public RolesEndpoint Roles { get; private set; }

        /// <summary>
        /// Sessions
        /// </summary>
        public SessionsEndpoint Sessions { get; private set; }

        /// <summary>
        /// SmartRules
        /// </summary>
        public SmartRulesEndpoint SmartRules { get; private set; }

        /// <summary>
        /// SyncedAccounts
        /// </summary>
        public SyncedAccountsEndpoint SyncedAccounts { get; private set; }

        /// <summary>
        /// TicketSystems
        /// </summary>
        public TicketSystemsEndpoint TicketSystems { get; private set; }

        /// <summary>
        /// UserGroups
        /// </summary>
        public UserGroupsEndpoint UserGroups { get; private set; }

        /// <summary>
        /// Users
        /// </summary>
        public UsersEndpoint Users { get; private set; }

        /// <summary>
        /// Vulnerabilities
        /// </summary>
        public VulnerabilitiesEndpoint Vulnerabilities { get; private set; }

        /// <summary>
        /// Workgroups
        /// </summary>
        public WorkgroupsEndpoint Workgroups { get; private set; }

        #endregion

    }

}
