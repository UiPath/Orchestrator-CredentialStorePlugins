# Orchestrator-CredentialStorePlugins-Samples
 Credential Store Plugins as examples on how to create Credential Store plugins to use on the Orchestrator.

## Getting Started
 ### Prerequisites
  Visual Studio with .NET Framwork 4.7.2
  
 ### Create your own Secure Store plugin
  1. In Visual Studio, create a new Class Library (.NET Standard) Project
  2. Replace the content of the .csproj file with the following:
```
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
```
namespace Your.NameSpace
{
    public class YourSecureStore : ISecureStore
    {
        // Your Implementation
    }
}
```
  4. When you finish, build the project, there should be a <YourSecureStore>.dll file under ./bin/
 
 ### Deployment
  Locate your orchestrator installation, copy the .dll file to the plugins folder, and restart your orchestrator instance.
  
  ### License
  Current samples are available under [UiPath Open Platform License Agreement (“OPLA”)](https://github.com/UiPath/Orchestrator-CredentialStorePlugins-Samples/blob/master/UiPath_Activity_License_Agreement.pdf)
