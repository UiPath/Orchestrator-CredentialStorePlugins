using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using UiPath.Orchestrator.Extensibility.Configuration;
using UiPath.Orchestrator.Extensibility.SecureStores;

namespace UiPath.Samples.SecureStores.RandomPasswordGenerator
{
    /// <summary>
    /// A sample secure store plugin reference describing an implementation of secure store.
    /// This sample can be used as a fully functional read-only secure store implementation that will generate a random password
    /// </summary>
    public class RandomPassSecureStore : ISecureStore
    {
        private const string CharRepo = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz`~!@#$%^&*()-_=+[{]}\\|;:'\",<.>/?";
        private const string NameIdentifier = "RandomPass";
        private const double SuccessRate = 0.9;

        private readonly Random _random;

        public RandomPassSecureStore()
        {
            _random = new Random();
        }

        /// <summary>
        /// Initialize the Secure Store plugin with host level appsetings settings
        /// stored in web.config under the key {Plugins.SecureStores}.{Plugin_Friendy_name}.{SettingName}
        /// </summary>
        /// <param name="hostSettings"></param>
        public void Initialize(Dictionary<string, string> hostSettings)
        {
            // No-op : current implementation of RandomPass does not have a host level configuration
        }

        /// <summary>
        /// Retrieve information about the secure store.
        /// </summary>
        /// <returns></returns>
        public SecureStoreInfo GetStoreInfo()
        {
            return new SecureStoreInfo { Identifier = NameIdentifier, IsReadOnly = true };
        }

        /// <summary>
        /// Verify if a given context is supported for current secure store
        /// </summary>
        /// <returns></returns>
        public Task ValidateContextAsync(string context)
        {
            // try convert to context object
            JsonConvert.DeserializeObject<RandomPassContext>(context);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Define the configurations needed by this plugin
        /// Configurations defined here will show up in the Web App for user input
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ConfigurationEntry> GetConfiguration()
        {
            return new List<ConfigurationEntry>
            {
                // The CabFail defines wether this secure store would have a chance to fail
                new ConfigurationValue(ConfigurationValueType.Boolean)
                {
                    Key = "CanFail",
                    DisplayName = "Can Fail",
                    IsMandatory = false,
                },
            };
        }

        /// <summary>
        /// Retrieves the secure value for the key.
        /// </summary>
        /// <param name="context">The configuration settings</param>
        /// <param name="key">The key to be used to retrive the credential value</param>
        /// <returns>The credential value</returns>
        public Task<string> GetValueAsync(string context, string key)
        {
            MightInjectFailure(context);
            return Task.FromResult(RandomPassword());
        }

        /// <summary>
        /// Retrieves the credentials for the key.
        /// </summary>
        /// <param name="context">The configuration settings</param>
        /// <param name="key">The key to be used to retrive the credentials</param>
        /// <returns>The credentials, including both username and password</returns>
        public Task<Credential> GetCredentialsAsync(string context, string key)
        {
            MightInjectFailure(context);
            return Task.FromResult(new Credential
            {
                Username = key,
                Password = RandomPassword(),
            });
        }

        /// <summary>
        /// Create the given value with the given key. Returns the augmented key that should be used to
        /// retrieve or remove the value from the store it it needs to change, otherwise NULL.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public Task<string> CreateValueAsync(string context, string key, string value)
        {
            throw new SecureStoreException(
                SecureStoreException.Type.UnsupportedOperation,
                Resource.ResourceManager.GetString(nameof(Resource.ReadOnly), CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Create the given credentials with the given key. Returns the augmented key that should be used to
        /// retrieve or remove the credentials from the store it it needs to change, otherwise NULL.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public Task<string> CreateCredentialsAsync(string context, string key, Credential value)
        {
            throw new SecureStoreException(
                SecureStoreException.Type.UnsupportedOperation,
                Resource.ResourceManager.GetString(nameof(Resource.ReadOnly), CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Update the given value with the given key. Returns the augmented key that should be used to
        /// retrieve or remove the value from the store it it needs to change, otherwise NULL.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="key"></param>
        /// <param name="oldAugmentedKey">
        /// the value of the key returned from the CreateValueAsync when the credential was created,
        /// or the last subsequent UpdateValueAsync if any called
        /// </param>
        /// <param name="value"></param>
        /// <returns></returns>
        public Task<string> UpdateValueAsync(string context, string key, string oldAugmentedKey, string value)
        {
            throw new SecureStoreException(
                SecureStoreException.Type.UnsupportedOperation,
                Resource.ResourceManager.GetString(nameof(Resource.ReadOnly), CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Update the given credentials with the given key. Returns the augmented key that should be used to
        /// retrieve or remove the credentials from the store it it needs to change, otherwise NULL.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="key"></param>
        /// <param name="oldAugmentedKey">
        /// the value of the key returned from the CreateCredentialsAsync when the credential was created,
        /// or the last subsequent UpdateCredentialsAsync if any called
        /// </param>
        /// <param name="value"></param>
        /// <returns></returns>
        public Task<string> UpdateCredentialsAsync(string context, string key, string oldAugmentedKey, Credential value)
        {
            throw new SecureStoreException(
                SecureStoreException.Type.UnsupportedOperation,
                Resource.ResourceManager.GetString(nameof(Resource.ReadOnly), CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Removes the coresponding key/value pair from the store.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public Task RemoveValueAsync(string context, string key)
        {
            throw new SecureStoreException(
                SecureStoreException.Type.UnsupportedOperation,
                Resource.ResourceManager.GetString(nameof(Resource.ReadOnly), CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// When the 'CanFail' option is true in the context, fail the operation at a chance of (1.0 - SuccessRate)
        /// </summary>
        /// <param name="context">The configuration settings</param>
        private void MightInjectFailure(string context)
        {
            RandomPassContext contextDeserialized = JsonConvert.DeserializeObject<RandomPassContext>(context);
            if (contextDeserialized.CanFail)
            {
                if (_random.NextDouble() > SuccessRate)
                {
                    throw new SecureStoreException(
                        SecureStoreException.Type.SecretNotFound,
                        Resource.ResourceManager.GetString(nameof(Resource.SecretNotFound), CultureInfo.InvariantCulture));
                }
            }
        }

        /// <summary>
        /// Generate a random password with added complexity.
        /// </summary>
        /// <returns></returns>
        private string RandomPassword()
        {
            // Arbitrarily defined password length
            char[] chars = new char[16];

            // At least two digits
            chars[0] = CharRepo[_random.Next(0, 10)];
            chars[1] = CharRepo[_random.Next(0, 10)];

            // At least two capital letters
            chars[2] = CharRepo[_random.Next(10, 36)];
            chars[3] = CharRepo[_random.Next(10, 36)];

            // At least one special character
            chars[4] = CharRepo[_random.Next(62, CharRepo.Length)];

            // Pick any random char for the remaining password length
            for (int i = 5; i < chars.Length; i++)
            {
                chars[i] = CharRepo[_random.Next(CharRepo.Length)];
            }

            // Randomize array
            for (int i = chars.Length - 1; i > 0; i--)
            {
                int j = _random.Next(i + 1);
                char temp = chars[i];
                chars[i] = chars[j];
                chars[j] = temp;
            }

            return new string(chars);
        }
    }
}
