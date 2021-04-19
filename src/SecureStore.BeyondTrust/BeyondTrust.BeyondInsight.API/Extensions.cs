using System.Net.Http;

namespace BeyondTrust.BeyondInsight.PasswordSafe.API.Client
{
    static class Extensions
    {

        /// <summary>
        /// Adds a new (or updates an existing) header on the HttpClient.DefaultRequestHeaders.
        /// </summary>
        /// <param name="name">The name of the header.</param>
        /// <param name="value">The value of the header.</param>
        public static void AddDefaultRequestHeader(this HttpClient client, string name, string value)
        {
            if (client.DefaultRequestHeaders.Contains(name))
                client.DefaultRequestHeaders.Remove(name);

            client.DefaultRequestHeaders.Add(name, value);
        }

    }
}
