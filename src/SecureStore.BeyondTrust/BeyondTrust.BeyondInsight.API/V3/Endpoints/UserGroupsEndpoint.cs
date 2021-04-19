using System.Net.Http;

namespace BeyondTrust.BeyondInsight.PasswordSafe.API.Client.V3
{
    public sealed class UserGroupsEndpoint : BaseEndpoint
    {
        internal UserGroupsEndpoint(PasswordSafeAPIConnector client)
            : base(client)
        {
        }

        /// <summary>
        /// Returns a list of User Groups.
        /// <para>API: GET <base>/UserGroups</para>
        /// </summary>
        /// <returns></returns>
        public UserGroupsResult GetAll()
        {
            HttpResponseMessage response = _conn.Get("UserGroups");
            UserGroupsResult result = new UserGroupsResult(response);
            return result;
        }

        /// <summary>
        /// Returns a User Group by ID.
        /// <para>API: GET UserGroups/{id}</para>
        /// </summary>
        /// <param name="id">ID of the User Group</param>
        /// <returns></returns>
        public UserGroupResult Get(int id)
        {
            HttpResponseMessage response = _conn.Get($"UserGroups/{id}");
            UserGroupResult result = new UserGroupResult(response);
            return result;
        }

        /// <summary>
        /// Returns a User Group by name.
        /// <para>API: GET UserGroups?name={name}</para>
        /// </summary>
        /// <param name="name">Name of the User Group</param>
        /// <returns></returns>
        public UserGroupResult Get(string name)
        {
            HttpResponseMessage response = _conn.Get($"UserGroups?name={name}");
            UserGroupResult result = new UserGroupResult(response);
            return result;
        }

        /// <summary>
        /// Creates a new BeyondInsight User Group, optionally with Permissions, Smart Rule access, and Application Registration IDs.
        /// <para>API: POST UserGroups</para>
        /// </summary>
        /// <param name="model">The User Group model</param>
        /// <returns></returns>
        public UserGroupResult Post(UserGroupBIModel model)
        {
            HttpResponseMessage response = _conn.Post("UserGroups", model);
            UserGroupResult result = new UserGroupResult(response);
            return result;
        }

        /// <summary>
        /// Creates a new ActiveDirectory User Group, optionally with Permissions, Smart Rule access, and Application Registration IDs.
        /// <para>API: POST UserGroups</para>
        /// </summary>
        /// <param name="model">The User Group model</param>
        /// <returns></returns>
        public UserGroupResult Post(UserGroupADModel model)
        {
            HttpResponseMessage response = _conn.Post("UserGroups", model);
            UserGroupResult result = new UserGroupResult(response);
            return result;
        }

        /// <summary>
        /// Creates a new LDAP User Group, optionally with Permissions, Smart Rule access, and Application Registration IDs.
        /// <para>API: POST UserGroups</para>
        /// </summary>
        /// <param name="model">The User Group model</param>
        /// <returns></returns>
        public UserGroupResult Post(UserGroupLDAPModel model)
        {
            HttpResponseMessage response = _conn.Post("UserGroups", model);
            UserGroupResult result = new UserGroupResult(response);
            return result;
        }

        /// <summary>
        /// Deletes a User Group by ID.
        /// <para>API: DELETE UserGroups/{id}</para>
        /// </summary>
        /// <param name="id">ID of the User Group</param>
        /// <returns></returns>
        public DeleteResult Delete(int id)
        {
            HttpResponseMessage response = _conn.Delete($"UserGroups/{id}");
            DeleteResult result = new DeleteResult(response);
            return result;
        }

        /// <summary>
        /// Deletes a User Group by name.
        /// <para>API: DELETE UserGroups?name={name}</para>
        /// </summary>
        /// <param name="name">Name of the User Group</param>
        /// <returns></returns>
        public DeleteResult Delete(string name)
        {
            HttpResponseMessage response = _conn.Delete($"UserGroups?name={name}");
            DeleteResult result = new DeleteResult(response);
            return result;
        }


        #region User Group Memberships

        /// <summary>
        /// Returns the User Group Memberships for an existing User.
        /// <para>API: GET Users/{userID}/UserGroups</para>
        /// </summary>
        /// <param name="userID">ID of the User</param>
        /// <returns></returns>
        public UserGroupsResult GetMemberships(int userID)
        {
            HttpResponseMessage response = _conn.Get($"Users/{userID}/UserGroups");
            UserGroupsResult result = new UserGroupsResult(response);
            return result;
        }

        /// <summary>
        /// Adds an existing User to a User Group.
        /// <para>API: POST Users/{userID}/UserGroups/{userGroupID}</para>
        /// </summary>
        /// <param name="userID">ID of the User</param>
        /// <param name="userGroupID">ID of the User Group</param>
        /// <returns></returns>
        public UserGroupResult PostMembership(int userID, int userGroupID)
        {
            HttpResponseMessage response = _conn.Post($"Users/{userID}/UserGroups/{userGroupID}");
            UserGroupResult result = new UserGroupResult(response);
            return result;
        }

        /// <summary>
        /// Removes a User from a User Group.
        /// <para>API: DELETE Users/{userID}/UserGroups/{userGroupID}}</para>
        /// </summary>
        /// <param name="userID">ID of the User</param>
        /// <param name="userGroupID">ID of the User Group</param>
        /// <returns></returns>
        public DeleteResult DeleteMembership(int userID, int userGroupID)
        {
            HttpResponseMessage response = _conn.Delete($"Users/{userID}/UserGroups/{userGroupID}");
            DeleteResult result = new DeleteResult(response);
            return result;
        }

        #endregion

    }
}
