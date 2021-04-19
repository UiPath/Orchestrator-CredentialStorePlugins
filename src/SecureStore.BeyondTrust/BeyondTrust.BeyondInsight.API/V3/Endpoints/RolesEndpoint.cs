using System.Net.Http;

namespace BeyondTrust.BeyondInsight.PasswordSafe.API.Client.V3
{
    public sealed class RolesEndpoint : BaseEndpoint
    {
        internal RolesEndpoint(PasswordSafeAPIConnector client)
            : base(client)
        {
        }

        /// <summary>
        /// Returns a list of Password Safe Roles.
        /// <para>API: GET Roles</para>
        /// </summary>
        /// <returns></returns>
        public RolesResult GetAll()
        {
            HttpResponseMessage response = _conn.Get("Roles");
            RolesResult result = new RolesResult(response);
            return result;
        }

        /// <summary>
        /// Returns a list of Roles for the User Group and Smart Rule referenced by ID.
        /// <para>API: GET UserGroups/{userGroupId}/SmartRules/{smartRuleId}/Roles</para>
        /// </summary>
        /// <param name="userGroupId">ID of the User Group</param>
        /// <param name="smartRuleId">ID of the Smart Rule</param>
        /// <returns></returns>
        public RolesResult Get(int userGroupId, int smartRuleId)
        {
            HttpResponseMessage response = _conn.Get(string.Format("UserGroups/{0}/SmartRules/{1}/Roles", userGroupId, smartRuleId));
            RolesResult result = new RolesResult(response);
            return result;
        }

        /// <summary>
        /// Sets Password Safe Roles for the User Group and Smart Rule referenced by ID.
        /// <para>API: POST UserGroups/{userGroupId}/SmartRules/{smartRuleId}/Roles</para>
        /// </summary>
        /// <param name="userGroupId">ID of the User Group</param>
        /// <param name="smartRuleId">ID of the Smart Rule</param>
        /// <param name="model">The model of Roles and Access Policy</param>
        /// <returns></returns>
        public RolesPostResult Post(int userGroupId, int smartRuleId, RolePostModel model)
        {
            HttpResponseMessage response = _conn.Post(string.Format("UserGroups/{0}/SmartRules/{1}/Roles", userGroupId, smartRuleId), model);
            RolesPostResult result = new RolesPostResult(response);
            return result;
        }

        /// <summary>
        /// Deletes all Password Safe Roles for the User Group and Smart Rule referenced by ID.
        /// <para>API: DELETE UserGroups/{userGroupId}/SmartRules/{smartRuleId}/Roles</para>
        /// </summary>
        /// <param name="userGroupId">ID of the User Group</param>
        /// <param name="smartRuleId">ID of the Smart Rule</param>
        /// <returns></returns>
        public DeleteResult Delete(int userGroupId, int smartRuleId)
        {
            HttpResponseMessage response = _conn.Delete(string.Format("UserGroups/{0}/SmartRules/{1}/Roles", userGroupId, smartRuleId));
            DeleteResult result = new DeleteResult(response);
            return result;
        }
    }
}
