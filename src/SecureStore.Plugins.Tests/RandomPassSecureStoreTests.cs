using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UiPath.Orchestrator.Extensibility.Configuration;
using UiPath.Orchestrator.Extensibility.SecureStores;
using Xunit;
using FluentAssertions;
using Newtonsoft.Json;
using System.Linq;

namespace UiPath.Samples.SecureStores.RandomPasswordGenerator
{
    public class RandomPassSecureStoreTests
    {
        private const string ContextCanFailTrue = "{\"CanFail\": true}";
        private const string ContextCanFailFalse = "{\"CanFail\": false}";
        private const string NotJsonString = "This is not a JSON string!";
        private const string SpecialChars = "`~!@#$%^&*()-_=+[{]}\\|;:'\",<.>/?";
        private const string DefaultKey = "ExternalName";
        private const string DefaultPasswordKey = "PasswordKey";
        private const string SecretNotFound = "Secret not found.";
        private const string ReadOnly = "RandomPass is read-only.";
        private const int PasswordLength = 16;

        private readonly RandomPassSecureStore subject = new RandomPassSecureStore();

        [Fact]
        public void InitializeDoesNothing()
        {
            subject.Initialize(new Dictionary<string, string>());
        }

        [Fact]
        public void GetStoreInfoReturnsCorrectObject()
        {
            var expected = new SecureStoreInfo
            {
                Identifier = "RandomPass",
                IsReadOnly = true,
            };

            var actual = subject.GetStoreInfo();
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void ValidateContextAsyncSucceedsGivenValidJsonString()
        {
            subject.ValidateContextAsync(ContextCanFailTrue);
        }

        [Fact]
        public async void ValidateContextAsyncFailsGivenInvalidJsonString()
        {
            var ex = await Assert.ThrowsAsync<JsonReaderException>(
                () => subject.ValidateContextAsync(NotJsonString));
        }

        [Fact]
        public void GetConfigurationReturnsCorrectObject()
        {
            var expected = new List<ConfigurationEntry>
            {
                new ConfigurationValue(ConfigurationValueType.Boolean)
                {
                    Key = "CanFail",
                    DisplayName = "Can Fail",
                    IsMandatory = false,
                },
            };

            var actual = subject.GetConfiguration();
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async void GetValueAsyncWithCanFailFalseReturnsRandomPassword()
        {
            string password = await subject.GetValueAsync(ContextCanFailFalse, string.Empty);
            Assert.True(IsPasswordValid(password));
        }
        
        [Fact]
        public async void GetValueAsyncWIthCanFailTrueHasATenPercentChanceOfFailure()
        {
            double expectedFailRate = 0.1;
            double actualFailRate = await GetFailureRateAsync(
                () => subject.GetValueAsync(ContextCanFailTrue, string.Empty), 1000);
            Assert.Equal(expectedFailRate, actualFailRate);
        }

        [Fact]
        public async void GetCredentialsAsyncWithCanFailFalseReturnsCredentialWithGivenKeyAsUserNameAndARandomPassword()
        {
            var credential = await subject.GetCredentialsAsync(ContextCanFailFalse, DefaultKey);
            credential.Username.Should().BeEquivalentTo(DefaultKey);
            Assert.True(IsPasswordValid(credential.Password));
        }

        [Fact]
        public async void GetCredentialsAsyncWithCanFailTrueHasATenPercentChanceOfFailure()
        {
            double expectedFailRate = 0.1;
            double actualFailRate = await GetFailureRateAsync(
                () => subject.GetCredentialsAsync(ContextCanFailTrue, DefaultKey), 1000);
            Assert.Equal(expectedFailRate, actualFailRate);
        }

        [Fact]
        public async void CreateValueAsyncThrowsSecureStoreExceptionWithReadOnlyMessage()
        {
            var ex = await Assert.ThrowsAsync<SecureStoreException>(
                () => subject.CreateValueAsync(ContextCanFailFalse, DefaultKey, string.Empty));
            ex.Message.Should().BeEquivalentTo(ReadOnly);
        }

        [Fact]
        public async void CreateCredentialsAsyncThrowsSecureStoreExceptionWithReadOnlyMessage()
        {
            var ex = await Assert.ThrowsAsync<SecureStoreException>(
                () => subject.CreateCredentialsAsync(ContextCanFailFalse, DefaultKey, new Credential()));
            ex.Message.Should().BeEquivalentTo(ReadOnly);
        }

        [Fact]
        public async void UpdateValueAsyncThrowsSecureStoreExceptionWithReadOnlyMessage()
        {
            var ex = await Assert.ThrowsAsync<SecureStoreException>(
                () => subject.UpdateValueAsync(ContextCanFailFalse, DefaultKey, DefaultPasswordKey, string.Empty));
            ex.Message.Should().BeEquivalentTo(ReadOnly);
        }

        [Fact]
        public async void UpdateCredentialsAsyncThrowsSecureStoreExceptionWithReadOnlyMessage()
        {
            var ex = await Assert.ThrowsAsync<SecureStoreException>(
                () => subject.UpdateCredentialsAsync(ContextCanFailFalse, DefaultKey, DefaultPasswordKey, new Credential()));
            ex.Message.Should().BeEquivalentTo(ReadOnly);
        }

        [Fact]
        public async void RemoveValueAsyncThrowsSecureStoreExceptionWithReadOnlyMessage()
        {
            var ex = await Assert.ThrowsAsync<SecureStoreException>(
                () => subject.RemoveValueAsync(ContextCanFailFalse, DefaultKey));
            ex.Message.Should().BeEquivalentTo(ReadOnly);
        }

        private bool IsPasswordValid(string password)
        {
            if (password.Length != PasswordLength)
            {
                return false;
            }

            int digitCount = 0, capitalCount = 0, specialCount = 0;
            foreach (char c in password)
            {
                if (SpecialChars.Contains(c))
                {
                    specialCount++;
                }
                else if (char.IsDigit(c))
                {
                    digitCount++;
                }
                else if (char.IsUpper(c))
                {
                    capitalCount++;
                }
            }

            return digitCount >= 2 && capitalCount >= 2 && specialCount >= 1;
        }

        private async Task<double> GetFailureRateAsync(Func<Task> func, int times)
        {
            int failCount = 0;
            for (int i = 0; i < times; i++)
            {
                try
                {
                    await func();
                }
                catch (SecureStoreException ex)
                {
                    ex.Message.Should().BeEquivalentTo(SecretNotFound);
                    failCount++;
                }
            }

            return Math.Round(failCount / (double)times, 1);
        }
    }
}
