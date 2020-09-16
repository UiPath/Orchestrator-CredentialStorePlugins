# How to use the plugin:

## Clone and build the project: 
1. Install [git](https://git-scm.com/book/en/v2/Getting-Started-Installing-Git)
2. Install dotnet [core](https://dotnet.microsoft.com/download)
3. Clone repo: `git clone https://github.com/UiPath/Orchestrator-CredentialStorePlugins.git`
4. Change directory to `Orchestrator-CredentialStorePlugins\src\SecureStore.CyberArkCCP` project.
5. Run `dotnet build --configuration Release`
6. Copy the file `bin\Release\UiPath.Orchestrator.Extensions.SecureStores.CyberArkCCP.dll` to the orchestrator machine(s) in the plugins folder.

## Configure CyberArk Central Credential Provider

### Create Application

1. The configuration discussed onward assumes that Central Credential Provider is already installed and running. Official CyberArk docs are [here](https://docs.cyberark.com/Product-Doc/OnlineHelp/AAM-CP/Latest/en/Content/LandingPages/lp_ACSPinstallation.htm?tocpath=Installation%7CApplication%20Server%20Credential%20Provider%7C_____0).
2. To define the CyberArk Application ID:
* login to the PVWA (Password Vault Web Access) interface using an user that can manage applications
* Click on the `Applications` tab and then `Add Application`:
![Add CyberArk application](/docs/img/CyberArkAddApplication.png)
3.	Specify the following information: 
* In the Name edit box, specify the Orchestrator.
* In the Description, specify a short description of the application that will help you identify it. 
* In the Business owner section, specify contact information about the application’s Business owner. 
* In the lowest section, specify the Location of the application in the Vault hierarchy. If a Location is not selected, the application will be added in the same Location as the user who is creating this application.
4. Click `Add` to add the application
![Add CyberArk application details](/docs/img/CyberArkApplicationDetails.png)
5. Allowing extended authentication restrictions.  This enables you to specify an unlimited number of machines and Windows domain OS users for a single application.  Please check this box.
6. Under the application details click add, select certificate serial number and fill it in. This will allow authentication with the certificate.
![Add CyberArk application details](/docs/img/CyberArkApplicatioCertificate.png)

### Provisioning accounts

In the Password Safe, provision the privileged accounts that will be required by the application. You can do this in either of the following ways:
* Manually – Add accounts manually one at a time and specify all the account details.
* Automatically – Add multiple accounts automatically using the Password Upload feature.
	> **For this step, you require the Add accounts authorization in the Password Safe.**

Official CyberArk [documentation](https://docs.cyberark.com/Product-Doc/OnlineHelp/PAS/Latest/en/Content/NewUI/NewUI-Add-accounts-in-PVWA.htm)

#### UiPath Robot credentials
For the UiPath Robots to correctly identify the Privileged accounts and be able to retrieve the passwords, the accounts must be provisioned in the Password Safe using one of the following naming conventions for the ObjectName of the account:
* domainName-username – for domain users.
* machineName-username – for local users.

#### UiPath credential assets
For credentials assets, the asset name or asset external name must be the exact same name in CyberArk (custom name) as in the Orchestrator. If the external name is present it will be used, if not the name will be used to identify the CyberArk entity. 

### Setting permissions for application access

Add the Credential Provider and application users as members of the Password Safes where the application passwords are stored. This can either be done manually in the Safes tab, or by specifying the Safe names in the CSV file for adding multiple applications.

1. Add the Provider user as a Safe Member with the following authorizations:
* List accounts
* Retrieve accounts
* View Safe Members
> Note: When installing multiple Providers for this integration, it is recommended to create a group for them, and add the group to the Safe once with the above authorization.

![Add CyberArk safe member](/docs/img/CyberArkAddSafeMember.png)
2. Add the application (the APPID) as a Safe Member with the following authorizations:
* Retrieve accounts

![Add CyberArk safe member2](/docs/img/CyberArkAddSafeMember2.png)

3. If your environment is configured for dual control: 
 * In PIM-PSM environments (v7.2 and lower), if the Safe is configured to require confirmation from authorized users before passwords can be retrieved, give the Provider user and the application the following permission:
o	Access Safe without Confirmation
* In Privileged Account Security solutions (v8.0 and higher), when working with dual control, the Provider user can always access without confirmation, thus, it is not necessary to set this permission.
4. If the Safe is configured for object level access, make sure that both the Provider user and the application have access to the password(s) to retrieve.


## Certificate(s)
1. If you are using an official certificate authority (CA) signed certificate for the CyberArk CCP's web server `https` connections, you will only need another client certificate for UiPath Orchestrator's authentication to CyberArk CCP.
2. If you are using a self-signed certificate for CyberArk CCP's web server `https` connections you need to add the root CA to the Trusted Root store on the orchestrator machine(s) (or [public certificate](https://github.com/MicrosoftDocs/azure-docs/blob/master/articles/app-service/configure-ssl-certificate.md#upload-a-public-certificate) for the Azure App Service ). Additionally you need a client certificate for UiPath Orchestrator's authentication to CyberArk CCP.


## Configuring UiPath Orchestrator

1.	Copy the CyberArkCCP plugin assembly (dll) in the UiPath’s Orchestrator plugins folder.
2.	Navigate to UiPath Orchestrator's web.config file and open it with a text editor, such as Notepad++.
3.	Configure the parameter with the DLL file name you got from building the project:
```xml
<add key="Plugins.SecureStores" value="<CyberArkCCP Dll name>.dll"/>
```
4. Save the file.
5.	In the UiPath Orchestrator, go to the credential store page.
6.	Select the CyberArkCCP plugin and fill in the information as seen in the following image: 
![Add CCP store in Orchestrator](/docs/img/OrchestratorAddCCPStore.png)
