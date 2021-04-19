using System.Text;

namespace BeyondTrust.BeyondInsight.PasswordSafe.API.Client.V3
{
    public class QueryParameterBuilder
    {
        /// <summary>
        /// Builds an optional query parameter string based on the Query Parameters in <paramref name="queryParams"/>.
        /// </summary>
        /// <param name="queryParams">List of query parameters for which to build a query parameter list.  If the <seealso cref="QueryParameter.Value"/> in a <seealso cref="QueryParameter" /> is null, the query parameter is excluded.</param>
        public static string Build(params QueryParameter[] queryParams)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var param in queryParams)
            {
                // assume that if the value is null, don't use it
                if (null == param.Value)
                    continue;

                // either start the query param
                if (sb.Length == 0)
                    sb.Append("?");
                else // or append to it
                    sb.Append("&");

                sb.Append($"{param.Key}={param.Value}");
            }

            return sb.ToString();
        }
    }

    public class QueryParameter
    {
        public QueryParameter(string key, object value)
        {
            Key = key;
            Value = value;
        }
        public string Key { get; set; }
        public object Value { get; set; }
    }
}
