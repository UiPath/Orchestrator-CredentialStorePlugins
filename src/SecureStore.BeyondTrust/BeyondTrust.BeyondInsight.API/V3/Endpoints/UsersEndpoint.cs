using System.Net.Http;

namespace BeyondTrust.BeyondInsight.PasswordSafe.API.Client.V3
{
    public sealed class UsersEndpoint : BaseEndpoint
    {
        internal UsersEndpoint(PasswordSafeAPIConnector client)
            : base(client)
        {
        }

        /// <summary>
        /// Returns a list of Users for the User Group referenced by ID.
        /// <para>API: GET UserGroups/{userGroupId}/Users</para>
        /// </summary>
        /// <param name="userGroupId">ID of the User Group</param>
        /// <returns></returns>
        public UsersResult GetAll(int userGroupId)
        {
            HttpResponseMessage response = _conn.Get($"UserGroups/{userGroupId}/Users");
            UsersResult result = new UsersResult(response);
            return result;
        }

        /// <summary>
        /// Returns a User by ID.
        /// <para>API: GET Users/{id}</para>
        /// </summary>
        /// <param name="userID">ID of the BeyondInsight User</param>
        /// <returns></returns>
        public UserResult Get(int userID)
        {
            HttpResponseMessage response = _conn.Get($"Users/{userID}");
            UserResult result = new UserResult(response);
            return result;
        }

        /// <summary>
        /// Creates a new BeyondInsight User with no User Group associations.
        /// <para>API: POST Users</para>
        /// </summary>
        /// <param name="model">Model of the new User to create.</param>
        /// <returns></returns>
        public UserResult Post(UserPostBIModel model)
        {
            HttpResponseMessage response = _conn.Post("Users", model);
            UserResult result = new UserResult(response);
            return result;
        }

        /// <summary>
        /// Creates a new Active Directory User with no User Group associations.
        /// <para>API: POST Users</para>
        /// </summary>
        /// <param name="model">Model of the new User to create.</param>
        /// <returns></returns>
        public UserResult Post(UserPostADModel model)
        {
            HttpResponseMessage response = _conn.Post("Users", model);
            UserResult result = new UserResult(response);
            return result;
        }

        /// <summary>
        /// Creates a new LDAP User with no User Group associations.
        /// <para>API: POST Users</para>
        /// </summary>
        /// <param name="model">Model of the new User to create.</param>
        /// <returns></returns>
        public UserResult Post(UserPostLDAPModel model)
        {
            HttpResponseMessage response = _conn.Post("Users", model);
            UserResult result = new UserResult(response);
            return result;
        }

        /// <summary>
        /// Creates a new User account in the User Group referenced by ID.
        /// <para>API: POST UserGroups/{userGroupId}/Users</para>
        /// </summary>
        /// <param name="userGroupId">ID of the User Group</param>
        /// <param name="model">Model of the new User to create.</param>
        /// <returns></returns>
        public UserResult Post(int userGroupId, UserPostPutModel model)
        {
            HttpResponseMessage response = _conn.Post($"UserGroups/{userGroupId}/Users", model);
            UserResult result = new UserResult(response);
            return result;
        }

        /// <summary>
        /// Updates a BeyondInsight User by ID. Note: Cannot update ActiveDirectory or LDAP users.
        /// <para>API: PUT Users/{id}</para>
        /// </summary>
        /// <param name="userID">ID of the BeyondInsight User</param>
        /// <param name="model">Model of the User to update.</param>
        /// <returns></returns>
        public UserResult Put(int userID, UserPostPutModel model)
        {
            HttpResponseMessage response = _conn.Put($"Users/{userID}", model);
            UserResult result = new UserResult(response);
            return result;
        }

        /// <summary>
        /// Deletes a BeyondInsight User by ID. Note: Cannot delete ActiveDirectory or LDAP users.
        /// <para>API: DELETE Users/{id}</para>
        /// </summary>
        /// <param name="userID">ID of the BeyondInsight User</param>
        /// <returns></returns>
        public DeleteResult Delete(int userID)
        {
            HttpResponseMessage response = _conn.Delete($"Users/{userID}");
            DeleteResult result = new DeleteResult(response);
            return result;
        }

        /// <summary>
        /// Quarantines the User referenced by ID.
        /// <para>API: POST Users/{id}/Quarantine</para>
        /// </summary>
        /// <param name="id">ID of the BeyondInsight User</param>
        /// <returns></returns>
        public UserResult Quarantine(int id)
        {
            HttpResponseMessage response = _conn.Post($"Users/{id}/Quarantine");
            UserResult result = new UserResult(response);
            return result;
        }

    }
}
