using System;
using System.Net.Http;

namespace BeyondTrust.BeyondInsight.PasswordSafe.API.Client.V3
{
    public sealed class AddressesEndpoint : BaseEndpoint
    {
        internal AddressesEndpoint(PasswordSafeAPIConnector client)
            : base(client)
        {
        }

        /// <summary>
        /// Returns an Address by ID.
        /// <para>API: GET Addresses/{id}</para>
        /// </summary>
        /// <param name="id">ID of the Address</param>
        /// <returns></returns>
        public AddressResult Get(int id)
        {
            HttpResponseMessage response = _conn.Get($"Addresses/{id}");
            AddressResult result = new AddressResult(response);
            return result;
        }

        /// <summary>
        /// Returns a list of Addresses by Address Group ID.
        /// <para>API: GET AddressGroups/{addressGroupID}/Addresses</para>
        /// </summary>
        /// <param name="addressGroupID">ID of the Address Group</param>
        /// <returns></returns>
        public AddressesResult GetAll(int addressGroupID)
        {
            HttpResponseMessage response = _conn.Get($"AddressGroups/{addressGroupID}/Addresses");
            AddressesResult result = new AddressesResult(response);
            return result;
        }

        /// <summary>
        /// Creates a new Address in the Address Group referenced by ID.
        /// <para>API: POST AddressGroups/{addressGroupID}/Addresses</para>
        /// </summary>
        /// <param name="addressGroupID">ID of the Address Group</param>
        /// <param name="version">The model version</param>
        /// <returns></returns>
        public AddressResult Post(int addressGroupID, AddressModel model)
        {
            HttpResponseMessage response = _conn.Post($"AddressGroups/{addressGroupID}/Addresses", model);
            AddressResult result = new AddressResult(response);
            return result;
        }

        /// <summary>
        /// Updates an existing Address by ID.
        /// <para>API: PUT Addresses/{id</para>
        /// </summary>
        /// <param name="id">ID of the Address</param>
        /// <param name="version">The model version</param>
        /// <returns></returns>
        public AddressResult Put(int id, AddressModel model)
        {
            HttpResponseMessage response = _conn.Put($"Addresses/{id}", model);
            AddressResult result = new AddressResult(response);
            return result;
        }

        /// <summary>
        /// Deletes an Address by ID.
        /// <para>API: DELETE Addresses/{id}</para>
        /// </summary>
        /// <param name="id">ID of the Address</param>
        /// <returns></returns>
        public DeleteResult Delete(int id)
        {
            HttpResponseMessage response = _conn.Delete($"Addresses/{id}");
            DeleteResult result = new DeleteResult(response);
            return result;
        }

        /// <summary>
        /// Deletes all Addresses by Address Group ID.
        /// <para>API: DELETE AddressGroups/{addressGroupID}/Addresses</para>
        /// </summary>
        /// <param name="addressGroupID">ID of the Address Group</param>
        /// <returns></returns>
        public DeleteResult DeleteAll(int addressGroupID)
        {
            HttpResponseMessage response = _conn.Delete($"AddressGroups/{addressGroupID}/Addresses");
            DeleteResult result = new DeleteResult(response);
            return result;
        }

    }
}
