using System.Net.Http;

namespace BeyondTrust.BeyondInsight.PasswordSafe.API.Client.V3
{
    public sealed class AttributeTypesEndpoint : BaseEndpoint
    {
        internal AttributeTypesEndpoint(PasswordSafeAPIConnector client)
            : base(client)
        {
        }

        /// <summary>
        /// Returns a list of Attribute Types.
        /// <para>API: GET AttributeTypes</para>
        /// </summary>
        /// <returns></returns>
        public AttributeTypesResult GetAll()
        {
            HttpResponseMessage response = _conn.Get("AttributeTypes");
            AttributeTypesResult result = new AttributeTypesResult(response);
            return result;
        }

        /// <summary>
        /// Returns an Attribute Type by ID.
        /// <para>API: GET AttributeTypes/{id}</para>
        /// </summary>
        /// <param name="id">ID of the Attribute Type</param>
        /// <returns></returns>
        public AttributeTypeResult Get(int id)
        {
            HttpResponseMessage response = _conn.Get(string.Format("AttributeTypes/{0}", id));
            AttributeTypeResult result = new AttributeTypeResult(response);
            return result;
        }

        /// <summary>
        /// Creates a new Attribute Type.
        /// <para>API: POST AttributeTypes</para>
        /// </summary>
        /// <param name="name">Name of the new Attribute Type</param>
        /// <returns></returns>
        public AttributeTypeResult Post(string name)
        {
            AttributeTypePostModel model = new AttributeTypePostModel() { Name = name };

            HttpResponseMessage response = _conn.Post("AttributeTypes", model);
            AttributeTypeResult result = new AttributeTypeResult(response);
            return result;
        }

        /// <summary>
        /// Deletes an Attribute Type and all related Attributes by ID.
        /// <para>API: DELETE AttributeTypes/{id}</para>
        /// </summary>
        /// <param name="id">ID of the Attribute Type</param>
        /// <returns></returns>
        public DeleteResult Delete(int id)
        {
            HttpResponseMessage response = _conn.Delete(string.Format("AttributeTypes/{0}", id));
            DeleteResult result = new DeleteResult(response);
            return result;
        }

    }
}
