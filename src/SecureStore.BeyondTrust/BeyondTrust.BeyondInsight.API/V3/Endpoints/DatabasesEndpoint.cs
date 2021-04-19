using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace BeyondTrust.BeyondInsight.PasswordSafe.API.Client.V3
{
    public class DatabasesEndpoint : BaseEndpoint
    {
        internal DatabasesEndpoint(PasswordSafeAPIConnector client)
            : base(client)
        {
        }

        /// <summary>
        /// Returns a list of Databases.
        /// <para>API: GET Databases</para>
        /// </summary>
        /// <returns></returns>
        public DatabasesResult GetAll()
        {
            HttpResponseMessage response = _conn.Get("Databases");
            DatabasesResult result = new DatabasesResult(response);
            return result;
        }

        /// <summary>
        /// Returns a Database by ID.
        /// <para>API: GET Databases/{id}</para>
        /// </summary>
        /// <param name="id">ID of the Database</param>
        /// <returns></returns>
        public DatabaseResult Get(int id)
        {
            HttpResponseMessage response = _conn.Get($"Databases/{id}");
            DatabaseResult result = new DatabaseResult(response);
            return result;
        }

        /// <summary>
        /// Returns a list of Databases for the given Asset.
        /// <para>API: GET Assets/{id}/Databases</para>
        /// </summary>
        /// <param name="id">ID of the Asset</param>
        /// <returns></returns>
        public DatabasesResult GetAllByAsset(int id)
        {
            HttpResponseMessage response = _conn.Get($"Assets/{id}/Databases");
            DatabasesResult result = new DatabasesResult(response);
            return result;
        }

        /// <summary>
        /// Creates a new Database in the Asset referenced by ID.
        /// <para>API: POST Assets/{id}/Databases</para>
        /// </summary>
        /// <param name="id">ID of the Asset</param>
        /// <returns></returns>
        public DatabaseResult Post(int id, DatabasePostModel model)
        {
            HttpResponseMessage response = _conn.Post($"Assets/{id}/Databases", model);
            DatabaseResult result = new DatabaseResult(response);
            return result;
        }

        /// <summary>
        /// Updates an existing Database by ID.
        /// <para>API: PUT Databases/{id</para>
        /// </summary>
        /// <param name="id">ID of the Database</param>
        /// <returns></returns>
        public DatabaseResult Put(int id, DatabaseModel model)
        {
            HttpResponseMessage response = _conn.Put($"Databases/{id}", model);
            DatabaseResult result = new DatabaseResult(response);
            return result;
        }

        /// <summary>
        /// Deletes a Database by ID.
        /// <para>API: DELETE Databases/{id}</para>
        /// </summary>
        /// <param name="id">ID of the Database</param>
        /// <returns></returns>
        public DeleteResult Delete(int id)
        {
            HttpResponseMessage response = _conn.Delete($"Databases/{id}");
            DeleteResult result = new DeleteResult(response);
            return result;
        }

    }
}
