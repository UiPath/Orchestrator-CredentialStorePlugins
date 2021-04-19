using System;
using System.Collections.Generic;
using System.Net.Http;

namespace BeyondTrust.BeyondInsight.PasswordSafe.API.Client.V3
{
    public sealed class ManagedAccountsEndpoint : BaseEndpoint
    {
        internal ManagedAccountsEndpoint(PasswordSafeAPIConnector client)
            : base(client)
        {
        }

        #region Managed Accounts - Role-based access (Requestor, Requestor/Approver, ISA)

        /// <summary>
        /// List of Managed Account query 'type' parameter values for Role-based API access.
        /// </summary>
        public List<string> Types = new List<string>() { "system", "domainlinked", "database", "cloud", "application" };

        /// <summary>
        /// Returns a list of Managed Accounts requestable by the current user, optionally supplying parameters.
        /// <para>API: GET ManagedAccounts</para>
        /// </summary>
        /// <returns></returns>
        public RequestableManagedAccountsResult GetAllRequestable(string type = "", string systemName = "", string accountName = "", string workgroupName = "", string applicationDisplayName = "", string ipAddress = "")
        {
            // only one of these is allowed -- if both are given, the response is a single account, so use on overload of Get instead, i.e Get(string systemName, string accountName)
            if (!string.IsNullOrEmpty(systemName) && !string.IsNullOrEmpty(accountName))
                throw new ArgumentException($"Only one of {nameof(systemName)} and {nameof(accountName)} can be supplied. Use Get(string systemName, string accountName) instead.");

            string queryParams = QueryParameterBuilder.Build(
                  new QueryParameter("type", type)
                , new QueryParameter("systemName", systemName)
                , new QueryParameter("accountName", accountName)
                , new QueryParameter("workgroupName", workgroupName)
                , new QueryParameter("applicationDisplayName", applicationDisplayName)
                , new QueryParameter("ipAddress", ipAddress)
                );

            HttpResponseMessage response = _conn.Get($"ManagedAccounts{queryParams}");
            RequestableManagedAccountsResult result = new RequestableManagedAccountsResult(response);
            return result;
        }

        /// <summary>
        /// Returns a list of Managed Accounts requestable by the current user.
        /// <para>API: GET ManagedAccounts</para>
        /// </summary>
        /// <param name="type">Type of the Managed Account to return
        /// <para>system – Returns local accounts</para>
        /// <para>domainlinked – Returns domain accounts linked to systems</para>
        /// <para>database – Returns database accounts</para>
        /// <para>cloud – Returns cloud system accounts</para>
        /// <para>application – Returns application accounts</para>
        /// </param>
        /// <returns></returns>
        public RequestableManagedAccountsResult GetAllRequestable(string type)
        {
            string queryParams = QueryParameterBuilder.Build(
                  new QueryParameter("type", type)
                );

            HttpResponseMessage response = _conn.Get($"ManagedAccounts{queryParams}");
            RequestableManagedAccountsResult result = new RequestableManagedAccountsResult(response);
            return result;
        }

        /// <summary>
        /// Returns a requestable Managed Account by Managed System name and Managed Account name.
        /// <para>API: GET ManagedAccounts?systemName={systemName}&amp;accountName={accountName}</para>
        /// </summary>
        /// <param name="systemName">Name of the Managed System</param>
        /// <param name="accountName">Name of the Managed Account</param>
        /// <param name="type">Type of the Managed Account to return
        /// <para>system – Returns local accounts</para>
        /// <para>domainlinked – Returns domain accounts linked to systems</para>
        /// <para>database – Returns database accounts</para>
        /// <para>cloud – Returns cloud system accounts</para>
        /// <para>application – Returns application accounts</para>
        /// </param>
        /// <returns></returns>
        public RequestableManagedAccountResult GetRequestable(string systemName, string accountName, string type = "")
        {
            string queryParams = QueryParameterBuilder.Build(
                  new QueryParameter("type", type)
                , new QueryParameter("systemName", systemName)
                , new QueryParameter("accountName", accountName)
                );

            HttpResponseMessage response = _conn.Get($"ManagedAccounts{queryParams}");
            RequestableManagedAccountResult result = new RequestableManagedAccountResult(response);
            return result;
        }

        /// <summary>
        /// Returns a Managed Account that can be requested by the current user by Managed System name, Managed Account name, and optionally Workgroup name.
        /// <para>API: GET ManagedAccounts?systemName={systemName}&amp;accountName={accountName}&amp;workgroupName={workgroupName}</para>
        /// </summary>
        /// <param name="systemName">Name of the Managed System</param>
        /// <param name="accountName">Name of the Managed Account</param>
        /// <param name="workgroupName">Name of the Workgroup</param>
        /// <param name="type">Type of the Managed Account to return
        /// <para>system – Returns local accounts</para>
        /// <para>domainlinked – Returns domain accounts linked to systems</para>
        /// <para>database – Returns database accounts</para>
        /// <para>cloud – Returns cloud system accounts</para>
        /// <para>application – Returns application accounts</para>
        /// </param>
        /// <returns></returns>
        public RequestableManagedAccountResult GetRequestable(string systemName, string accountName, string workgroupName, string type = "")
        {
            string queryParams = QueryParameterBuilder.Build(
                  new QueryParameter("type", type)
                , new QueryParameter("systemName", systemName)
                , new QueryParameter("accountName", accountName)
                , new QueryParameter("workgroupName", workgroupName)
                );

            HttpResponseMessage response = _conn.Get($"ManagedAccounts{queryParams}");
            RequestableManagedAccountResult result = new RequestableManagedAccountResult(response);
            return result;
        }

        /// <summary>
        /// Returns a Managed Account that can be requested by the current user by Managed System name, Managed Account name, and optionally Workgroup name and/or Application display name.
        /// <para>API: GET ManagedAccounts?systemName={systemName}&amp;accountName={accountName}&amp;workgroupName={workgroupName}</para>
        /// </summary>
        /// <param name="type">Type of the Managed Account to return
        /// <para>system – Returns local accounts</para>
        /// <para>domainlinked – Returns domain accounts linked to systems</para>
        /// <para>database – Returns database accounts</para>
        /// <para>cloud – Returns cloud system accounts</para>
        /// <para>application – Returns application accounts</para>
        /// </param>
        /// <param name="systemName">Name of the Managed System</param>
        /// <param name="accountName">Name of the Managed Account</param>
        /// <param name="workgroupName">Name of the Workgroup</param>
        /// <param name="applicationDisplayName">Display name of the Application</param>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        public RequestableManagedAccountResult GetRequestable(string type, string systemName, string accountName, string workgroupName, string applicationDisplayName, string ipAddress = "")
        {
            string queryParams = QueryParameterBuilder.Build(
                  new QueryParameter("type", type)
                , new QueryParameter("systemName", systemName)
                , new QueryParameter("accountName", accountName)
                , new QueryParameter("workgroupName", workgroupName)
                , new QueryParameter("applicationDisplayName", applicationDisplayName)
                , new QueryParameter("ipAddress", ipAddress)
                );

            HttpResponseMessage response = _conn.Get($"ManagedAccounts{queryParams}");
            RequestableManagedAccountResult result = new RequestableManagedAccountResult(response);
            return result;
        }


        /// <summary>
        /// Returns a paged list of Managed Systems.
        /// <para>API: GET ManagedSystems?limit={limit}&offset={offset}</para>
        /// </summary>
        /// <returns></returns>
        public RequestableManagedAccountsResult GetAllRequestable(int limit, int? offset = null, string type = "", string systemName = "", string accountName = "", string workgroupName = "", string applicationDisplayName = "", string ipAddress = "")
        {
            // only one of these is allowed -- if both are given, the response is a single account, so use on overload of Get instead, i.e Get(string systemName, string accountName)
            if (!string.IsNullOrEmpty(systemName) && !string.IsNullOrEmpty(accountName))
                throw new ArgumentException($"Only one of {nameof(systemName)} and {nameof(accountName)} can be supplied. Use Get(string systemName, string accountName) instead.");

            string queryParams = QueryParameterBuilder.Build(
                  new QueryParameter("limit", limit)
                , new QueryParameter("offset", offset)
                , new QueryParameter("type", type)
                , new QueryParameter("systemName", systemName)
                , new QueryParameter("accountName", accountName)
                , new QueryParameter("workgroupName", workgroupName)
                , new QueryParameter("applicationDisplayName", applicationDisplayName)
                , new QueryParameter("ipAddress", ipAddress)
                );

            HttpResponseMessage response = _conn.Get($"ManagedAccounts{queryParams}");
            RequestableManagedAccountsResult result = new RequestableManagedAccountsResult(response);
            return result;
        }

        #endregion

        #region Managed Account Provisioning

        /// <summary>
        /// List of Managed Account model versions.
        /// </summary>
        public static List<string> Versions = new List<string>() { v30, v31, v32 };
        public const string v30 = "3.0";
        public const string v31 = "3.1";
        public const string v32 = "3.2";

        /// <summary>
        /// Returns a Managed Account by ID.
        /// <para>API: GET ManagedAccounts/{id}</para>
        /// </summary>
        /// <param name="id">ID of the Managed Account</param>
        /// <returns></returns>
        public ManagedAccountResult Get(int id)
        {
            HttpResponseMessage response = _conn.Get($"ManagedAccounts/{id}");
            ManagedAccountResult result = new ManagedAccountResult(response);
            return result;
        }

        /// <summary>
        /// Returns a Managed Account by Managed System ID and Managed Account name.
        /// <para>API: GET ManagedSystems/{id}/ManagedAccounts?name={name}</para>
        /// </summary>
        public ManagedAccountResult Get(int managedSystemID, string accountName)
        {
            string queryParams = QueryParameterBuilder.Build(
                  new QueryParameter("name", accountName)
                );

            HttpResponseMessage response = _conn.Get($"ManagedSystems/{managedSystemID}/ManagedAccounts{queryParams}");
            ManagedAccountResult result = new ManagedAccountResult(response);
            return result;
        }

        /// <summary>
        /// Returns a list of Managed Accounts by Managed System ID.
        /// <para>API: GET ManagedSystems/{managedSystemID}/ManagedAccounts</para>
        /// </summary>
        /// <param name="managedSystemID">ID of the Managed System</param>
        /// <returns></returns>
        public ManagedAccountsResult GetAll(int managedSystemID)
        {
            HttpResponseMessage response = _conn.Get(string.Format("ManagedSystems/{0}/ManagedAccounts", managedSystemID));
            ManagedAccountsResult result = new ManagedAccountsResult(response);
            return result;
        }

        /// <summary>
        /// Creates a new Managed Account in the Managed System referenced by ID.
        /// <para>API: POST ManagedSystems/{managedSystemID}/ManagedAccounts</para>
        /// </summary>
        /// <param name="managedSystemID">ID of the Managed System</param>
        /// <param name="version">The model version</param>
        /// <returns></returns>
        public ManagedAccountResult Post(int managedSystemID, ManagedAccountPostPutModel model, string version = v30)
        {
            if (!Versions.Contains(version)) throw new ArgumentException($"Invalid version: {version}");

            HttpResponseMessage response = _conn.Post($"ManagedSystems/{managedSystemID}/ManagedAccounts/?version={version}", model);
            ManagedAccountResult result = new ManagedAccountResult(response);
            return result;
        }

        /// <summary>
        /// Updates an existing Managed Account by ID.
        /// <para>API: PUT ManagedAccounts/{id</para>
        /// </summary>
        /// <param name="id">ID of the Managed Account</param>
        /// <param name="version">The model version</param>
        /// <returns></returns>
        public ManagedAccountResult Put(int id, ManagedAccountPostPutModel model, string version = v30)
        {
            if (!Versions.Contains(version)) throw new ArgumentException($"Invalid version: {version}");

            HttpResponseMessage response = _conn.Put($"ManagedAccounts/{id}/?version={version}", model);
            ManagedAccountResult result = new ManagedAccountResult(response);
            return result;
        }

        /// <summary>
        /// Deletes a Managed Account by ID.
        /// <para>API: DELETE ManagedAccounts/{id}</para>
        /// </summary>
        /// <param name="id">ID of the Managed Account</param>
        /// <returns></returns>
        public DeleteResult Delete(int id)
        {
            HttpResponseMessage response = _conn.Delete($"ManagedAccounts/{id}");
            DeleteResult result = new DeleteResult(response);
            return result;
        }

        /// <summary>
        /// Deletes a Managed Account by Managed System ID and Managed Account name.
        /// <para>API: DELETE ManagedSystems/{managedSystemID}/ManagedAccounts/{accountName}</para>
        /// </summary>
        /// <param name="managedSystemID">ID of the Managed System</param>
        /// <param name="accountName">Name of the Managed Account</param>
        /// <returns></returns>
        public DeleteResult Delete(int managedSystemID, string accountName)
        {
            HttpResponseMessage response = _conn.Delete($"ManagedSystems/{managedSystemID}/ManagedAccounts/{accountName}");
            DeleteResult result = new DeleteResult(response);
            return result;
        }

        /// <summary>
        /// Deletes all Managed Accounts by Managed System ID.
        /// <para>API: DELETE ManagedSystems/{managedSystemID}/ManagedAccounts</para>
        /// </summary>
        /// <param name="managedSystemID">ID of the Managed System</param>
        /// <returns></returns>
        public DeleteResult DeleteAll(int managedSystemID)
        {
            HttpResponseMessage response = _conn.Delete($"ManagedSystems/{managedSystemID}/ManagedAccounts");
            DeleteResult result = new DeleteResult(response);
            return result;
        }

        #endregion

        #region Managed Accounts - Smart Rule

        /// <summary>
        /// Returns a list of Managed Accounts by Smart Rule ID.
        /// <para>API: GET SmartRules/{smartRuleID}/ManagedAccounts</para>
        /// </summary>
        /// <param name="smartRuleID">ID of the Smart Rule</param>
        /// <returns></returns>
        public ManagedAccountsResult GetAllBySmartRule(int smartRuleID)
        {
            HttpResponseMessage response = _conn.Get($"SmartRules/{smartRuleID}/ManagedAccounts");
            ManagedAccountsResult result = new ManagedAccountsResult(response);
            return result;
        }

        #endregion

        #region Managed Accounts - Quick Rule

        /// <summary>
        /// Returns a list of Managed Accounts by Quick Rule ID.
        /// <para>API: GET QuickRules/{quickRuleID}/ManagedAccounts</para>
        /// </summary>
        /// <param name="quickRuleID">ID of the Quick Rule</param>
        /// <returns></returns>
        public ManagedAccountsResult GetAllByQuickRule(int quickRuleID)
        {
            HttpResponseMessage response = _conn.Get($"QuickRules/{quickRuleID}/ManagedAccounts");
            ManagedAccountsResult result = new ManagedAccountsResult(response);
            return result;
        }

        /// <summary>
        /// Updates the entire list of Managed Accounts in a Quick Rule by removing all 'Managed Account Fields - Quick Group ID' filters 
        /// and adding a new one with the Managed Accounts referenced by ID.
        /// <para>
        /// Note: If the Quick Rule contains complex filters and/or actions created via the UI, the rule must reprocess before 
        /// returning. It is more performant to use a Quick Rule that contains a single filter of type 'Managed Account Fields - Quick Group ID' 
        /// and a single action of type 'Show as Smart Group', as is created using POST QuickRules.
        /// </para>
        /// <para>API: PUT QuickRules/{quickRuleID}/ManagedAccounts</para>
        /// </summary>
        /// <param name="quickRuleID">ID of the Quick Rule</param>
        /// <returns></returns>
        public ManagedAccountsResult PutInQuickRule(int quickRuleID, List<int> accountIDs)
        {
            object model = new { AccountIDs = accountIDs };

            HttpResponseMessage response = _conn.Put($"QuickRules/{quickRuleID}/ManagedAccounts", model);
            ManagedAccountsResult result = new ManagedAccountsResult(response);
            return result;
        }

        /// <summary>
        /// Adds the Managed Account referenced by ID to the Quick Rule by adding it to the first 'Managed Account Fields - Quick Group ID' filter found.
        /// <para>
        /// Note: If the Quick Rule contains complex filters and/or actions created via the UI, the rule must reprocess before 
        /// returning. It is more performant to use a Quick Rule that contains a single filter of type 'Managed Account Fields - Quick Group ID' 
        /// and a single action of type 'Show as Smart Group', as is created using POST QuickRules.
        /// </para>
        /// <para>API: POST QuickRules/{quickRuleID}/ManagedAccounts/{accountID}</para>
        /// </summary>
        /// <param name="quickRuleID">ID of the Quick Rule</param>
        /// <param name="accountID">ID of the Managed Account</param>
        /// <returns></returns>
        public ManagedAccountsResult PostInQuickRule(int quickRuleID, int accountID)
        {
            HttpResponseMessage response = _conn.Post($"QuickRules/{quickRuleID}/ManagedAccounts/{accountID}");
            ManagedAccountsResult result = new ManagedAccountsResult(response);
            return result;
        }

        /// <summary>
        /// Removes the Managed Account referenced by ID from the Quick Rule by removing it from all 'Managed Account Fields - Quick Group ID' filters found.
        /// <para>
        /// Important: A rule cannot be left in an invalid state. If removing the account will result in an empty filter, the filter itself will be removed. 
        /// If there are no filters left in the rule, a 400 Bad Request is returned. If you intend to replace all accounts in the rule, 
        /// see PUT QuickRules/{quickRuleID}/ManagedAccounts. If you intend to delete the rule, see DELETE QuickRules/{id}.
        /// </para>
        /// <para>
        /// Note: If the Quick Rule contains complex filters and/or actions created via the UI, the rule must reprocess before 
        /// returning. It is more performant to use a Quick Rule that contains a single filter of type 'Managed Account Fields - Quick Group ID' 
        /// and a single action of type 'Show as Smart Group', as is created using POST QuickRules.
        /// </para>
        /// <para>API: POST QuickRules/{quickRuleID}/ManagedAccounts/{accountID}</para>
        /// </summary>
        /// <param name="quickRuleID">ID of the Quick Rule</param>
        /// <param name="accountID">ID of the Managed Account</param>
        /// <returns></returns>
        public ManagedAccountsResult DeleteFromQuickRule(int quickRuleID, int accountID)
        {
            HttpResponseMessage response = _conn.Delete($"QuickRules/{quickRuleID}/ManagedAccounts/{accountID}");
            ManagedAccountsResult result = new ManagedAccountsResult(response);
            return result;
        }

        #endregion

    }
}
