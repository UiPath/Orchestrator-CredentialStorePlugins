using System;
using System.Net.Http;

namespace BeyondTrust.BeyondInsight.PasswordSafe.API.Client.V3
{
    public sealed class ImportsEndpoint : BaseEndpoint
    {
        internal ImportsEndpoint(PasswordSafeAPIConnector client)
            : base(client)
        {
        }

        /// <summary>
        /// Queues a Pasword Safe Import using the FileContents in the request body.
        /// <para>API: POST Imports</para>
        /// </summary>
        /// <param name="workgroupID">The ID of the Workgroup to which the import will be placed.</param>
        /// <param name="fileName">The filename of the import file.</param>
        /// <param name="fileContents">The contents of the import file.</param>
        /// <returns></returns>
        [Obsolete("Use Post overload with base64FileContents instead")]
        public ImportsResult Post(int workgroupID, string fileName, byte[] fileContents)
        {
            ImportModel model = new ImportModel()
            {
                WorkgroupID = workgroupID,
                FileName = fileName,
                FileContents = fileContents
            };

            return Post(model);
        }

        /// <summary>
        /// Queues an Import using the Base64FileContents in the request body.
        /// <para>API: POST Imports</para>
        /// </summary>
        /// <param name="workgroupID">The ID of the Workgroup to which the import will be placed.</param>
        /// <param name="importType">Type of Import being queued.</param>
        /// <param name="filter">Asset selection filter.</param>
        /// <param name="fileName">The filename of the import file.</param>
        /// <param name="base64FileContents">Base64 string containing the content of the import file.</param>
        /// <returns></returns>
        public ImportsResult Post(int workgroupID, string importType, string filter, string fileName, string base64FileContents)
        {
            ImportModel model = new ImportModel()
            {
                WorkgroupID = workgroupID,
                FileName = fileName,
                Base64FileContents = base64FileContents,
                ImportType = importType,
                Filter = filter
            };

            return Post(model);
        }


        /// <summary>
        /// Queues an Import.
        /// <para>API: POST Imports</para>
        /// </summary>
        /// <param name="model">The Import model.</param>
        /// <returns></returns>
        public ImportsResult Post(ImportModel model)
        {
            HttpResponseMessage response = _conn.Post("Imports", model);
            ImportsResult result = new ImportsResult(response);
            return result;
        }

    }
}
