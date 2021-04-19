using System.Collections.Generic;
using System.Net.Http;

namespace BeyondTrust.BeyondInsight.PasswordSafe.API.Client.V3
{
    public sealed class PermissionsEndpoint : BaseEndpoint
    {
        internal PermissionsEndpoint(PasswordSafeAPIConnector client)
            : base(client)
        {
        }

        /// <summary>
        /// Returns a list of Permissions.
        /// <para>API: GET Permissions</para>
        /// </summary>
        /// <returns></returns>
        public PermissionsResult GetAll()
        {
            HttpResponseMessage response = _conn.Get("Permissions");
            PermissionsResult result = new PermissionsResult(response);
            return result;
        }

        /// <summary>
        /// Gets all Permissions for the User Group referenced by ID.
        /// <para>API: GET UserGroups/{userGroupId}/Permissions</para>
        /// </summary>
        /// <returns></returns>
        public PermissionsAccessLevelResult Get(int userGroupId)
        {
            HttpResponseMessage response = _conn.Get($"UserGroups/{userGroupId}/Permissions");
            PermissionsAccessLevelResult result = new PermissionsAccessLevelResult(response);
            return result;
        }

        /// <summary>
        /// Sets Permissions for the User Group referenced by ID.
        /// <para>API: POST UserGroups/{userGroupId}/Permissions</para>
        /// </summary>
        /// <param name="userGroupId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public PermissionsPostResult Post(int userGroupId, List<PermissionAccessLevelModel> model)
        {
            HttpResponseMessage response = _conn.Post($"UserGroups/{userGroupId}/Permissions", model);
            PermissionsPostResult result = new PermissionsPostResult(response);
            return result;
        }

        /// <summary>
        /// Deletes all Permissions for the User Group referenced by ID.
        /// <para>API: DELETE UserGroups/{userGroupId}/Permissions</para>
        /// </summary>
        /// <param name="userGroupId">ID of the User Group</param>
        /// <returns></returns>
        public DeleteResult Delete(int userGroupId)
        {
            HttpResponseMessage response = _conn.Delete($"UserGroups/{userGroupId}/Permissions");
            DeleteResult result = new DeleteResult(response);
            return result;
        }

    }
}
