using System.Net.Http;

namespace BeyondTrust.BeyondInsight.PasswordSafe.API.Client.V3
{
    public sealed class WorkgroupsEndpoint : BaseEndpoint
    {
        internal WorkgroupsEndpoint(PasswordSafeAPIConnector client)
            : base(client)
        {
        }

        /// <summary>
        /// Returns a list of Workgroups to which the current user has permission.
        /// <para>API: GET Workgroups</para>
        /// </summary>
        /// <returns></returns>
        public WorkgroupsResult GetAll()
        {
            HttpResponseMessage response = _conn.Get("Workgroups");
            WorkgroupsResult result = new WorkgroupsResult(response);
            return result;
        }

        /// <summary>
        /// Returns a Workgroup by ID.
        /// <para>API: GET Workgroups/{id}</para>
        /// </summary>
        /// <param name="id">ID of the Workgroup</param>
        /// <returns></returns>
        public WorkgroupResult Get(int id)
        {
            HttpResponseMessage response = _conn.Get(string.Format("Workgroups/{0}", id));
            WorkgroupResult result = new WorkgroupResult(response);
            return result;
        }

        /// <summary>
        /// Returns a Workgroup by name.
        /// <para>API: GET Workgroups?name={name}</para>
        /// </summary>
        /// <param name="name">Name of the Workgroup</param>
        /// <returns></returns>
        public WorkgroupResult Get(string name)
        {
            HttpResponseMessage response = _conn.Get($"Workgroups?name={name}");
            WorkgroupResult result = new WorkgroupResult(response);
            return result;
        }

        /// <summary>
        /// Creates a Workgroup.
        /// <para>API: POST Workgroups</para>
        /// </summary>
        /// <param name="model">The Workgroup model</param>
        /// <returns></returns>
        public WorkgroupResult Post(WorkgroupModel model)
        {
            HttpResponseMessage response = _conn.Post("Workgroups", model);
            WorkgroupResult result = new WorkgroupResult(response);
            return result;
        }

    }
}
