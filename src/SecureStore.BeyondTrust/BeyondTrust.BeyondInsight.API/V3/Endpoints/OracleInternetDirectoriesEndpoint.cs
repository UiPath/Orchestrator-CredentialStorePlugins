using System;
using System.Net.Http;

namespace BeyondTrust.BeyondInsight.PasswordSafe.API.Client.V3
{
    public sealed class OracleInternetDirectoriesEndpoint : BaseEndpoint
    {
        internal OracleInternetDirectoriesEndpoint(PasswordSafeAPIConnector client)
            : base(client)
        {
        }

        /// <summary>
        /// Returns a list of Oracle Internet Directories.
        /// <para>API: GET OracleInternetDirectories</para>
        /// </summary>
        public OracleInternetDirectoriesResult GetAll()
        {
            HttpResponseMessage response = _conn.Get("OracleInternetDirectories");
            OracleInternetDirectoriesResult result = new OracleInternetDirectoriesResult(response);
            return result;
        }

        /// <summary>
        /// Returns a list of Oracle Internet Directories by Organization ID.
        /// <para>API: GET Organizations/{orgID}/OracleInternetDirectories</para>
        /// </summary>
        public OracleInternetDirectoriesResult GetAll(Guid orgID)
        {
            HttpResponseMessage response = _conn.Get($"Organizations/{orgID}/OracleInternetDirectories");
            OracleInternetDirectoriesResult result = new OracleInternetDirectoriesResult(response);
            return result;
        }

        /// <summary>
        /// Returns an Oracle Internet Directory by ID.
        /// <para>API: GET OracleInternetDirectories/{id}</para>
        /// </summary>
        public OracleInternetDirectoryResult Get(Guid id)
        {
            HttpResponseMessage response = _conn.Get($"OracleInternetDirectories/{id}");
            OracleInternetDirectoryResult result = new OracleInternetDirectoryResult(response);
            return result;
        }

        /// <summary>
        /// Tests the connection to an Oracle Internet Directory by ID.
        /// <para>API: POST OracleInternetDirectories/{id}/Test</para>
        /// </summary>
        /// <param name="id"></param>
        public OracleInternetDirectoryTestResult Test(Guid id)
        {
            HttpResponseMessage response = _conn.Post($"OracleInternetDirectories/{id}/Test");
            OracleInternetDirectoryTestResult result = new OracleInternetDirectoryTestResult(response);
            return result;
        }

        /// <summary>
        /// Queries and returns DB Services for an Oracle Internet Directory by ID.
        /// <para>API: POST OracleInternetDirectories/{id}/Services/Query</para>
        /// </summary>
        /// <param name="id"></param>
        public OracleInternetDirectoryQueryServicesResult QueryServices(Guid id)
        {
            HttpResponseMessage response = _conn.Post($"OracleInternetDirectories/{id}/Services/Query");
            OracleInternetDirectoryQueryServicesResult result = new OracleInternetDirectoryQueryServicesResult(response);
            return result;
        }

    }
}
