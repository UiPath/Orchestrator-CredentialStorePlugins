using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UiPath.Orchestrator.Extensibility.Configuration;
using UiPath.Orchestrator.Extensibility.SecureStores;
using Xunit;
using FluentAssertions;
using Newtonsoft.Json;
using System.Linq;
using UiPath.Orchestrator.Extensions.SecureStores.CyberArkCCP;
using System.Reflection;
using Moq;
using System.Net.Http;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Security.Cryptography;
using System.IO;

namespace UiPath.Samples.SecureStores.CyberArkCCPStore
{
    public class CyberArkCCPSecureStoreTests
    {
        private string ContextIsValidTrue = JsonConvert.SerializeObject(
            new CyberArkCCPOptions()
            {
                ApplicationId = "AppID",
                Folder = "Folder",
                Safe = "Safe",
                Thumbprint = "Thumbprint",
                URL = "https://localhost"
            });
        private const string NotJsonString = "This is not a JSON string!";

        private readonly CyberArkSecureStoreCCP subject = new CyberArkSecureStoreCCP();
        private readonly CyberArkSecureStoreCCP customSubject = new CyberArkSecureStoreCCP(CreateHttpClientHandler(), CreateHttpClient());

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
                Identifier = "CyberArkCCP",
                IsReadOnly = true,
            };

            var actual = subject.GetStoreInfo();
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void ValidateContextAsyncSucceedsGivenValidJsonString()
        {
            subject.ValidateContextAsync(ContextIsValidTrue);
        }

        [Fact]
        public async void ValidateContextAsyncFailsGivenInvalidData()
        {
            var type = typeof(CyberArkCCPOptions);
            PropertyInfo[] properties = type.GetProperties();
            var obj = new CyberArkCCPOptions()
            {
                ApplicationId = "AppID",
                Folder = "Folder",
                Safe = "Safe",
                Thumbprint = "Thumbprint",
                URL = "https://localhost"
            };
            foreach (PropertyInfo propertyInfo in properties)
            {
                propertyInfo.SetValue(obj, null);
                var ex = await Assert.ThrowsAsync<SecureStoreException>(
                    () => subject.ValidateContextAsync(JsonConvert.SerializeObject(obj)));
            }
        }

        [Fact]
        public async void ValidateContextAsyncFailsGivenInvalidJsonString()
        {
            var ex = await Assert.ThrowsAsync<SecureStoreException>(
                () => subject.ValidateContextAsync(NotJsonString));
        }

        [Fact]
        public void GetConfigurationReturnsCorrectObject()
        {
            var expected = new List<ConfigurationEntry>
            {
                new ConfigurationValue(ConfigurationValueType.String)
                {
                    Key = "URL",
                    DisplayName = "CyberArk CCP URL",
                    IsMandatory = true,
                },
                new ConfigurationValue(ConfigurationValueType.String)
                {
                    Key = "Thumbprint",
                    DisplayName = "CyberArkCCP Certificate thumbprint",
                    IsMandatory = true,
                },
                new ConfigurationValue(ConfigurationValueType.String)
                {
                    Key = "ApplicationId",
                    DisplayName = "Application ID",
                    IsMandatory = true,
                },
                new ConfigurationValue(ConfigurationValueType.String)
                {
                    Key = "Safe",
                    DisplayName = "CyberArkCCP Safe",
                    IsMandatory = true,
                },
                new ConfigurationValue(ConfigurationValueType.String)
                {
                    Key = "Folder",
                    DisplayName = "CyberArkCCP Folder",
                    IsMandatory = false,
                },
            };

            var actual = subject.GetConfiguration();
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async void GetValueAsyncWithValidContextWithInvalidCert()
        {
            using (var certificate = CreateSelfSignedCertificateWithoutPrivateKey())
            {
                using (var store = new X509Store(StoreName.My, StoreLocation.CurrentUser))
                {
                    store.Open(OpenFlags.ReadWrite);
                    store.Add(certificate);

                    var context = JsonConvert.SerializeObject(
                    new CyberArkCCPOptions()
                    {
                        ApplicationId = "AppID",
                        Folder = "Folder",
                        Safe = "Safe",
                        Thumbprint = certificate.Thumbprint,
                        URL = "https://localhost"
                    });

                    CyberArkSecureStoreCCP subj = new CyberArkSecureStoreCCP(new HttpClientHandler(), CreateHttpClient());

                    var ex = await Assert.ThrowsAsync<SecureStoreException>(
                       () => subj.GetValueAsync(context, "Key"));

                    //Cleanup
                    store.Remove(certificate);
                    store.Close();
                }
            }
        }
        [Fact]
        public async void GetValueAsyncWithValidContextWithValidCert()
        {
            using (var cert = CreateSelfSignedCertificateWithPrivateKey())
            {
                using (var store = new X509Store(StoreName.My, StoreLocation.CurrentUser))
                {
                    store.Open(OpenFlags.ReadWrite);
                    store.Add(cert);

                    var context = JsonConvert.SerializeObject(
                    new CyberArkCCPOptions()
                    {
                        ApplicationId = "AppID",
                        Folder = "Folder",
                        Safe = "Safe",
                        Thumbprint = cert.Thumbprint,
                        URL = "https://localhost"
                    });

                    CyberArkSecureStoreCCP subj = new CyberArkSecureStoreCCP(new HttpClientHandler(), CreateHttpClient());
                    var actual = await subj.GetValueAsync(context, "Key");

                    var expected = "Password";
                    actual.Should().BeEquivalentTo(expected);

                    //Cleanup
                    store.Remove(cert);
                    store.Close();
                }
            }
        }

        [Fact]
        public async void GetValueAsyncWithValidContextWithoutCert()
        {
            var actual = await customSubject.GetValueAsync(ContextIsValidTrue, "Key");
            var expected = "Password";
            actual.Should().BeEquivalentTo(expected);
        }

        private static HttpClientHandler CreateHttpClientHandler()
        {
            var mock = new Mock<HttpClientHandler>();
            using (var cert = new X509Certificate2())
            {
                var certCollection = new X509Certificate2Collection() { cert };
                mock.Object.ClientCertificates.Add(certCollection[0]);
            }

            return mock.Object;
        }

        private static HttpClient CreateHttpClient()
        {
            var handler = new CustomMsgHandler();
            var mock = new Mock<HttpClient>(handler);
            return mock.Object;
        }

        static X509Certificate2 CreateSelfSignedCertificateWithoutPrivateKey()
        {
            var certificate = new X509Certificate2();
            using (var ecdsa = ECDsa.Create())
            { // generate asymmetric key pair
                using (var rsa = RSA.Create())
                {
                    var req = new CertificateRequest("cn=foobar", rsa, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
                    certificate = req.CreateSelfSigned(DateTimeOffset.Now, DateTimeOffset.Now.AddYears(5));
                }
            }
            return certificate;
        }
        static X509Certificate2 CreateSelfSignedCertificateWithPrivateKey()
        {
            var ecdsa = ECDsa.Create(); // generate asymmetric key pair
            var req = new CertificateRequest("cn=foobar", ecdsa, HashAlgorithmName.SHA256);
            var cert = req.CreateSelfSigned(DateTimeOffset.Now, DateTimeOffset.Now.AddYears(5));
            var filename = Path.GetTempFileName();

            // Create PFX (PKCS #12) with private key
            File.WriteAllBytes(filename, cert.Export(X509ContentType.Pfx, "P@55w0rd"));
            cert.Import(filename, "P@55w0rd", X509KeyStorageFlags.Exportable);

            ecdsa.Dispose();
            return cert;
        }
    }
    public class CustomMsgHandler : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            HttpContent content = new StringContent(
                JsonConvert.SerializeObject(
                    new CyberArkCCPPassword()
                    {
                        Content = "Password",
                        Folder = "Folder",
                        Safe = "Safe",
                        Name = "KeyName",
                        PasswordChangeInProcess = "No"
                    }));
            return Task.FromResult(new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = content });
        }
    }
}
