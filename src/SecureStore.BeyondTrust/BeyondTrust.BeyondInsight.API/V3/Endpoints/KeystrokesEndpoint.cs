using System.Net.Http;

namespace BeyondTrust.BeyondInsight.PasswordSafe.API.Client.V3
{
    public sealed class KeystrokesEndpoint : BaseEndpoint
    {
        internal KeystrokesEndpoint(PasswordSafeAPIConnector client)
            : base(client)
        {
        }

        public enum KeystrokeType
        {
            All=0, 
            StdIn=1, 
            StdOut=2, 
            StdErr=3, 
            Application=4, 
            WindowTitle=5
        }

        /// <summary>
        /// Returns a Keystroke by ID.
        /// <para>API: GET Keystrokes/{id}</para>
        /// </summary>
        /// <param name="id">ID of the Keystroke</param>
        /// <returns></returns>
        public KeystrokeResult Get(long id)
        {
            HttpResponseMessage response = _conn.Get(string.Format("Keystrokes/{0}", id));
            KeystrokeResult result = new KeystrokeResult(response);
            return result;
        }

        /// <summary>
        /// Returns a list of Keystrokes by Session ID.
        /// <para>API: GET Sessions/{sessionId}/Keystrokes</para>
        /// </summary>
        /// <param name="sessionID">ID of the Session</param>
        /// <returns></returns>
        public KeystrokesResult GetAll(int sessionID)
        {
            HttpResponseMessage response = _conn.Get(string.Format("Sessions/{0}/Keystrokes", sessionID));
            KeystrokesResult result = new KeystrokesResult(response);
            return result;
        }

        /// <summary>
        /// Search for Keystrokes by condition.
        /// <para>API: GET Keystrokes/search/{condition}</para>
        /// </summary>
        /// <param name="condition">Keyword to search for</param>
        /// <returns></returns>
        public KeystrokesResult Search(string condition)
        {
            HttpResponseMessage response = _conn.Get(string.Format("Keystrokes/search/{0}", condition));
            KeystrokesResult result = new KeystrokesResult(response);
            return result;
        }
        
        /// <summary>
        /// Search for Keystrokes by condition and keystroke type.
        /// <para>API: GET Keystrokes/search/{condition}/{type}</para>
        /// </summary>
        /// <param name="condition">Keyword to search for</param>
        /// <param name="type">Type of keystrokes to search</param>
        /// <returns></returns>
        public KeystrokesResult Search(string condition, KeystrokeType type)
        {
            int t = (int)type;
            HttpResponseMessage response = _conn.Get(string.Format("Keystrokes/search/{0}/{1}", condition, t));
            KeystrokesResult result = new KeystrokesResult(response);
            return result;
        }

    }
}
