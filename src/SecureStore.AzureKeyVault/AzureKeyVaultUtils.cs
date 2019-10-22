using System.Globalization;
using System.Threading;
using UiPath.Orchestrator.AzureKeyVault.SecureStore;

namespace UiPath.Orchestrator.Extensions.SecureStores.AzureKeyVault
{
    public static class AzureKeyVaultUtils
    {
        public static string GetLocalizedResource(string resourceName, params object[] parameters)
        {
            var resource = GetLocalizedResource(Thread.CurrentThread.CurrentUICulture, resourceName, parameters);
            if (!string.IsNullOrEmpty(resource))
            {
                return resource;
            }

            return GetLocalizedResource(CultureInfo.InvariantCulture, resourceName, parameters);
        }

        private static string GetLocalizedResource(CultureInfo cultureInfo, string resourceName, params object[] parameters)
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
