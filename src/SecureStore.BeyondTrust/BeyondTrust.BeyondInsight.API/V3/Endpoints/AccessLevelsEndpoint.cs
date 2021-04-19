using System.Net.Http;

namespace BeyondTrust.BeyondInsight.PasswordSafe.API.Client.V3
{
    public sealed class AccessLevelsEndpoint : BaseEndpoint
    {
        internal AccessLevelsEndpoint(PasswordSafeAPIConnector client)
            : base(client)
        {
        }

        /// <summary>
        /// Returns a list of Access Levels for Permissions. (i.e. None, Read, Read/Write)
        /// <para>API: GET AccessLevels</para>
        /// </summary>
        /// <returns></returns>
        public AccessLevelsResult GetAll()
        {
            HttpResponseMessage response = _conn.Get("AccessLevels");
            AccessLevelsResult result = new AccessLevelsResult(response);
            return result;
        }

        /// <summary>
        /// Sets the Access Level for a User Group Smart Rule.
        /// <para>API: POST UserGroups/{userGroupId}/SmartRules/{smartRuleId}/AccessLevels</para>
        /// </summary>
        /// <param name="userGroupId">ID of the User Group</param>
        /// <param name="smartRuleId">ID of the Smart Rule</param>
        /// <param name="accessLevelID">The Access Level</param>
        /// <returns></returns>
        public AccessLevelResult Post(int userGroupId, int smartRuleId, int accessLevelID)
        {
            AccessLevelPostModel model = new AccessLevelPostModel() { AccessLevelID = accessLevelID };

            HttpResponseMessage response = _conn.Post(string.Format("UserGroups/{0}/SmartRules/{1}/AccessLevels", userGroupId, smartRuleId), model);
            AccessLevelResult result = new AccessLevelResult(response);
            return result;
        }

    }
}
