using System.Net.Http;

namespace BeyondTrust.BeyondInsight.PasswordSafe.API.Client.V3
{
    public sealed class AttributesEndpoint : BaseEndpoint
    {
        internal AttributesEndpoint(PasswordSafeAPIConnector client)
            : base(client)
        {
        }

        #region Attribute Definitions

        /// <summary>
        /// Returns a list of Attribute definitions by Attribute Type.
        /// <para>API: GET AttributeTypes/{attributeTypeID}/Attributes</para>
        /// </summary>
        /// <param name="attributeTypeID">ID of the Attribute Type</param>
        /// <returns></returns>
        public AttributesResult GetAll(int attributeTypeID)
        {
            HttpResponseMessage response = _conn.Get(string.Format("AttributeTypes/{0}/Attributes", attributeTypeID));
            AttributesResult result = new AttributesResult(response);
            return result;
        }

        /// <summary>
        /// Returns an Attribute definition by ID.
        /// <para>API: GET Attributes/{id}</para>
        /// </summary>
        /// <param name="id">ID of the Asset</param>
        /// <returns></returns>
        public AttributeResult Get(int id)
        {
            HttpResponseMessage response = _conn.Get(string.Format("Attributes/{0}", id));
            AttributeResult result = new AttributeResult(response);
            return result;
        }

        /// <summary>
        /// Creates a new Attribute definition by Attribute Type ID.
        /// <para>API: POST AttributeTypes/{attributeTypeID}/Attributes</para>
        /// </summary>
        /// <param name="attributeTypeID">ID of the Attribute Type</param>
        /// <returns></returns>
        public AttributeResult Post(int attributeTypeID, AttributePostModel model)
        {
            HttpResponseMessage response = _conn.Post(string.Format("AttributeTypes/{0}/Attributes", attributeTypeID), model);
            AttributeResult result = new AttributeResult(response);
            return result;
        }

        /// <summary>
        /// Deletes an Attribute definition by ID.
        /// <para>API: DELETE Attributes/{id}</para>
        /// </summary>
        /// <param name="id">ID of the Attribute</param>
        /// <returns></returns>
        public DeleteResult Delete(int id)
        {
            HttpResponseMessage response = _conn.Delete(string.Format("Attributes/{0}", id));
            DeleteResult result = new DeleteResult(response);
            return result;
        }

        #endregion

        #region Asset Attributes

        /// <summary>
        /// Returns a list of Attributes by Asset ID.
        /// <para>API: GET Assets/{assetID}/Attributes</para>
        /// </summary>
        /// <param name="assetID">ID of the Asset</param>
        /// <returns></returns>
        public AttributesResult GetAllByAsset(int assetID)
        {
            HttpResponseMessage response = _conn.Get(string.Format("Assets/{0}/Attributes", assetID));
            AttributesResult result = new AttributesResult(response);
            return result;
        }

        /// <summary>
        /// Assigns an Attribute to an Asset.
        /// <para>API: POST Assets/{assetID}/Attributes/{attributeID}</para>
        /// </summary>
        /// <param name="assetID">ID of the Attribute Type</param>
        /// <returns></returns>
        public AttributeResult Post(int assetID, int attributeID)
        {
            HttpResponseMessage response = _conn.Post(string.Format("Assets/{0}/Attributes/{1}", assetID, attributeID));
            AttributeResult result = new AttributeResult(response);
            return result;
        }

        /// <summary>
        /// Deletes all Asset Attributes by Asset ID.
        /// <para>API: DELETE Assets/{id}/Attributes</para>
        /// </summary>
        /// <param name="assetID">ID of the Asset</param>
        /// <returns></returns>
        public DeleteResult DeleteAll(int assetID)
        {
            HttpResponseMessage response = _conn.Delete(string.Format("Assets/{0}/Attributes", assetID));
            DeleteResult result = new DeleteResult(response);
            return result;
        }

        /// <summary>
        /// Deletes an Asset Attribute by Asset ID and Attribute ID.
        /// <para>API: DELETE Attributes/{id}</para>
        /// </summary>
        /// <param name="assetID">ID of the Asset</param>
        /// <param name="attributeID">ID of the Attribute</param>
        /// <returns></returns>
        public DeleteResult Delete(int assetID, int attributeID)
        {
            HttpResponseMessage response = _conn.Delete(string.Format("Assets/{0}/Attributes/{1}", assetID, attributeID));
            DeleteResult result = new DeleteResult(response);
            return result;
        }

        #endregion

    }
}
