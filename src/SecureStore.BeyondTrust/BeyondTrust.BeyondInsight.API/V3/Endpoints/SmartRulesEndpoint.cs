using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace BeyondTrust.BeyondInsight.PasswordSafe.API.Client.V3
{
    public sealed class SmartRulesEndpoint : BaseEndpoint
    {
        internal SmartRulesEndpoint(PasswordSafeAPIConnector client)
            : base(client)
        {
        }

        /// <summary>
        /// List of Smart Rule query 'type' parameter values.
        /// </summary>
        public readonly IReadOnlyList<string> Types = new List<string>() { "Asset", "ManagedSystem", "ManagedAccount", "Vulnerabilities" };

        /// <summary>
        /// Returns a list of Smart Rules to which the current user has at least Read access.
        /// <para>API: GET SmartRules</para>
        /// </summary>
        /// <param name="type">The type of Smart Rules to return (all, ManagedAccount, Asset, Vulnerabilities)</param>
        /// <returns></returns>
        public SmartRulesResult GetAll(string type = "all")
        {
            HttpResponseMessage response = _conn.Get($"SmartRules?type={type}");
            SmartRulesResult result = new SmartRulesResult(response);
            return result;
        }

        /// <summary>
        /// Returns a list of Asset-based Smart Rules to which the current user has at least Read access.
        /// </summary>
        /// <returns></returns>
        public SmartRulesResult GetAssetSmartRules()
        {
            return GetAll("Asset");
        }

        /// <summary>
        /// Returns a list of Managed System-based Smart Rules to which the current user has at least Read access.
        /// </summary>
        /// <returns></returns>
        public SmartRulesResult GetManagedSystemSmartRules()
        {
            return GetAll("ManagedSystem");
        }

        /// <summary>
        /// Returns a list of Managed Account-based Smart Rules to which the current user has at least Read access.
        /// </summary>
        /// <returns></returns>
        public SmartRulesResult GetManagedAccountSmartRules()
        {
            return GetAll("ManagedAccount");
        }

        /// <summary>
        /// Returns a list of Vulnerability-based Smart Rules to which the current user has at least Read access.
        /// </summary>
        /// <returns></returns>
        public SmartRulesResult GetVulnerabilitySmartRules()
        {
            return GetAll("Vulnerabilities");
        }

        /// <summary>
        /// Returns a Smart Rule by ID.
        /// <para>API: GET SmartRules/{id}</para>
        /// </summary>
        /// <param name="id">ID of the Smart Rule</param>
        /// <returns></returns>
        public SmartRuleResult Get(int id)
        {
            HttpResponseMessage response = _conn.Get($"SmartRules/{id}");
            SmartRuleResult result = new SmartRuleResult(response);
            return result;
        }

        /// <summary>
        /// Returns a Smart Rule by title.
        /// <para>In a multi-tenant environment, assumes Global Organization.</para>
        /// <para>API: GET SmartRules?title={title}</para>
        /// </summary>
        /// <param name="title">Title of the Smart Rule</param>
        /// <returns></returns>
        public SmartRuleResult Get(string title)
        {
            HttpResponseMessage response = _conn.Get($"SmartRules?title={title}");
            SmartRuleResult result = new SmartRuleResult(response);
            return result;
        }

        /// <summary>
        /// Returns a Smart Rule by Organization ID and title.
        /// <para>Only valid in a multi-tenant environment.</para>
        /// <para>API: GET Organizations/{orgID}/SmartRules?title={title}</para>
        /// </summary>
        /// <param name="orgID">ID of the Organization</param>
        /// <param name="title">Title of the Smart Rule</param>
        /// <returns></returns>
        public SmartRuleResult Get(string orgID, string title)
        {
            HttpResponseMessage response = _conn.Get($"Organizations/{orgID}/SmartRules?title={title}");
            SmartRuleResult result = new SmartRuleResult(response);
            return result;
        }

        /// <summary>
        /// Deletes a Smart Rule by ID.
        /// <para>API: DELETE SmartRules/{id}</para>
        /// </summary>
        /// <param name="id">ID of the Smart Rule</param>
        /// <returns></returns>
        public DeleteResult Delete(int id)
        {
            HttpResponseMessage response = _conn.Delete($"SmartRules/{id}");
            DeleteResult result = new DeleteResult(response);
            return result;
        }

        /// <summary>
        /// Deletes a Smart Rule by title.
        /// <para>In a multi-tenant environment, assumes Global Organization.</para>
        /// <para>API: DELETE SmartRules?title={title}</para>
        /// </summary>
        /// <param name="title">Title of the Smart Rule</param>
        /// <returns></returns>
        public DeleteResult Delete(string title)
        {
            HttpResponseMessage response = _conn.Delete($"SmartRules?title={title}");
            DeleteResult result = new DeleteResult(response);
            return result;
        }

        /// <summary>
        /// Deletes a Smart Rule by Organization ID and title.
        /// <para>Only valid in a multi-tenant environment.</para>
        /// <para>API: DELETE Organizations/{orgID}/SmartRules?title={title}</para>
        /// </summary>
        /// <param name="orgID">ID of the Organization</param>
        /// <param name="title">Title of the Smart Rule</param>
        /// <returns></returns>
        public DeleteResult Delete(string orgID, string title)
        {
            HttpResponseMessage response = _conn.Delete($"Organizations/{orgID}/SmartRules?title={title}");
            DeleteResult result = new DeleteResult(response);
            return result;
        }

        /// <summary>
        /// Immediately process a Smart Rule by ID.
        /// <para>API: POST SmartRules/{id}/Process</para>
        /// </summary>
        /// <param name="id">ID of the Smart Rule</param>
        /// <returns></returns>
        public SmartRuleResult Process(int id)
        {
            HttpResponseMessage response = _conn.Post($"SmartRules/{id}/Process");
            SmartRuleResult result = new SmartRuleResult(response);
            return result;
        }

        /// <summary>
        /// Specialized action for creating an Asset-type Smart Rule for filtering Assets by Attributes.
        /// <para>API: POST SmartRules/FilterAssetAttribute</para>
        /// </summary>
        public SmartRuleResult FilterAssetAttribute(IEnumerable<int> attributeIDs, string title, string category, string description = null, bool processImmediately = true)
        {
            SmartRuleFilterAssetAttributeModel model = new SmartRuleFilterAssetAttributeModel
            {
                AttributeIDs = attributeIDs.ToList(),
                Title = title,
                Category = category,
                Description = description,
                ProcessImmediately = processImmediately,
            };

            HttpResponseMessage response = _conn.Post("SmartRules/FilterAssetAttribute", model);
            SmartRuleResult result = new SmartRuleResult(response);
            return result;
        }


        #region Deprecated

        /// <summary>
        /// Specialized action for creating a Managed Account-type Smart Rule for filtering a single Managed Account by System Name and Account Name.
        /// <para>API: POST SmartRules/FilterSingleAccount</para>
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        [Obsolete("Use QuickRules instead")]
        public SmartRuleResult FilterSingleAccount(int accountId, string title = "")
        {
            SmartRuleFilterSingleAccountModel model = new SmartRuleFilterSingleAccountModel 
            { 
                AccountID = accountId,
                Title = title 
            };

            HttpResponseMessage response = _conn.Post("SmartRules/FilterSingleAccount", model);
            SmartRuleResult result = new SmartRuleResult(response);
            return result;
        }

        #endregion

    }
}
