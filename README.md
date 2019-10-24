# Orchestrator-CredentialStorePlugins
 Credential Store Plugins are a set of examples of how to create Credential Store plugins to use with Orchestrator.

## Getting Started
 ### Prerequisites
  Visual Studio with .NET Framework 4.7.2
  
 ### Create your own Secure Store plugin
  1. In Visual Studio, create a new Class Library (.NET Standard) Project
  2. Replace the content of the .csproj file with the following:
```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net472</TargetFramework>
    <AppendTargetFrameworkToOutputPath>false</AppendTargetFrameworkToOutputPath>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include="UiPath.Core.Extensibility" Version="19.10" />
    (Other dependencies here)
  </ItemGroup>
</Project>
```
  3. Under the new project, create a new class and implement ISecureStore interface:
```csharp
namespace Your.NameSpace
{
    public class YourSecureStore : ISecureStore
    {
        // Your Implementation
    }
}
```
  4. When you finish, build the project to get `<YourSecureStore>.dll`.
 
 ## Secure Store Lifecycle
 Secure Store plugins allow 3rd party developers to have a custom implementation of storage for secrets and credentials in UiPath Orchestrator, by offering an implementation for the following interface:
 
 ```csharp
public interface ISecureStore
{
	SecureStoreInfo GetStoreInfo();

	// Configuration APIs
	void Initialize(Dictionary<string, string> hostSettings);

	IEnumerable<ConfigurationEntry> GetConfiguration();

	Task ValidateContextAsync(string context);

	// Robots credential APIs
	Task<string> GetValueAsync(string context, string key);
	
	Task<string> CreateValueAsync(string context, string key, string value);

	Task<string> UpdateValueAsync(string context, string key, string oldAugumentedKey, string value);

	// Assets credential APIs
	Task<Credential> GetCredentialsAsync(string context, string key);

	Task<string> CreateCredentialsAsync(string context, string key, Credential value);

	Task<string> UpdateCredentialsAsync(string context, string key, string oldAugumentedKey, Credential value);

	// deletion for both Asstes and Robots credentials
	Task RemoveValueAsync(string context, string key);
}
 ```
 ### Info 
 The Secure store is defined by 
 ```csharp
public class SecureStoreInfo
{
	public string Identifier { get; set; }

	public bool IsReadOnly { get; set; }
}
 ```
 
 `Identifier` is the name of the Secure store type in Orchestrator UI to define new Secure Stores instances
 
 `IsReadOnly` specifies if the current store type has the capability to create/update/delete new records, or all the records are immutable, already present in the store. 
 
 ### Initialization and Configuration
 Secure store plugins have 2 types of configuration
 1) The Host level configuration is specified in web.config by key-value pairs. The keys have the following format `Plugins.SecureStores.{Plugin_indentifier}.{Setting_name}`. During the start-up of Orchestrator, all host level settings for the current plug-in are injected by a call to `Initialize(Dictionary<string, string> hostSettings)`. The plugin has the option to validate the settings by throwing an exception on initialization, and if they are not valid, an error will be logged, and the plugin will not be available for the creation of new credential stores. Existing Robots and Assets using instances of that Secure Store type will fail to load protected values.
 
![Plugin Load Sequence](/docs/img/Pluggable.png)
 
 2) Secure Store Instance level configuration. Each secure store instance can specify a configuration relevant only for the current instance. The configuration is in JSON format with fields defined by `IEnumerable<ConfigurationEntry> GetConfiguration();` which will be used to dynamically create new Secure Store UI, for example, this is [the configuration](https://github.com/UiPath/Orchestrator-CredentialStorePlugins/blob/master/src/SecureStore.AzureKeyVault/AzureKeyVaultSecureStore.cs#L200) for the UI generated for a new instance of AzureKeyVault Secure Store.
 ![Azure Key Vault Config](/docs/img/SecureStoreConfig.PNG)
When a new Secure Store is defined, the configuration will be further validated by calling `Task ValidateContextAsync(string context)`. In the case of AzureKeyVault Secure Store, the validation will check if the basic operations for Create/Read/Update/Delete are supported. 
 
  ### Assets Credentials
  
Credential assets specific APIs are in the context of a `key`.  The key is the optional parameter external_name specified in the Asset creation/edit flow. If external_name is empty, then the key would be the asset name.

 In the case of a read-only store, the credential assets records are already present in the store, and they can be retrieved by correlating with the `key`.
 
 In the case of a read/write store, a new record in the secure store will be generated with the call to `CreateCredentialsAsync(string context, string key, Credential value)` where `value` is the username/password pair in the asset definition. The plugin can return a reference for the created record, to be used instead of the external_name key for subsequent operations on the asset credential READ / UPDATE / DELETE. If the returned key is null or empty, the external_name key will be used for subsequent operations.
 
 ![Assets CRUD workflow ](docs/img/Asset%20Diagram%20%5Bexternal%5D.png)
 
 ### Robots Credentials
 
 Robot specific APIs are in the context of a `key`.  The key is the optional parameter external_name specified in the robot creation/edit flow. If external_name is empty, then the key would be username if this is an Active Directory username format `{Domain}\{user}` or if not the key will be `{machineName}\{userName}`.
 In the case of a read-only store, the robot records are already present in the store, and they can be retrieved by correlating with the `key`.
 
 In the case of a read/write store, a new record in the secure store will be generated with the call to `CreateValueAsync(string context, string key, string value)` where value is the password for the username used for the robot. The plugin can return a reference for the created record, to be used instead of the external_name key for subsequent operations on the robot credential READ / UPDATE / DELETE. If the returned key is null or empty, the external_name key will be used for subsequent operations.
 

 ## Deployment
  1. Locate your Orchestrator installation
  2. Copy the `YourSecureStore.dll` file to the plugins folder
  3. Enabled plugin via updating [web.config](https://docs.uipath.com/orchestrator/v2019/docs/app-settings#section-password-vault) where
  `<add key="Plugins.SecureStores" value="YourSecureStore.dll"/>`
  4. Restart your Orchestrator instance.
  
  ### License
  Current samples are available under [UiPath Open Platform License Agreement (“OPLA”)](https://github.com/UiPath/Orchestrator-CredentialStorePlugins-Samples/blob/master/UiPath_Activity_License_Agreement.pdf)
