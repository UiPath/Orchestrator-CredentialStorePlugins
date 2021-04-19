using System.Net.Http;

namespace BeyondTrust.BeyondInsight.PasswordSafe.API.Client.V3
{
    public sealed class AddressGroupsEndpoint : BaseEndpoint
    {
        internal AddressGroupsEndpoint(PasswordSafeAPIConnector client)
            : base(client)
        {
        }

        /// <summary>
        /// Returns a list of Address Groups.
        /// <para>API: GET AddressGroups</para>
        /// </summary>
        /// <returns></returns>
        public AddressGroupsResult GetAll()
        {
            HttpResponseMessage response = _conn.Get($"AddressGroups");
            AddressGroupsResult result = new AddressGroupsResult(response);
            return result;
        }

        /// <summary>
        /// Returns an Address Group by name.
        /// <para>API: GET AddressGroups?name={name}</para>
        /// </summary>
        /// <returns></returns>
        public AddressGroupResult Get(string name)
        {
            string queryParams = QueryParameterBuilder.Build(
                new QueryParameter("name", name)
                );

            HttpResponseMessage response = _conn.Get($"AddressGroups{queryParams}");
            AddressGroupResult result = new AddressGroupResult(response);
            return result;
        }

        /// <summary>
        /// Returns an Address Group by ID.
        /// <para>API: GET AddressGroups/{id}</para>
        /// </summary>
        /// <param name="id">ID of the Address Group</param>
        /// <returns></returns>
        public AddressGroupResult Get(int id)
        {
            HttpResponseMessage response = _conn.Get($"AddressGroups/{id}");
            AddressGroupResult result = new AddressGroupResult(response);
            return result;
        }

        /// <summary>
        /// Creates an Address Group.
        /// <para>API: POST AddressGroups</para>
        /// </summary>
        /// <param name="model">The Address Group model</param>
        /// <returns></returns>
        public AddressGroupResult Post(AddressGroupModel model)
        {
            HttpResponseMessage response = _conn.Post("AddressGroups", model);
            AddressGroupResult result = new AddressGroupResult(response);
            return result;
        }

        /// <summary>
        /// Updates an existing Address Group by ID.
        /// <para>API: PUT AddressGroups/{id</para>
        /// </summary>
        /// <param name="id">ID of the Address Group</param>
        /// <returns></returns>
        public AddressGroupResult Put(int id, AddressGroupModel model)
        {
            HttpResponseMessage response = _conn.Put($"AddressGroups/{id}", model);
            AddressGroupResult result = new AddressGroupResult(response);
            return result;
        }

        /// <summary>
        /// Deletes an Address Group by ID.
        /// <para>API: DELETE AddressGroups/{id}</para>
        /// </summary>
        /// <param name="id">ID of the Address Group</param>
        /// <returns></returns>
        public DeleteResult Delete(int id)
        {
            HttpResponseMessage response = _conn.Delete($"AddressGroups/{id}");
            DeleteResult result = new DeleteResult(response);
            return result;
        }

    }
}
