using System.Net.Http;

namespace BeyondTrust.BeyondInsight.PasswordSafe.API.Client.V3
{
    public sealed class ReplayEndpoint : BaseEndpoint
    {
        internal ReplayEndpoint(PasswordSafeAPIConnector client)
            : base(client)
        {
        }
        
        /// <summary>
        /// Creates a new replay session for a specified session token. The session token can be discovered using the Sessions endpoints.
        /// <para>API: POST pbsm/replay</para>
        /// </summary>
        /// <param name="model">The Replay POST model</param>
        /// <returns></returns>
        public ReplayPostResult Post(ReplayPostModel model)
        {
            HttpResponseMessage response = _conn.Post("pbsm/replay", model);
            ReplayPostResult result = new ReplayPostResult(response);
            return result;
        }

        /// <summary>
        /// Displays the replay session details.
        /// <para>API: GET pbsm/replay/{replayId}</para>
        /// </summary>
        /// <param name="replayId">ID of the replay session returned from POST pbsm/replay</param>
        /// <returns></returns>
        public ReplayResult Get(string replayId)
        {
            HttpResponseMessage response = _conn.Get(string.Format("pbsm/replay/{0}", replayId));
            ReplayResult result = new ReplayResult(response);
            return result;
        }

        /// <summary>
        /// Returns a full-size jpeg image of the current RDP replay session.
        /// <para>API: GET pbsm/replay/{replayId}?jpeg=1</para>
        /// </summary>
        /// <param name="replayId">ID of the replay session returned from POST pbsm/replay</param>
        /// <returns></returns>
        public APIStreamResult GetJpeg(string replayId)
        {
            return GetJpeg(replayId, 1);
        }

        /// <summary>
        /// Returns a jpeg image of the current RDP replay session scaled in size by the given scale.
        /// <para>API: GET pbsm/replay/{replayId}?jpeg={scale}</para>
        /// </summary>
        /// <param name="replayId">ID of the replay session returned from POST pbsm/replay</param>
        /// <param name="scale">The scale of the image.  1==1/1 (full size), 2==1/2 (half size), 3==1/3 (one-third size), 4==1/4 (quarter size), etc.</param>
        /// <returns></returns>
        public APIStreamResult GetJpeg(string replayId, int scale)
        {
            HttpResponseMessage response = _conn.Get(string.Format("pbsm/replay/{0}?jpeg={1}", replayId, scale));
            APIStreamResult result = new APIStreamResult(response);
            return result;
        }

        /// <summary>
        /// Returns a full-size png image of the current RDP replay session.
        /// <para>API: GET pbsm/replay/{replayId}?png=1</para>
        /// </summary>
        /// <param name="replayId">ID of the replay session returned from POST pbsm/replay</param>
        /// <returns></returns>
        public APIStreamResult GetPng(string replayId)
        {
            return GetPng(replayId, 1);
        }

        /// <summary>
        /// Returns a png image of the current RDP replay session scaled in size by the given scale.
        /// <para>API: GET pbsm/replay/{replayId}?png={scale}</para>
        /// </summary>
        /// <param name="replayId">ID of the replay session returned from POST pbsm/replay</param>
        /// <param name="scale">The scale of the image.  1==1/1 (full size), 2==1/2 (half size), 3==1/3 (one-third size), 4==1/4 (quarter size), etc.</param>
        /// <returns></returns>
        public APIStreamResult GetPng(string replayId, int scale)
        {
            HttpResponseMessage response = _conn.Get(string.Format("pbsm/replay/{0}?png={1}", replayId, scale));
            APIStreamResult result = new APIStreamResult(response);
            return result;
        }

        /// <summary>
        /// Requests a text representation of the current SSH session.
        /// <para>API: GET pbsm/replay/{replayId}?screen=1</para>
        /// </summary>
        /// <param name="replayId">ID of the replay session returned from POST pbsm/replay</param>
        /// <returns></returns>
        public APIStreamResult GetText(string replayId)
        {
            HttpResponseMessage response = _conn.Get(string.Format("pbsm/replay/{0}?screen=1", replayId));
            APIStreamResult result = new APIStreamResult(response);
            return result;
        }

        /// <summary>
        /// Controls the replay session status.
        /// <para>API: PUT pbsm/replay/{replayId}</para>
        /// </summary>
        /// <param name="replayId">ID of the replay session returned from POST pbsm/replay</param>
        /// <param name="model">The Replay PUT model</param>
        /// <returns></returns>
        public ReplayResult Put(string replayId, ReplayPutModel model)
        {
            HttpResponseMessage response = _conn.Put(string.Format("pbsm/replay/{0}", replayId), model);
            ReplayResult result = new ReplayResult(response);
            return result;
        }

        /// <summary>
        /// Terminates the replay session.
        /// <para>API: DELETE pbsm/replay/{replayId}</para>
        /// </summary>
        /// <param name="replayId">ID of the replay session returned from POST pbsm/replay</param>
        /// <returns></returns>
        public DeleteResult Delete(string replayId)
        {
            HttpResponseMessage response = _conn.Delete(string.Format("pbsm/replay/{0}", replayId));
            DeleteResult result = new DeleteResult(response);
            return result;
        }

    }
}
