using System.Globalization;
using UiPath.Orchestrator.Extensibility.SecureStores;
using UiPath.Orchestrator.Extensions.SecureStores.CyberArkCCP.Resources;

namespace UiPath.Orchestrator.Extensions.SecureStores.CyberArkCCP
{
    public static class SecureStoresUtil
    {
        public static void ThrowIfNull(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                throw new SecureStoreException(
                    SecureStoreException.Type.InvalidConfiguration,
                    "The value is null or empty.");
            }
        }
        public static string GetLocalizedResource(string resourceName, params object[] parameters)
        {
            var resource = GetLocalizedResourceInternal(CultureInfo.CurrentUICulture, resourceName, parameters);
            if (!string.IsNullOrEmpty(resource))
            {
                return resource;
            }

            return GetLocalizedResourceInternal(CultureInfo.InvariantCulture, resourceName, parameters);
        }

        private static string GetLocalizedResourceInternal(CultureInfo cultureInfo, string resourceName, params object[] parameters)
        {
            if (parameters != null && parameters.Length > 0)
            {
                var message = Resource.ResourceManager.GetString(resourceName, cultureInfo);

                return string.IsNullOrEmpty(message) ? null : string.Format(message, parameters);
            }

            return Resource.ResourceManager.GetString(resourceName, cultureInfo);
        }
    }
}
