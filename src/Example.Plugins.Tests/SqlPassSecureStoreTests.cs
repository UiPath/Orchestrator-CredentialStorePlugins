using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data.Odbc;
using System.Data.SqlClient;
using UiPath.Orchestrator.Extensibility.SecureStores;
using UiPath.Orchestrator.Extensibility.Configuration;
using Newtonsoft.Json;
using Xunit;
using FluentAssertions;

namespace UiPath.Samples.SecureStores.SqlPasswordStore
{
    public class DatabaseFixture : IDisposable
    {
        public DatabaseFixture()
        {
        }

        public void Dispose()
        {
            var connStrBuilder = new SqlConnectionStringBuilder
            {
                { "Server", DefaultServer },
                { "Database", DefaultDatabase },
                { "Trusted_Connection", true },
            };
            using (var connection = new SqlConnection(connStrBuilder.ConnectionString))
            using (var cmdBuilder = new SqlCommandBuilder())
            {
                var escapedTableName = cmdBuilder.QuoteIdentifier(DefaultTableName);
                try
                {
                    using (var command = new SqlCommand($"DROP TABLE {escapedTableName}", connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
                catch { }
            }
        }

        public string DefaultServer { get; } = ".\\SQLEXPRESS";
        public string DefaultDatabase { get; } = "UiPath";
        public string DefaultTableName { get; } = "SqlPassSecureStoreTestsCredentials";
    }

    public class SqlPassSecureStoreTests : IClassFixture<DatabaseFixture>
    {
        private const string NotJsonString = "This is not a JSON string!";
        private const string SecretNotFound = "Secret not found.";

        private readonly DatabaseFixture fixture;
        private readonly string contextString;

        private readonly SqlPassSecureStore subject = new SqlPassSecureStore();

        public SqlPassSecureStoreTests(DatabaseFixture fixture)
        {
            this.fixture = fixture;
            contextString = JsonConvert.SerializeObject(new SqlPassContext
            {
                Driver = "SQL Server",
                Server = fixture.DefaultServer,
                Database = fixture.DefaultDatabase,
                TableName = fixture.DefaultTableName,
                TrustedConnection = true,
                IsEncrypted = false,
            });
        }

        [Fact]
        public void InitializeNonNullNonEmptySettingsInstantiatesHostSettings()
        {
            var hostSettings = new Dictionary<string, string>
            {
                { "Driver", "driver" }
            };

            var expected = new SqlPassContext
            {
                Driver = "driver",
                TableName = SqlPassSecureStore.TableName,
            };

            subject.Initialize(hostSettings);
            subject.HostSettings.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void InitializeNullOrEmptySettingsDoesNothing()
        {
            subject.Initialize(null);
            Assert.Null(subject.HostSettings);

            subject.Initialize(new Dictionary<string, string>());
            Assert.Null(subject.HostSettings);
        }

        [Fact]
        public void GetStoreInfoReturnsCorrectObject()
        {
            var expected = new SecureStoreInfo
            {
                Identifier = "SqlPass",
                IsReadOnly = false,
            };

            var actual = subject.GetStoreInfo();
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async void ValidateContextAsyncThrowsJsonReaderExceptionGivenInvalidJsonString()
        {
            await Assert.ThrowsAsync<JsonReaderException>(
                () => subject.ValidateContextAsync(NotJsonString));
        }

        [Fact]
        public async void ValidateContextAsyncThrowsOdbcExceptionGivenValidJsonStringWithIncorrectConnectionInfo()
        {
            var context = new Dictionary<string, string>
            {
                { "Driver", "x" },
                { "DataSource", "x" },
                { "Database", "x" },
                { "DbTableName", "x" },
            };

            await Assert.ThrowsAsync<OdbcException>(
                () => subject.ValidateContextAsync(JsonConvert.SerializeObject(context)));
        }

        [Fact]
        public void GetConfigurationReturnsCorrectObject()
        {
            var expected = new List<ConfigurationEntry>
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

            var actual = subject.GetConfiguration();
            actual.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public async void SecureStoreCrudTests_Password(bool isEncrypted)
        {
            // create input context string
            string contextString = GetContextString(fixture, isEncrypted);

            // validate and create table
            await subject.ValidateContextAsync(contextString);

            // CREATE
            var expectedPasswordKey = Guid.NewGuid().ToString();
            {
                // create arbitrary password
                var expectedPassword = Guid.NewGuid().ToString();
                var actualPasswordKey = await subject.CreateValueAsync(contextString, expectedPasswordKey, expectedPassword);

                // read and validate created
                actualPasswordKey.Should().BeEquivalentTo(expectedPasswordKey);
                var actualPassword = await subject.GetValueAsync(contextString, actualPasswordKey);
                actualPassword.Should().BeEquivalentTo(expectedPassword);

                // GetCredentialsAsync on password should throw JsonReaderException
                var ex = await Assert.ThrowsAsync<JsonReaderException>(
                    () => subject.GetCredentialsAsync(contextString, actualPasswordKey));
            }

            // UPDATE
            var oldPasswordKey = expectedPasswordKey;
            expectedPasswordKey = Guid.NewGuid().ToString();
            {
                // create arbitrary value and update
                var expectedPassword = Guid.NewGuid().ToString();
                var actualPasswordKey = await subject.UpdateValueAsync(contextString, expectedPasswordKey, oldPasswordKey, expectedPassword);

                // read and validate updated
                actualPasswordKey.Should().BeEquivalentTo(expectedPasswordKey);
                var actualPassword = await subject.GetValueAsync(contextString, actualPasswordKey);
                actualPassword.Should().BeEquivalentTo(expectedPassword);
                await VerifySecretNotFound(() => subject.GetValueAsync(contextString, oldPasswordKey));
            }

            // DELETE
            // delete existing rows
            await subject.RemoveValueAsync(contextString, expectedPasswordKey);

            // verify deleted
            // RUD operations on non existing items should fail
            // get should throw SecureStoreException
            await VerifySecretNotFound(() => subject.GetValueAsync(contextString, expectedPasswordKey));

            // update should throw SecureStoreException
            await VerifySecretNotFound(() => subject.UpdateValueAsync(contextString, expectedPasswordKey, expectedPasswordKey, Guid.NewGuid().ToString()));

            // delete should throw SecureStoreException
            await VerifySecretNotFound(() => subject.RemoveValueAsync(contextString, expectedPasswordKey));
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public async void SecureStoreCrudTests_Credentials(bool isEncrypted)
        {
            // create input context string
            string contextString = GetContextString(fixture, isEncrypted);

            // validate and create table
            await subject.ValidateContextAsync(contextString);

            // CREATE
            var expectedCredentialsKey = Guid.NewGuid().ToString();
            {
                // create arbitrary credential
                var expectedCredentials = new Credential
                {
                    Username = Guid.NewGuid().ToString(),
                    Password = Guid.NewGuid().ToString(),
                };
                var actualCredentialsKey = await subject.CreateCredentialsAsync(contextString, expectedCredentialsKey, expectedCredentials);

                // read and validate created
                actualCredentialsKey.Should().BeEquivalentTo(expectedCredentialsKey);
                var actualCredentials = await subject.GetCredentialsAsync(contextString, actualCredentialsKey);
                actualCredentials.Should().BeEquivalentTo(expectedCredentials);

                // GetValueAsync on credentials should return a valid JSON string
                actualCredentials = JsonConvert.DeserializeObject<Credential>(await subject.GetValueAsync(contextString, actualCredentialsKey));
                actualCredentials.Should().BeEquivalentTo(expectedCredentials);
            }

            // UPDATE
            var oldCredentialsKey = expectedCredentialsKey;
            expectedCredentialsKey = Guid.NewGuid().ToString();
            {
                // create arbitrary credential and update
                var expectedCredentials = new Credential
                {
                    Username = Guid.NewGuid().ToString(),
                    Password = Guid.NewGuid().ToString(),
                };
                var actualCredentialsKey = await subject.UpdateCredentialsAsync(contextString, expectedCredentialsKey, oldCredentialsKey, expectedCredentials);

                // read and validate updated
                actualCredentialsKey.Should().BeEquivalentTo(expectedCredentialsKey);
                var actualCredentials = await subject.GetCredentialsAsync(contextString, actualCredentialsKey);
                actualCredentials.Should().BeEquivalentTo(expectedCredentials);
                await VerifySecretNotFound(() => subject.GetValueAsync(contextString, oldCredentialsKey));
            }

            // DELETE
            // delete existing rows
            await subject.RemoveValueAsync(contextString, expectedCredentialsKey);

            // verify deleted and RUD operations on non existing items should fail
            // get should throw SecureStoreException
            await VerifySecretNotFound(() => subject.GetCredentialsAsync(contextString, expectedCredentialsKey));

            // update should throw SecureStoreException
            await VerifySecretNotFound(() => subject.UpdateCredentialsAsync(contextString, expectedCredentialsKey, expectedCredentialsKey, new Credential
            {
                Username = Guid.NewGuid().ToString(),
                Password = Guid.NewGuid().ToString(),
            }));

            // delete should throw SecureStoreException
            await VerifySecretNotFound(() => subject.RemoveValueAsync(contextString, expectedCredentialsKey));
        }

        private string GetContextString(DatabaseFixture fixture, bool isEncrypted)
        {
            return JsonConvert.SerializeObject(new SqlPassContext
            {
                Driver = "SQL Server",
                Server = fixture.DefaultServer,
                Database = fixture.DefaultDatabase,
                TableName = fixture.DefaultTableName,
                TrustedConnection = true,
                IsEncrypted = isEncrypted,
            });
        }

        private async Task VerifySecretNotFound(Func<Task> action)
        {
            var ex = await Assert.ThrowsAsync<SecureStoreException>(action);
            ex.Message.Should().BeEquivalentTo(SecretNotFound);
        }
    }
}
