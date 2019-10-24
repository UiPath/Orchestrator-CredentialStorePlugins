using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Globalization;
using System.Threading.Tasks;
using UiPath.Orchestrator.Extensibility.Configuration;
using UiPath.Orchestrator.Extensibility.SecureStores;

namespace UiPath.Samples.SecureStores.SqlPasswordStore
{
    /// <summary>
    /// A sample secure store plugin reference, describing an implementation of a secure store.
    /// This sample can be used as a fully functional read/write secure store implementation
    /// that connects to an SQL database and reads/writes credentials there.
    /// </summary>
    public class SqlPassSecureStore : ISecureStore
    {
        // configure the table name here, this will override the host setting table name
        public const string TableName = "SqlPassCredentials";

        private const string NameIdentifier = "SqlPass";

        private static readonly Func<OdbcCommand, Task<object>> executeNonQueryAsync =
            async (command) => await command.ExecuteNonQueryAsync();

        // host settings read during Initialize()
        public SqlPassContext HostSettings { get; private set; } = null;

        /// <summary>
        /// Initialize the Secure Store plugin with host level appSettings settings
        /// stored in web.config under the key {Plugins.SecureStores}.{Plugin_Friendly_Name}.{SettingName}
        /// </summary>
        /// <param name="hostSettings"></param>
        public void Initialize(Dictionary<string, string> hostSettings)
        {
            if (hostSettings == null || hostSettings.Count == 0)
            {
                return;
            }

            string serialized = JsonConvert.SerializeObject(hostSettings);
            HostSettings = JsonConvert.DeserializeObject<SqlPassContext>(serialized);

            // override host settings table name if it is not set
            if (string.IsNullOrEmpty(HostSettings.TableName))
            {
                HostSettings.TableName = TableName;
            }
        }

        /// <summary>
        /// Retrieve information about the secure store.
        /// </summary>
        /// <returns></returns>
        public SecureStoreInfo GetStoreInfo()
        {
            return new SecureStoreInfo { Identifier = NameIdentifier, IsReadOnly = false };
        }

        /// <summary>
        /// Verify if a given context is supported for current secure store
        /// Tries to connect to the DB given by the context
        /// </summary>
        /// <returns></returns>
        public async Task ValidateContextAsync(string context)
        {
            // verify context is able to be deserialized
            SqlPassContext config = DeserializeContext(context);

            // verify connection can be established
            string connectionString = BuildConnectionString(config);

            using (OdbcConnection connection = new OdbcConnection(connectionString))
            using (OdbcCommandBuilder builder = new OdbcCommandBuilder())
            {
                await connection.OpenAsync();

                string escapedTableName = builder.QuoteIdentifier(config.TableName, connection);

                try
                {
                    using (OdbcCommand command = new OdbcCommand($"SELECT TOP 1 * FROM {escapedTableName}", connection))
                    {
                        await command.ExecuteReaderAsync();
                    }
                }
                catch (OdbcException ex)
                {
                    if (!ex.Message.Contains($"Invalid object name '{config.TableName}'"))
                    {
                        throw;
                    }

                    string queryStringCreateTable = $@"CREATE TABLE {escapedTableName} (
                                                name varchar(96) NOT NULL PRIMARY KEY,
                                                value varchar(max) NOT NULL)";
                    using (OdbcCommand command = new OdbcCommand(queryStringCreateTable, connection))
                    {
                        await command.ExecuteNonQueryAsync();
                    }
                }
            }
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
                new ConfigurationValue(ConfigurationValueType.String)
                {
                    Key = "Driver",
                    DisplayName = "Driver",
                    IsMandatory = false,
                    DefaultValue = "SQL Server",
                },
                new ConfigurationValue(ConfigurationValueType.String)
                {
                    Key = "Server",
                    DisplayName = "Server",
                    IsMandatory = false,
                },
                new ConfigurationValue(ConfigurationValueType.String)
                {
                    Key = "UserId",
                    DisplayName = "User Id",
                    IsMandatory = false,
                },
                new ConfigurationValue(ConfigurationValueType.String)
                {
                    Key = "Password",
                    DisplayName = "Password",
                    IsMandatory = false,
                },
                new ConfigurationValue(ConfigurationValueType.Boolean)
                {
                    Key = "TrustedConnection",
                    DisplayName = "Trusted_Connection",
                    IsMandatory = false,
                },
                new ConfigurationValue(ConfigurationValueType.String)
                {
                    Key = "Databse",
                    DisplayName = "Database",
                    IsMandatory = false,
                },
                new ConfigurationValue(ConfigurationValueType.String)
                {
                    Key = "TableName",
                    DisplayName = "Table Name",
                    IsMandatory = false,
                },
                new ConfigurationValue(ConfigurationValueType.Boolean)
                {
                    Key = "IsEncrypted",
                    DisplayName = "Is Encrypted",
                    IsMandatory = false,
                },
            };
        }

        /// <summary>
        /// Retrieves the secure value for the key.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<string> GetValueAsync(string context, string key)
        {
            return await GetValueInternalAsync(context, key);
        }

        /// <summary>
        /// Retrieves the credentials for the key.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<Credential> GetCredentialsAsync(string context, string key)
        {
            string value = await GetValueInternalAsync(context, key);
            return JsonConvert.DeserializeObject<Credential>(value);
        }

        /// <summary>
        /// Set the given value with the given key. Returns the augmented key that should be used to
        /// retrieve or remove the value from the store it needs to change, otherwise NULL.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="key">the external name, in this implementation it is not saved, therefore no usage.</param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<string> CreateValueAsync(string context, string key, string value)
        {
            return await CreateValueInternalAsync(context, key, value);
        }

        /// <summary>
        /// Set the given credentials with the given key. Returns the augmented key that should be used to
        /// retrieve or remove the credentials from the store it needs to change, otherwise NULL.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="key">the external name, in this implementation it is not saved, therefore no usage.</param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<string> CreateCredentialsAsync(string context, string key, Credential value)
        {
            return await CreateValueInternalAsync(context, key, JsonConvert.SerializeObject(value));
        }

        public async Task<string> UpdateValueAsync(string context, string key, string oldAugmentedKey, string value)
        {
            return await UpdateValueInternalAsync(context, key, oldAugmentedKey, value);
        }

        public async Task<string> UpdateCredentialsAsync(string context, string key, string oldAugmentedKey, Credential value)
        {
            return await UpdateValueInternalAsync(context, key, oldAugmentedKey, JsonConvert.SerializeObject(value));
        }

        /// <summary>
        /// Removes the corresponding key/value pair from the store.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task RemoveValueAsync(string context, string key)
        {
            await RemoveValueInternalAsync(context, key);
        }

        private static string BuildConnectionString(SqlPassContext context)
        {
            OdbcConnectionStringBuilder connectionStringBuilder = new OdbcConnectionStringBuilder
            {
                Driver = context.Driver,
            };
            connectionStringBuilder.Add("Server", context.Server);
            connectionStringBuilder.Add("Database", context.Database);
            connectionStringBuilder.Add("User ID", context.UserId);
            connectionStringBuilder.Add("Password", context.Password);
            connectionStringBuilder.Add("Trusted_Connection", context.TrustedConnection);
            return connectionStringBuilder.ConnectionString;
        }

        private async Task<string> GetValueInternalAsync(string context, string key)
        {
            ValidateInput(key);
            SqlPassContext config = DeserializeContext(context);

            string queryTemplate = "SELECT value FROM {0} WHERE name = ?";
            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                { "name", key },
            };

            string value = (string)await OpenConnectionAndExecute(config, queryTemplate, parameters, async (command) =>
            {
                using (OdbcDataReader reader = command.ExecuteReader())
                {
                    if (await reader.ReadAsync())
                    {
                        return reader["value"].ToString();
                    }
                    else
                    {
                        throw new SecureStoreException(
                            SecureStoreException.Type.SecretNotFound,
                            Resource.ResourceManager.GetString(nameof(Resource.SecretNotFound), CultureInfo.InvariantCulture));
                    }
                }
            });

            return config.IsEncrypted.Value ? CryptoHelper.Decrypt(value) : value;
        }

        private async Task<string> CreateValueInternalAsync(string context, string key, string value)
        {
            ValidateInput(key);
            ValidateInput(value);
            SqlPassContext config = DeserializeContext(context);

            string queryTemplate = "INSERT INTO {0} (name, value) VALUES (?, ?)";
            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                { "name", key },
                { "value", config.IsEncrypted.Value ? CryptoHelper.Encrypt(value) : value },
            };

            await VerifyRowsAffected(
                async () => (int)await OpenConnectionAndExecute(config, queryTemplate, parameters, executeNonQueryAsync));
            return key;
        }

        private async Task<string> UpdateValueInternalAsync(string context, string key, string oldAugmentedKey, string value)
        {
            ValidateInput(key);
            ValidateInput(oldAugmentedKey);
            ValidateInput(value);
            SqlPassContext config = DeserializeContext(context);

            string queryTemplate = "UPDATE {0} SET name = ?, value = ? WHERE name = ?";
            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                { "name", key },
                { "value", config.IsEncrypted.Value ? CryptoHelper.Encrypt(value) : value },
                { "oldName", oldAugmentedKey },
            };

            await VerifyRowsAffected(
                async () => (int)await OpenConnectionAndExecute(config, queryTemplate, parameters, executeNonQueryAsync));
            return key;
        }

        private async Task RemoveValueInternalAsync(string context, string key)
        {
            ValidateInput(key);
            SqlPassContext config = DeserializeContext(context);

            string queryTemplate = "DELETE FROM {0} WHERE name = ?";
            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                { "name", key },
            };

            await VerifyRowsAffected(
                async () => (int)await OpenConnectionAndExecute(config, queryTemplate, parameters, executeNonQueryAsync));
        }

        private async Task<object> OpenConnectionAndExecute(SqlPassContext context, string queryTemplate, Dictionary<string, string> parameters, Func<OdbcCommand, Task<object>> action)
        {
            string connectionString = BuildConnectionString(context);

            using (OdbcConnection connection = new OdbcConnection(connectionString))
            using (OdbcCommandBuilder builder = new OdbcCommandBuilder())
            {
                await connection.OpenAsync();

                string escapedTableName = builder.QuoteIdentifier(context.TableName, connection);
                string queryString = string.Format(CultureInfo.InvariantCulture, queryTemplate, escapedTableName);

                using (OdbcCommand command = new OdbcCommand(queryString, connection))
                {
                    foreach (var key in parameters.Keys)
                    {
                        if (parameters.TryGetValue(key, out var value))
                        {
                            command.Parameters.AddWithValue($"@{key}", value);
                        }
                    }

                    return await action(command);
                }
            }
        }

        private void ValidateInput(string arg)
        {
            if (string.IsNullOrEmpty(arg))
            {
                throw new ArgumentNullException(nameof(arg));
            }
        }

        private async Task VerifyRowsAffected(Func<Task<int>> action)
        {
            int rowsAffected = await action();
            if (rowsAffected == 0)
            {
                throw new SecureStoreException(
                            SecureStoreException.Type.SecretNotFound,
                            Resource.ResourceManager.GetString(nameof(Resource.SecretNotFound), CultureInfo.InvariantCulture));
            }
        }

        private SqlPassContext DeserializeContext(string context)
        {
            var config = JsonConvert.DeserializeObject<SqlPassContext>(context);

            if (HostSettings != null)
            {
                if (string.IsNullOrEmpty(config.Driver))
                {
                    config.Driver = HostSettings.Driver;
                }

                if (string.IsNullOrEmpty(config.Server))
                {
                    config.Server = HostSettings.Server;
                }

                if (string.IsNullOrEmpty(config.UserId))
                {
                    config.UserId = HostSettings.UserId;
                }

                if (string.IsNullOrEmpty(config.Password))
                {
                    config.Password = HostSettings.Password;
                }

                if (!config.TrustedConnection.HasValue)
                {
                    config.TrustedConnection = HostSettings.TrustedConnection;
                }

                if (string.IsNullOrEmpty(config.Database))
                {
                    config.Database = HostSettings.Database;
                }

                if (string.IsNullOrEmpty(config.TableName))
                {
                    config.TableName = HostSettings.TableName;
                }

                if (!config.IsEncrypted.HasValue)
                {
                    config.IsEncrypted = HostSettings.IsEncrypted;
                }
            }

            return config;
        }
    }
}
