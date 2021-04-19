using System.Collections.Generic;
using System.Net.Http;

namespace BeyondTrust.BeyondInsight.PasswordSafe.API.Client.V3
{
    public sealed class QuickRulesEndpoint : BaseEndpoint
    {
        internal QuickRulesEndpoint(PasswordSafeAPIConnector client)
            : base(client)
        {
        }

        /// <summary>
        /// Creates a new Quick Rule with the Managed Accounts referenced by ID, containing a single filter of type 'Managed Account Fields - Quick Group ID' and a single action of type 'Show as Smart Group'.
        /// <para>API: POST QuickRules</para>
        /// </summary>
        /// <returns></returns>
        public QuickRuleResult Post(List<int> accountIDs, string title, string category = null, string description = null)
        {
            QuickRulePostModel model = new QuickRulePostModel()
            {
                Title = title,
                Category = category,
                Description = description,
            };
            model.AccountIDs.AddRange(accountIDs);

            HttpResponseMessage response = _conn.Post("QuickRules", model);
            QuickRuleResult result = new QuickRuleResult(response);
            return result;
        }

        /// <summary>
        /// Returns a list of Quick Rules to which the current user has at least Read access.
        /// <para>API: GET QuickRules</para>
        /// </summary>
        /// <returns></returns>
        public QuickRulesResult GetAll()
        {
            HttpResponseMessage response = _conn.Get("QuickRules");
            QuickRulesResult result = new QuickRulesResult(response);
            return result;
        }

        /// <summary>
        /// Returns a Quick Rule by ID.
        /// <para>API: GET QuickRules/{id}</para>
        /// </summary>
        /// <param name="id">ID of the Quick Rule</param>
        /// <returns></returns>
        public QuickRuleResult Get(int id)
        {
            HttpResponseMessage response = _conn.Get($"QuickRules/{id}");
            QuickRuleResult result = new QuickRuleResult(response);
            return result;
        }

        /// <summary>
        /// Returns a Quick Rule by title.
        /// <para>In a multi-tenant environment, assumes Global Organization.</para>
        /// <para>API: GET QuickRules?title={title}</para>
        /// </summary>
        /// <param name="title">Title of the Quick Rule</param>
        /// <returns></returns>
        public QuickRuleResult Get(string title)
        {
            HttpResponseMessage response = _conn.Get($"QuickRules?title={title}");
            QuickRuleResult result = new QuickRuleResult(response);
            return result;
        }

        /// <summary>
        /// Returns a Quick Rule by Organization ID and title.
        /// <para>Only valid in a multi-tenant environment.</para>
        /// <para>API: GET Organizations/{orgID}/QuickRules?title={title}</para>
        /// </summary>
        /// <param name="orgID">ID of the Organization</param>
        /// <param name="title">Title of the Quick Rule</param>
        /// <returns></returns>
        public QuickRuleResult Get(string orgID, string title)
        {
            HttpResponseMessage response = _conn.Get($"Organizations/{orgID}/QuickRules?title={title}");
            QuickRuleResult result = new QuickRuleResult(response);
            return result;
        }

        /// <summary>
        /// Deletes a Quick Rule by ID.
        /// <para>API: DELETE QuickRules/{id}</para>
        /// </summary>
        /// <param name="id">ID of the Quick Rule</param>
        /// <returns></returns>
        public DeleteResult Delete(int id)
        {
            HttpResponseMessage response = _conn.Delete($"QuickRules/{id}");
            DeleteResult result = new DeleteResult(response);
            return result;
        }

        /// <summary>
        /// Deletes a Quick Rule by title.
        /// <para>In a multi-tenant environment, assumes Global Organization.</para>
        /// <para>API: DELETE QuickRules?title={title}</para>
        /// </summary>
        /// <param name="title">Title of the Quick Rule</param>
        /// <returns></returns>
        public DeleteResult Delete(string title)
        {
            HttpResponseMessage response = _conn.Delete($"QuickRules?title={title}");
            DeleteResult result = new DeleteResult(response);
            return result;
        }

        /// <summary>
        /// Deletes a Quick Rule by Organization ID and title.
        /// <para>Only valid in a multi-tenant environment.</para>
        /// <para>API: DELETE Organizations/{orgID}/QuickRules?title={title}</para>
        /// </summary>
        /// <param name="orgID">ID of the Organization</param>
        /// <param name="title">Title of the Quick Rule</param>
        /// <returns></returns>
        public DeleteResult Delete(string orgID, string title)
        {
            HttpResponseMessage response = _conn.Delete($"Organizations/{orgID}/QuickRules?title={title}");
            DeleteResult result = new DeleteResult(response);
            return result;
        }

    }
}
