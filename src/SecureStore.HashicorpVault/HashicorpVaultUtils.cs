using System.Globalization;
using UiPath.Orchestrator.Extensions.SecureStores.HashicorpVault.Resources;

namespace UiPath.Orchestrator.Extensions.SecureStores.HashicorpVault
{
    public static class HashicorpVaultUtils
    {
        public static string GetLocalizedResource(string resourceName, params object[] parameters)
        {
            var resource = GetLocalizedResourceInCulture(CultureInfo.CurrentUICulture, resourceName, parameters);
            if (!string.IsNullOrEmpty(resource))
            {
                return resource;
            }

            return GetLocalizedResourceInCulture(CultureInfo.InvariantCulture, resourceName, parameters);
        }

        private static string GetLocalizedResourceInCulture(CultureInfo cultureInfo, string resourceName, params object[] parameters)
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
