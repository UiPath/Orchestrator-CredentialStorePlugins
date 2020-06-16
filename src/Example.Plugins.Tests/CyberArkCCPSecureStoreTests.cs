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
using UiPath.Orchestrator.Extensions.SecureStores.CyberArkCCP.Services;

namespace UiPath.Samples.SecureStores.CyberArkCCPStore
{
    public class CyberArkCCPSecureStoreTests
    {
        private const string NotJsonString = "This is not a JSON string!";

        private readonly CyberArkSecureStoreCCP _simpleStore = new CyberArkSecureStoreCCP();

        [Fact]
        public void InitializeDoesNothing()
        {
            _simpleStore.Initialize(null);
        }

        [Fact]
        public void GetStoreInfoReturnsCorrectObject()
        {
            var expected = new SecureStoreInfo
            {
                Identifier = "CyberArkCCP",
                IsReadOnly = true,
            };

            var actual = _simpleStore.GetStoreInfo();
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void ValidateContextAsyncSucceedsGivenValidJsonString()
        {
            var validContext = JsonConvert.SerializeObject(
            new CyberArkCCPOptions()
            {
                ApplicationId = "AppID",
                Folder = "Folder",
                Safe = "Safe",
                ClientCertificateThumbprint = "Thumbprint",
                URL = "https://localhost"
            });
        _simpleStore.ValidateContextAsync(validContext);
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
                ClientCertificateThumbprint = "Thumbprint",
                URL = "https://localhost"
            };
            foreach (PropertyInfo propertyInfo in properties)
            {
                propertyInfo.SetValue(obj, null);
                var ex = await Assert.ThrowsAsync<SecureStoreException>(
                    () => _simpleStore.ValidateContextAsync(JsonConvert.SerializeObject(obj)));
            }
        }

        [Fact]
        public async void ValidateContextAsyncFailsGivenInvalidJsonString()
        {
            var ex = await Assert.ThrowsAsync<SecureStoreException>(
                () => _simpleStore.ValidateContextAsync(NotJsonString));
        }

        [Fact]
        public void GetConfigurationReturnsCorrectObject()
        {
            var expected = new List<ConfigurationEntry>
            {
                new ConfigurationValue(ConfigurationValueType.String)
                {
                    Key = "URL",
                    DisplayName = "CCP URL",
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
                new ConfigurationValue(ConfigurationValueType.String)
                {
                    Key = "ClientCertificateThumbprint",
                    DisplayName = "CyberArkCCP Client Certificate Thumbprint",
                    IsMandatory = false,
                },
                new ConfigurationValue(ConfigurationValueType.String)
                {
                    Key = "CertificateAuthorityThumbprint",
                    DisplayName ="Personal Store Certificate Authority Thumbprint",
                    IsMandatory = false,
                }
            };

            var actual = _simpleStore.GetConfiguration();
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
                        ClientCertificateThumbprint = certificate.Thumbprint,
                        URL = "https://localhost"
                    });

                    var credentialStore = CreateCyberArkCCPStoreMock();

                    var ex = await Assert.ThrowsAsync<SecureStoreException>(
                       () => credentialStore.GetValueAsync(context, "Key"));

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
                        ClientCertificateThumbprint = cert.Thumbprint,
                        URL = "https://localhost"
                    });

                    var credentialStore = CreateCyberArkCCPStoreMock();
                    var actual = await credentialStore.GetValueAsync(context, "Key");

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
            var contextWithoutCertificateThumbrint = JsonConvert.SerializeObject(
            new CyberArkCCPOptions()
            {
                ApplicationId = "AppID",
                Folder = "Folder",
                Safe = "Safe",
                URL = "https://localhost"
            });

            var actual = await CreateCyberArkCCPStoreMock().GetValueAsync(contextWithoutCertificateThumbrint, "Key");
            var expected = "Password";
            actual.Should().BeEquivalentTo(expected);
        }

        private static CyberArkSecureStoreCCP CreateCyberArkCCPStoreMock()
        {
            var mock = new Mock<HttpClient>(new CustomMsgHandler());
            var httpClientFactoryMock = new Mock<IHttpClientFactory>();
            httpClientFactoryMock
                .Setup(f => f.CreateWithCertificate(It.IsAny<X509Certificate2>(), It.IsAny<string>()))
                .Returns(mock.Object);

            return new CyberArkSecureStoreCCP(httpClientFactoryMock.Object, new X509CertificateManager());
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
