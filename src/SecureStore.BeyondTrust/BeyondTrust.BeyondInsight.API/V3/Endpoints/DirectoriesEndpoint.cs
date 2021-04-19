using System.Net.Http;

namespace BeyondTrust.BeyondInsight.PasswordSafe.API.Client.V3
{
    public class DirectoriesEndpoint : BaseEndpoint
    {
        internal DirectoriesEndpoint(PasswordSafeAPIConnector client)
            : base(client)
        {
        }

        /// <summary>
        /// Returns a list of Directories.
        /// <para>API: GET Directories</para>
        /// </summary>
        public DirectoriesResult GetAll()
        {
            HttpResponseMessage response = _conn.Get("Directories");
            DirectoriesResult result = new DirectoriesResult(response);
            return result;
        }

        /// <summary>
        /// Returns a Directory by ID.
        /// <para>API: GET Directories/{id}</para>
        /// </summary>
        /// <param name="id">ID of the Directory</param>
        /// <returns></returns>
        public DirectoryResult Get(int id)
        {
            HttpResponseMessage response = _conn.Get($"Directories/{id}");
            DirectoryResult result = new DirectoryResult(response);
            return result;
        }

        /// <summary>
        /// Creates a new Directory in the Workgroup referenced by ID.
        /// <para>API: POST Workgroups/{workgroupID}/Directories</para>
        /// </summary>
        /// <param name="workgroupID">ID of the Workgroup</param>
        /// <returns></returns>
        public DirectoryResult Post(int workgroupID, DirectoryPostModel model)
        {
            HttpResponseMessage response = _conn.Post($"Workgroups/{workgroupID}/Directories", model);
            DirectoryResult result = new DirectoryResult(response);
            return result;
        }

        /// <summary>
        /// Updates an existing Directory by ID.
        /// <para>API: PUT Directories/{id</para>
        /// </summary>
        /// <param name="id">ID of the Directory</param>
        /// <returns></returns>
        public DirectoryResult Put(int id, DirectoryModel model)
        {
            HttpResponseMessage response = _conn.Put($"Directories/{id}", model);
            DirectoryResult result = new DirectoryResult(response);
            return result;
        }

        /// <summary>
        /// Deletes a Directory by ID.
        /// <para>API: DELETE Directories/{id}</para>
        /// </summary>
        /// <param name="id">ID of the Directory</param>
        /// <returns></returns>
        public DeleteResult Delete(int id)
        {
            HttpResponseMessage response = _conn.Delete($"Directories/{id}");
            DeleteResult result = new DeleteResult(response);
            return result;
        }

    }
}
