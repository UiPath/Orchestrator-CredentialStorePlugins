using System;
using System.Collections.Generic;
using System.Linq;

namespace BeyondTrust.BeyondInsight.PasswordSafe.API.Client.V3
{
    public abstract class BaseContainer<T>
    {
        public int TotalCount { get; set; }
        public List<T> Data { get; set; }

        public override string ToString()
        {
            return $"Total Count: {TotalCount}{Environment.NewLine}";
        }
    }


    public sealed class SignAppInUserModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string EmailAddress { get; set; }

        public override string ToString()
        {
            return string.Format(
                  "User ID: {1}{0}"
                + "Username: {2}{0}"
                + "Name: {3}{0}"
                + "EmailAddress: {4}{0}"
                , Environment.NewLine
                , UserId, UserName, Name, EmailAddress
                );
        }
    }

    public sealed class VersionModel
    {
        public string Version { get; set; }

        public override string ToString()
        {
            return $"Version: {Version}{Environment.NewLine}";
        }
    }

    public sealed class RequestableManagedAccountContainer : BaseContainer<RequestableManagedAccountModel>
    {
    }

    public class RequestableManagedAccountModel
    {
        public int PlatformID { get; set; }
        public int SystemId { get; set; }
        public string SystemName { get; set; }
        public string DomainName { get; set; }
        public int AccountId { get; set; }
        public string AccountName { get; set; }
        public string AccountNameFull { get { return string.IsNullOrEmpty(DomainName) ? AccountName : string.Format(@"{0}\{1}", DomainName, AccountName); } }
        public string InstanceName { get; set; }
        public int DefaultReleaseDuration { get; set; }
        public int MaximumReleaseDuration { get; set; }
        public DateTime? LastChangeDate { get; set; }
        public DateTime? NextChangeDate { get; set; }
        public bool IsChanging { get; set; }
        public bool IsISAAccess { get; set; }
        public int? ApplicationID { get; set; }
        public string ApplicationDisplayName { get; set; }
        public string PreferredNodeID { get; set; }

        public override string ToString()
        {
            return string.Format(
                  "Platform ID: {1}{0}"
                + "System Name: {2}{0}"
                + "System ID: {3}{0}"
                + "InstanceName: {4}{0}"
                + "DomainName: {5}{0}"
                + "AccountName: {6}{0}"
                + "Account ID: {7}{0}"
                + "Application ID: {8}{0}"
                + "Application Display Name: {9}{0}"
                + "Preferred Node ID: {10}{0}"
                + "Default Release Duration: {11}{0}"
                + "Maximum Release Duration: {12}{0}"
                + "ISA Access: {13}{0}"
                , Environment.NewLine
                , PlatformID, SystemName, SystemId, InstanceName
                , DomainName, AccountName, AccountId
                , ApplicationID, ApplicationDisplayName
                , PreferredNodeID
                , DefaultReleaseDuration, MaximumReleaseDuration
                , IsISAAccess
                );
        }
    }

    public sealed class ManagedAccountModel
    {
        // v3.0
        public int ManagedAccountID { get; set; }
        public int ManagedSystemID { get; set; }
        public string DomainName { get; set; }
        public string AccountName { get; set; }
        public string AccountNameFull { get { return string.IsNullOrEmpty(DomainName) ? AccountName : string.Format(@"{0}\{1}", DomainName, AccountName); } }
        public string DistinguishedName { get; set; }

        public string Description { get; set; }
        public bool ApiEnabled { get; set; }
        public int PasswordRuleID { get; set; }
        public string ReleaseNotificationEmail { get; set; }

        public bool ChangeServicesFlag { get; set; }
        public bool ChangeTasksFlag { get; set; }
        public bool RestartServicesFlag { get; set; }

        public int ReleaseDuration { get; set; }
        public int MaxReleaseDuration { get; set; }
        public int ISAReleaseDuration { get; set; }

        public bool AutoManagementFlag { get; set; }
        public bool CheckPasswordFlag { get; set; }
        public bool ChangePasswordAfterAnyReleaseFlag { get; set; }
        public bool ResetPasswordOnMismatchFlag { get; set; }

        public string ChangeFrequencyType { get; set; }
        public int ChangeFrequencyDays { get; set; }
        public DateTime? NextChangeDate { get; set; }
        public string ChangeTime { get; set; }
        public DateTime? LastChangeDate { get; set; }

        public bool IsSubscribedAccount { get; set; }
        public int? ParentAccountID { get; set; }

        // v3.1
        public bool UseOwnCredentials { get; set; }

        // v3.2
        public bool ChangeIISAppPoolFlag { get; set; }
        public bool RestartIISAppPoolFlag { get; set; }

        public override string ToString()
        {
            return string.Format(
                  "Managed Account ID: {1}{0}"
                + "Managed System ID: {2}{0}"
                + "DomainName: {3}{0}"
                + "AccountName: {4}{0}"
                + "DistinguishedName: {5}{0}"
                + "Description: {6}{0}"
                + "ApiEnabled: {7}{0}"
                + "PasswordRuleID: {8}{0}"
                + "ReleaseNotificationEmail: {9}{0}"
                + "ReleaseDuration: {10}{0}"
                + "MaxReleaseDuration: {11}{0}"
                + "ISAReleaseDuration: {12}{0}"
                + "ChangeServicesFlag: {13}{0}"
                + "RestartServicesFlag: {14}{0}"
                + "AutoManagementFlag: {15}{0}"
                + "CheckPasswordFlag: {16}{0}"
                + "ResetPasswordOnMismatchFlag: {17}{0}"
                + "ChangePasswordAfterAnyReleaseFlag: {18}{0}"
                + "ChangeFrequencyType: {19}{0}"
                + "ChangeFrequencyDays: {20}{0}"
                + "LastChangeDate: {21}{0}"
                + "NextChangeDate: {22}{0}"
                + "ChangeTime: {23}{0}"
                + "IsSubscribedAccount: {24}{0}"
                + "ParentAccountID: {25}{0}"
                + "ChangeTasksFlag: {26}{0}"
                + "UseOwnCredentials: {27}{0}"
                + "ChangeIISAppPoolFlag: {28}{0}"
                + "RestartIISAppPoolFlag: {29}{0}"
                , Environment.NewLine
                , ManagedAccountID, ManagedSystemID
                , DomainName, AccountName, DistinguishedName
                , Description, ApiEnabled, PasswordRuleID, ReleaseNotificationEmail
                , ReleaseDuration, MaxReleaseDuration, ISAReleaseDuration
                , ChangeServicesFlag, RestartServicesFlag
                , AutoManagementFlag, CheckPasswordFlag, ResetPasswordOnMismatchFlag, ChangePasswordAfterAnyReleaseFlag
                , ChangeFrequencyType, ChangeFrequencyDays, LastChangeDate, NextChangeDate, ChangeTime
                , IsSubscribedAccount, ParentAccountID, ChangeTasksFlag
                , UseOwnCredentials, ChangeIISAppPoolFlag, RestartIISAppPoolFlag
                );
        }
    }

    public sealed class ManagedAccountPostPutModel
    {
        // v3.0
        public int ManagedSystemID { get; set; }
        public string DomainName { get; set; }
        public string AccountName { get; set; }
        public string Password { get; set; }

        public string Description { get; set; }
        public bool ApiEnabled { get; set; }
        public int PasswordRuleID { get; set; }
        public string ReleaseNotificationEmail { get; set; }

        public bool ChangeServicesFlag { get; set; }
        public bool ChangeTasksFlag { get; set; }
        public bool RestartServicesFlag { get; set; }

        public int ReleaseDuration { get; set; }
        public int MaxReleaseDuration { get; set; }
        public int ISAReleaseDuration { get; set; }

        public bool AutoManagementFlag { get; set; }
        public bool CheckPasswordFlag { get; set; }
        public bool ChangePasswordAfterAnyReleaseFlag { get; set; }
        public bool ResetPasswordOnMismatchFlag { get; set; }

        public string ChangeFrequencyType { get; set; }
        public int ChangeFrequencyDays { get; set; }
        public DateTime? NextChangeDate { get; set; }
        public string ChangeTime { get; set; }

        // v3.1
        public bool UseOwnCredentials { get; set; }

        // v3.2
        public bool ChangeIISAppPoolFlag { get; set; }
        public bool RestartIISAppPoolFlag { get; set; }

        public override string ToString()
        {
            return string.Format(
                 $"ManagedSystemID: {ManagedSystemID}{Environment.NewLine}"
                + "DomainName: {1}{0}"
                + "AccountName: {2}{0}"
                + "Password: {3}{0}"
                + "Description: {4}{0}"
                + "ApiEnabled: {5}{0}"
                + "PasswordRuleID: {6}{0}"
                + "ReleaseNotificationEmail: {7}{0}"
                + "ChangeServicesFlag: {8}{0}"
                + "RestartServicesFlag: {9}{0}"
                + "ReleaseDuration: {10}{0}"
                + "MaxReleaseDuration: {11}{0}"
                + "ISAReleaseDuration: {12}{0}"
                + "AutoManagementFlag: {13}{0}"
                + "CheckPasswordFlag: {14}{0}"
                + "ResetPasswordOnMismatchFlag: {15}{0}"
                + "ChangePasswordAfterAnyReleaseFlag: {16}{0}"
                + "ChangeFrequencyType: {17}{0}"
                + "ChangeFrequencyDays: {18}{0}"
                + "NextChangeDate: {19}{0}"
                + "ChangeTime: {20}{0}"
                + "ChangeTasksFlag: {21}{0}"
                + "UseOwnCredentials: {22}{0}"
                + "ChangeIISAppPoolFlag: {23}{0}"
                + "RestartIISAppPoolFlag: {24}{0}"
                , Environment.NewLine
                , DomainName, AccountName, Password
                , Description, ApiEnabled, PasswordRuleID, ReleaseNotificationEmail
                , ChangeServicesFlag, RestartServicesFlag
                , ReleaseDuration, MaxReleaseDuration, ISAReleaseDuration
                , AutoManagementFlag, CheckPasswordFlag, ResetPasswordOnMismatchFlag, ChangePasswordAfterAnyReleaseFlag
                , ChangeFrequencyType, ChangeFrequencyDays, NextChangeDate, ChangeTime, ChangeTasksFlag
                , UseOwnCredentials, ChangeIISAppPoolFlag, RestartIISAppPoolFlag
                );
        }
    }

    public sealed class ManagedSystemContainer : BaseContainer<ManagedSystemModel>
    {
    }

    public sealed class ManagedSystemModel
    {
        public int ManagedSystemID { get; set; }
        public int WorkgroupID { get; set; }
        public int EntityTypeID { get; set; }

        public int? AssetID { get; set; }
        public int? DatabaseID { get; set; }
        public int? DirectoryID { get; set; }
        public int? CloudID { get; set; }

        public string HostName { get; set; }
        public string IPAddress { get; set; }
        public string DnsName { get; set; }
        public string InstanceName { get; set; }
        public bool? IsDefaultInstance { get; set; }
        public string Template { get; set; }
        public string ForestName { get; set; }
        public bool? UseSSL { get; set; }
        public int AccountNameFormat { get; set; }

        public Guid? OracleInternetDirectoryID { get; set; }
        public string OracleInternetDirectoryServiceName { get; set; }

        public string SystemName { get; set; }
        public int PlatformID { get; set; }
        public string NetBiosName { get; set; }
        public int? Port { get; set; }
        public short Timeout { get; set; }

        public string Description { get; set; }
        public string ContactEmail { get; set; }

        public int PasswordRuleID { get; set; }
        public int? DSSKeyRuleID { get; set; }

        public int ReleaseDuration { get; set; }
        public int MaxReleaseDuration { get; set; }
        public int ISAReleaseDuration { get; set; }

        public bool AutoManagementFlag { get; set; }
        public int? FunctionalAccountID { get; set; }
        public string ElevationCommand { get; set; }
        public int? SshKeyEnforcementMode { get; set; }

        public bool CheckPasswordFlag { get; set; }
        public bool ResetPasswordOnMismatchFlag { get; set; }
        public bool ChangePasswordAfterAnyReleaseFlag { get; set; }
        public string ChangeFrequencyType { get; set; }
        public int ChangeFrequencyDays { get; set; }
        public string ChangeTime { get; set; }

        public override string ToString()
        {
            return 
                  $"Managed System ID: {ManagedSystemID}{Environment.NewLine}"
                + $"Workgroup ID: {WorkgroupID}{Environment.NewLine}"
                + $"Entity Type ID: {EntityTypeID}{Environment.NewLine}"
                + $"Asset ID: {AssetID}{Environment.NewLine}"
                + $"Database ID: {DatabaseID}{Environment.NewLine}"
                + $"Directory ID: {DirectoryID}{Environment.NewLine}"
                + $"Cloud ID: {CloudID}{Environment.NewLine}"
                + $"Host Name: {HostName}{Environment.NewLine}"
                + $"IPAddress: {IPAddress}{Environment.NewLine}"
                + $"DnsName: {DnsName}{Environment.NewLine}"
                + $"InstanceName: {InstanceName}{Environment.NewLine}"
                + $"IsDefaultInstance: {IsDefaultInstance}{Environment.NewLine}"
                + $"Template: {Template}{Environment.NewLine}"
                + $"ForestName: {ForestName}{Environment.NewLine}"
                + $"UseSSL: {UseSSL}{Environment.NewLine}"
                + $"AccountNameFormat: {AccountNameFormat}{Environment.NewLine}"
                + $"OracleInternetDirectoryID: {OracleInternetDirectoryID}{Environment.NewLine}"
                + $"OracleInternetDirectoryServiceName: {OracleInternetDirectoryServiceName}{Environment.NewLine}"
                + $"System Name: {SystemName}{Environment.NewLine}"
                + $"Platform ID: {PlatformID}{Environment.NewLine}"
                + $"NetBiosName: {NetBiosName}{Environment.NewLine}"
                + $"Port: {Port}{Environment.NewLine}"
                + $"Timeout: {Timeout}{Environment.NewLine}"
                + $"Description: {Description}{Environment.NewLine}"
                + $"ContactEmail: {ContactEmail}{Environment.NewLine}"
                + $"PasswordRuleID: {PasswordRuleID}{Environment.NewLine}"
                + $"DSSKeyRuleID: {DSSKeyRuleID}{Environment.NewLine}"
                + $"ReleaseDuration: {ReleaseDuration}{Environment.NewLine}"
                + $"MaxReleaseDuration: {MaxReleaseDuration}{Environment.NewLine}"
                + $"ISAReleaseDuration: {ISAReleaseDuration}{Environment.NewLine}"
                + $"AutoManagementFlag: {AutoManagementFlag}{Environment.NewLine}"
                + $"FunctionalAccountID: {FunctionalAccountID}{Environment.NewLine}"
                + $"ElevationCommand: {ElevationCommand}{Environment.NewLine}"
                + $"SshKeyEnforcementMode: {SshKeyEnforcementMode}{Environment.NewLine}"
                + $"CheckPasswordFlag: {CheckPasswordFlag}{Environment.NewLine}"
                + $"ResetPasswordOnMismatchFlag: {ResetPasswordOnMismatchFlag}{Environment.NewLine}"
                + $"ChangeFrequencyType: {ChangeFrequencyType}{Environment.NewLine}"
                + $"ChangeFrequencyDays: {ChangeFrequencyDays}{Environment.NewLine}"
                + $"ChangeTime: {ChangeTime}{Environment.NewLine}"
                ;
        }
    }

    public sealed class FunctionalAccountModel
    {
        public int FunctionalAccountID { get; set; }
        public int PlatformID { get; set; }
        public string DomainName { get; set; }
        public string AccountName { get; set; }
        //public string AccountNameFull { get { return string.IsNullOrEmpty(DomainName) ? AccountName : string.Format(@"{0}\{1}", DomainName, AccountName); } }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string ElevationCommand { get; set; }
        public int SystemReferenceCount { get; set; }

        public override string ToString()
        {
            return string.Format(
                  "Functional Account ID: {1}{0}"
                + "Platform ID: {2}{0}"
                + "DomainName: {3}{0}"
                + "AccountName: {4}{0}"
                + "DisplayName: {5}{0}"
                + "Description: {6}{0}"
                + "ElevationCommand: {7}{0}"
                + "System Reference Count: {8}{0}"
                , Environment.NewLine
                , FunctionalAccountID, PlatformID
                , DomainName, AccountName, DisplayName
                , Description, ElevationCommand, SystemReferenceCount
                );
        }
    }

    public sealed class FunctionalAccountPostModel
    {
        public int PlatformID { get; set; }
        public string DomainName { get; set; }
        public string AccountName { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string ElevationCommand { get; set; }

        public string Password { get; set; }
        public string PrivateKey { get; set; }

        public override string ToString()
        {
            return string.Format(
                  "Platform ID: {1}{0}"
                + "DomainName: {2}{0}"
                + "AccountName: {3}{0}"
                + "DisplayName: {4}{0}"
                + "Description: {5}{0}"
                + "ElevationCommand: {6}{0}"
                , Environment.NewLine
                , PlatformID
                , DomainName, AccountName, DisplayName
                , Description, ElevationCommand
                );
        }
    }

    public sealed class RequestPostModel
    {
        public string AccessType { get; set; }
        public int AccountId { get; set; }
        public int SystemId { get; set; }
        public int? ApplicationID { get; set; }
        public int DurationMinutes { get; set; }
        public string Reason { get; set; }
        public int? AccessPolicyScheduleID { get; set; }
        public string ConflictOption { get; set; }
        public int? TicketSystemID { get; set; }
        public string TicketNumber { get; set; }

        public override string ToString()
        {
            return string.Format(
                  "Access Type: {1}{0}"
                + "Account ID: {2}{0}"
                + "System ID: {3}{0}"
                + "Application ID: {4}{0}"
                + "Duration: {5}{0}"
                + "Reason: {6}{0}"
                + "AccessPolicyScheduleID: {7}{0}"
                + "ConflictOption: {8}{0}"
                + "TicketSystemID: {9}{0}"
                + "TicketNumber: {10}{0}"
                , Environment.NewLine
                , string.IsNullOrEmpty(AccessType) ? "(not given)" : AccessType
                , AccountId, SystemId, ApplicationID, DurationMinutes, Reason
                , !AccessPolicyScheduleID.HasValue ? "(not given)" : Convert.ToString(AccessPolicyScheduleID.Value)
                , string.IsNullOrEmpty(ConflictOption) ? "(not given)" : ConflictOption
                , TicketSystemID, TicketNumber
                );
        }
    }

    public sealed class RequestModel
    {
        public int RequestId { get; set; }
        public string AccessType { get; set; }
        public string Status { get; set; }
        public int SystemID { get; set; }
        public string SystemName { get; set; }
        public int AccountID { get; set; }
        public string AccountName { get; set; }
        public string DomainName { get; set; }
        public int? AliasID { get; set; }
        public string RequestReleaseDate { get; set; }
        public string ApprovedDate { get; set; }
        public string ExpiresDate { get; set; }

        public override string ToString()
        {
            return string.Format(
                  "Request ID: {1}{0}"
                + "Access Type: {2}{0}"
                + "Status: {3}{0}"
                + "System ID: {4}{0}"
                + "System Name: {5}{0}"
                + "Account ID: {6}{0}"
                + "Account Name: {7}{0}"
                + "Domain Name: {8}{0}"
                + "Alias ID: {9}{0}"
                + "Requested: {10}{0}"
                + "Approved:  {11}{0}"
                + "Expires:   {12}{0}"
                , Environment.NewLine
                , RequestId, AccessType, Status, SystemID, SystemName, AccountID, AccountName, DomainName
                , !AliasID.HasValue ? "(not set)" : Convert.ToString(AliasID.Value)
                , RequestReleaseDate, ApprovedDate, ExpiresDate
                );
        }
    }

    public sealed class RequestSetModel
    {
        public string RequestSetID { get; set; }
        public List<RequestModel> Requests { get; set; } = new List<RequestModel>();

        public override string ToString()
        {
            return string.Format(
                  "RequestSetID: {1}{0}"
                + "Requests count: {2}{0}"
                , Environment.NewLine
                , RequestSetID, Requests.Count
                );
        }
    }

    public sealed class RequestSetPostModel
    {
        public List<string> AccessTypes { get; set; } = new List<string>();
        public int AccountId { get; set; }
        public int SystemId { get; set; }
        public int DurationMinutes { get; set; }
        public string Reason { get; set; }
        //public int? AccessPolicyScheduleID { get; set; }
        public int? TicketSystemID { get; set; }
        public string TicketNumber { get; set; }

        public override string ToString()
        {
            return string.Format(
                  "Access Types: {1}{0}"
                + "Account ID: {2}{0}"
                + "System ID: {3}{0}"
                + "Duration: {4}{0}"
                + "Reason: {5}{0}"
                //+ "AccessPolicyScheduleID: {6}{0}"
                + "TicketSystemID: {6}{0}"
                + "TicketNumber: {7}{0}"
                , Environment.NewLine
                , string.Join(",", AccessTypes)
                , AccountId, SystemId, DurationMinutes, Reason
                //, !AccessPolicyScheduleID.HasValue ? "(not given)" : Convert.ToString(AccessPolicyScheduleID.Value)
                , TicketSystemID, TicketNumber
                );
        }
    }

    public sealed class ISARequestPostModel
    {
        public int AccountID { get; set; }
        public int SystemID { get; set; }
        public int? DurationMinutes { get; set; }
        public string Reason { get; set; }

        public override string ToString()
        {
            return string.Format(
                  "Account ID: {1}{0}"
                + "System ID: {2}{0}"
                + "Duration: {3}{0}"
                + "Reason: {4}{0}"
                , Environment.NewLine
                , AccountID, SystemID
                , !DurationMinutes.HasValue ? "(not given)" : Convert.ToString(DurationMinutes.Value)
                , Reason
                );
        }
    }

    public sealed class ISASessionPostModel
    {
        public string SessionType { get; set; }
        public int AccountID { get; set; }
        public int SystemID { get; set; }
        public int? DurationMinutes { get; set; }
        public string Reason { get; set; }

        public override string ToString()
        {
            return string.Format(
                  "Session Type: {1}{0}"
                + "Account ID: {2}{0}"
                + "System ID: {3}{0}"
                + "Duration: {4}{0}"
                + "Reason: {5}{0}"
                , Environment.NewLine
                , SessionType, AccountID, SystemID
                , !DurationMinutes.HasValue ? "(not given)" : Convert.ToString(DurationMinutes.Value)
                , Reason
                );
        }
    }

    public sealed class WorkgroupModel
    {
        public string OrganizationID { get; set; }
        public int ID { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return string.Format(
                  "Organization: {1}{0}"
                + "Workgroup ID: {2}{0}"
                + "Name: {3}{0}"
                , Environment.NewLine
                , OrganizationID, ID, Name
                );
        }
    }

    public abstract class RequestPutBaseModel
    {
        public string Reason;

        public override string ToString()
        {
            return string.Format(
                  "Reason: {1}{0}"
                , Environment.NewLine
                , Reason
                );
        }
    }

    public sealed class RequestCheckinModel : RequestPutBaseModel
    {
    }

    public sealed class RequestApproveModel : RequestPutBaseModel
    {
    }

    public sealed class RequestDenyModel : RequestPutBaseModel
    {
    }

    public sealed class RequestTerminateModel
    {
        public string Reason;

        public override string ToString()
        {
            return string.Format(
                  "Reason: {1}{0}"
                , Environment.NewLine
                , Reason
                );
        }
    }

    public sealed class ImportModel
    {
        public int WorkgroupID;
        public string ImportType;
        public string Filter;
        public string FileName;
        public byte[] FileContents;
        public string Base64FileContents;

        public override string ToString()
        {
            return string.Format(
                  "Workgroup ID: {1}{0}"
                + "ImportType: {2}{0}"
                + "Filter: {3}{0}"
                + "Filename: {4}{0}"
                , Environment.NewLine
                , WorkgroupID, ImportType, Filter, FileName
                );
        }
    }

    public sealed class CredentialsPutModel
    {
        public CredentialsPutModel()
        {
            UpdateSystem = true; // match API default
        }

        public string Password;

        public string PublicKey;
        public string PrivateKey;
        public string Passphrase;

        public bool UpdateSystem { get; set; }

        public override string ToString()
        {
            return string.Format(
                  "Password supplied: {1}{0}"
                , "PublicKey supplied: {2}{0}"
                , "PrivateKey supplied: {3}{0}"
                , "Passphrase supplied: {4}{0}"
                , "UpdateSystem: {5}{0}"
                , Environment.NewLine
                , !string.IsNullOrEmpty(Password)
                , !string.IsNullOrEmpty(PublicKey)
                , !string.IsNullOrEmpty(PrivateKey)
                , !string.IsNullOrEmpty(Passphrase)
                , UpdateSystem
                );
        }
    }

    public sealed class CredentialsTestResponseModel
    {
        public bool Success { get; set; }

        public override string ToString()
        {
            return Success
                ? "Success"
                : "Failed";
        }
    }

    public sealed class AliasModel : RequestableManagedAccountModel
    {
        public int AliasId { get; set; }
        public string AliasName { get; set; }
        public int AliasState { get; set; }
        public List<AliasMappingModel> MappedAccounts { get; set; } = new List<AliasMappingModel>();

        public override string ToString()
        {
            return string.Format(
                  "Alias Name: {1}{0}"
                + "Alias ID: {2}{0}"
                + "Alias State: {3}{0}"
                + "System Name: {4}{0}"
                + "System ID: {5}{0}"
                + "InstanceName: {6}{0}"
                + "DomainName: {7}{0}"
                + "AccountName: {8}{0}"
                + "Account ID: {9}{0}"
                + "Mapped Accounts ({10}):{0}"
                + "{11}{0}"
                , Environment.NewLine
                , AliasName, AliasId, AliasState
                , SystemName, SystemId
                , InstanceName, DomainName
                , AccountName, AccountId
                , MappedAccounts.Count
                , string.Join(Environment.NewLine, MappedAccounts.Select(ma => ma.ToString()))
                );
        }
    }

    public sealed class AliasMappingModel
    {
        public int AliasID { get; set; }
        public int ManagedSystemID { get; set; }
        public int ManagedAccountID { get; set; }
        public string Status { get; set; }

        public override string ToString()
        {
            return string.Format(
                  "\tSystem ID: {1}{0}"
                + "\tAccount ID: {2}{0}"
                + "\tStatus: {3}{0}"
                , Environment.NewLine
                , ManagedSystemID, ManagedAccountID, Status
                );
        }
    }

    public sealed class AliasCredentialModel
    {
        public int AliasId { get; set; }
        public string AliasName { get; set; }

        public int SystemId { get; set; }
        public string SystemName { get; set; }

        public string DomainName { get; set; }
        public int AccountId { get; set; }
        public string AccountName { get; set; }
        public string AccountNameFull { get { return string.IsNullOrEmpty(DomainName) ? AccountName : string.Format(@"{0}\{1}", DomainName, AccountName); } }

        public string Password { get; set; }

        public override string ToString()
        {
            return string.Format(
                  "Alias ID: {1}{0}"
                + "Alias Name: {2}{0}"
                + "System ID: {3}{0}"
                + "System Name: {4}{0}"
                + "Domain Name: {5}{0}"
                + "Account ID: {6}{0}"
                + "Account Name:  {7}{0}"
                , Environment.NewLine
                , AliasId, AliasName
                , SystemId, SystemName
                , DomainName, AccountId, AccountName
                );
        }
    }


    public sealed class AccessLevelModel
    {
        public int AccessLevelID { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return string.Format(
                  "AccessLevelID: {1}{0}"
                + "Name: {2}{0}"
                , Environment.NewLine
                , AccessLevelID, Name
                );
        }
    }

    public sealed class AccessLevelPostModel
    {
        public int AccessLevelID { get; set; }

        public override string ToString()
        {
            return string.Format(
                  "ID: {1}{0}"
                , Environment.NewLine
                , AccessLevelID
                );
        }
    }


    public sealed class AccessPolicyTestModel
    {
        public int AccountID { get; set; }
        public int SystemID { get; set; }
        public int DurationMinutes { get; set; }
    }

    public sealed class AccessPolicyModel
    {
        public int AccessPolicyID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<AccessPolicyScheduleModel> Schedules { get; set; } = new List<AccessPolicyScheduleModel>();

        public override string ToString()
        {
            return string.Format(
                  "AccessPolicyID: {1}{0}"
                + "Name: {2}{0}"
                + "Description: {3}{0}"
                + "Schedules count: {4}{0}"
                , Environment.NewLine
                , AccessPolicyID, Name, Description, Schedules.Count
                );
        }
    }

    public sealed class AccessPolicyScheduleModel
    {
        public int ScheduleID { get; set; }
        public List<AccessPolicyAccessTypesModel> AccessTypes { get; set; } = new List<AccessPolicyAccessTypesModel>();

        public override string ToString()
        {
            return string.Format(
                  "ScheduleID: {1}{0}"
                + "AccessTypes count: {2}{0}"
                , Environment.NewLine
                , ScheduleID, AccessTypes.Count
                );
        }
    }

    public sealed class AccessPolicyAccessTypesModel
    {
        public string AccessType { get; set; }
        public bool IsSession { get; set; }
        public bool RecordSession { get; set; }
        public int MinApprovers { get; set; }
        public int MaxConcurrent { get; set; }

        public override string ToString()
        {
            return string.Format(
                  "AccessType: {1}{0}"
                + "IsSession: {2}{0}"
                + "RecordSession: {3}{0}"
                + "MinApprovers: {4}{0}"
                + "MaxConcurrent: {5}{0}"
                , Environment.NewLine
                , AccessType, IsSession, RecordSession, MinApprovers, MaxConcurrent
                );
        }
    }

    public sealed class ApplicationModel
    {
        public int ApplicationID { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Version { get; set; }
        public string Command { get; set; }
        public string Parameters { get; set; }
        public string Publisher { get; set; }
        public string ApplicationType { get; set; }
        public int? FunctionalAccountID { get; set; }
        public int? ManagedSystemID { get; set; }
        public bool IsActive { get; set; }

        public override string ToString()
        {
            return string.Format(
                  "ApplicationID: {1}{0}"
                + "Name: {2}{0}"
                + "DisplayName: {3}{0}"
                + "Version: {4}{0}"
                + "Command: {5}{0}"
                + "Parameters: {6}{0}"
                + "Publisher: {7}{0}"
                + "ApplicationType: {8}{0}"
                + "FunctionalAccountID: {9}{0}"
                + "ManagedSystemID: {10}{0}"
                + "IsActive: {11}{0}"
                , Environment.NewLine
                , ApplicationID, Name, DisplayName, Version
                , Command, Parameters, Publisher, ApplicationType
                , FunctionalAccountID, ManagedSystemID
                , IsActive
                );
        }
    }

    public sealed class AttributeTypeModel
    {
        public int AttributeTypeID { get; set; }
        public string Name { get; set; }
        public bool IsReadOnly { get; set; }

        public override string ToString()
        {
            return string.Format(
                  "AttributeTypeID: {1}{0}"
                + "Name: {2}{0}"
                + "IsReadOnly: {3}{0}"
                , Environment.NewLine
                , AttributeTypeID, Name, IsReadOnly
                );
        }
    }

    public sealed class AttributeTypePostModel
    {
        public string Name { get; set; }

        public override string ToString()
        {
            return string.Format(
                  "Name: {1}{0}"
                , Environment.NewLine
                , Name
                );
        }
    }


    public sealed class AttributeModel
    {
        public int AttributeID { get; set; }
        public int AttributeTypeID { get; set; }
        public int? ParentAttributeID { get; set; }
        public string ShortName { get; set; }
        public string LongName { get; set; }
        public string Description { get; set; }
        public int? ValueInt { get; set; }
        public bool IsReadOnly { get; set; }
        public List<AttributeModel> ChildAttributes { get; set; } = new List<AttributeModel>();

        public override string ToString()
        {
            return string.Format(
                  "AttributeID: {1}{0}"
                + "AttributeTypeID: {2}{0}"
                + "ParentAttributeID: {3}{0}"
                + "ShortName: {4}{0}"
                + "LongName: {5}{0}"
                + "Description: {6}{0}"
                + "ValueInt: {7}{0}"
                + "IsReadOnly: {8}{0}"
                + "ChildAttributes count: {9}{0}"
                , Environment.NewLine
                , AttributeID, AttributeTypeID, ParentAttributeID, ShortName, LongName, Description
                , ValueInt, IsReadOnly, ChildAttributes.Count
                );
        }
    }

    public sealed class AttributePostModel
    {
        public int? ParentAttributeID { get; set; }
        public string ShortName { get; set; }
        public string LongName { get; set; }
        public string Description { get; set; }
        public int? ValueInt { get; set; }

        public override string ToString()
        {
            return string.Format(
                  "ParentAttributeID: {1}{0}"
                + "ShortName: {2}{0}"
                + "LongName: {3}{0}"
                + "Description: {4}{0}"
                + "ValueInt: {5}{0}"
                , Environment.NewLine
                , ParentAttributeID, ShortName, LongName, Description, ValueInt
                );
        }
    }

    public sealed class AssetContainer : BaseContainer<AssetModel>
    {
    }

    public sealed class AssetModel
    {
        public int WorkgroupID { get; set; }
        public int AssetID { get; set; }
        public string AssetName { get; set; }
        public string DnsName { get; set; }
        public string DomainName { get; set; }
        public string IPAddress { get; set; }
        public string MacAddress { get; set; }
        public string AssetType { get; set; }
        public string OperatingSystem { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime LastUpdateDate { get; set; }

        public override string ToString()
        {
            return string.Format(
                  "ID: {1}{0}"
                + "WorkgroupID: {2}{0}"
                + "Name: {3}{0}"
                + "DnsName: {4}{0}"
                + "DomainName: {5}{0}"
                + "IPAddress: {6}{0}"
                + "MacAddress: {7}{0}"
                + "Type: {8}{0}"
                + "OperatingSystem: {9}{0}"
                + "CreateDate: {10}{0}"
                + "LastUpdateDate: {11}{0}"
                , Environment.NewLine
                , AssetID, WorkgroupID
                , AssetName, DnsName, DomainName
                , IPAddress, MacAddress, AssetType, OperatingSystem, CreateDate, LastUpdateDate
                );
        }
    }

    public sealed class AssetPostModel
    {
        public string AssetName { get; set; }
        public string DnsName { get; set; }
        public string DomainName { get; set; }
        public string IPAddress { get; set; }
        public string MacAddress { get; set; }
        public string AssetType { get; set; }
        public string OperatingSystem { get; set; }

        public override string ToString()
        {
            return string.Format(
                  "Name: {1}{0}"
                + "DnsName: {2}{0}"
                + "DomainName: {3}{0}"
                + "IPAddress: {4}{0}"
                + "MacAddress: {5}{0}"
                + "Type: {6}{0}"
                + "OperatingSystem: {7}{0}"
                , Environment.NewLine
                , AssetName, DnsName, DomainName
                , IPAddress, MacAddress, AssetType, OperatingSystem
                );
        }
    }

    public sealed class AssetPutModel
    {
        public int WorkgroupID { get; set; }
        public string AssetName { get; set; }
        public string DnsName { get; set; }
        public string DomainName { get; set; }
        public string IPAddress { get; set; }
        public string MacAddress { get; set; }
        public string AssetType { get; set; }
        public string OperatingSystem { get; set; }

        public override string ToString()
        {
            return string.Format(
                  "WorkgroupID: {1}{0}"
                + "Name: {2}{0}"
                + "DnsName: {3}{0}"
                + "DomainName: {4}{0}"
                + "IPAddress: {5}{0}"
                + "MacAddress: {6}{0}"
                + "Type: {7}{0}"
                + "OperatingSystem: {8}{0}"
                , Environment.NewLine
                , WorkgroupID
                , AssetName, DnsName, DomainName
                , IPAddress, MacAddress, AssetType, OperatingSystem
                );
        }
    }

    public sealed class DatabaseModel
    {
        public int DatabaseID { get; set; }
        public int AssetID { get; set; }
        public int PlatformID { get; set; }
        public string InstanceName { get; set; }
        public bool IsDefaultInstance { get; set; }
        public int Port { get; set; }
        public string Version { get; set; }
        public string Template { get; set; }

        public override string ToString()
        {
            return
                  $"DatabaseID: {DatabaseID}{Environment.NewLine}"
                + $"AssetID: {AssetID}{Environment.NewLine}"
                + $"PlatformID: {PlatformID}{Environment.NewLine}"
                + $"InstanceName: {InstanceName}{Environment.NewLine}"
                + $"IsDefaultInstance: {IsDefaultInstance}{Environment.NewLine}"
                + $"Port: {Port}{Environment.NewLine}"
                + $"Version: {Version}{Environment.NewLine}"
                + $"Template: {Template}{Environment.NewLine}"
                ;
        }
    }

    public sealed class DatabasePostModel
    {
        public int PlatformID { get; set; }
        public string InstanceName { get; set; }
        public bool IsDefaultInstance { get; set; }
        public int Port { get; set; }
        public string Version { get; set; }
        public string Template { get; set; }

        public override string ToString()
        {
            return
                  $"PlatformID: {PlatformID}{Environment.NewLine}"
                + $"InstanceName: {InstanceName}{Environment.NewLine}"
                + $"IsDefaultInstance: {IsDefaultInstance}{Environment.NewLine}"
                + $"Port: {Port}{Environment.NewLine}"
                + $"Version: {Version}{Environment.NewLine}"
                + $"Template: {Template}{Environment.NewLine}"
                ;
        }
    }


    public sealed class KeystrokeModel
    {
        public long KeystrokeID { get; set; }
        public int SessionID { get; set; }
        public long TimeMarker { get; set; }
        public byte Type { get; set; }
        public string Data { get; set; }

        public override string ToString()
        {
            return string.Format(
                  "KeystrokeID: {1}{0}"
                + "SessionID: {2}{0}"
                + "TimeMarker: {3}{0}"
                + "Type: {4}{0}"
                + "Data: {5}{0}"
                , Environment.NewLine
                , KeystrokeID, SessionID
                , TimeMarker, Type, Data
                );
        }
    }


    public sealed class PasswordRuleModel
    {
        public int PasswordRuleID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public int MinimumLength { get; set; }
        public int MaximumLength { get; set; }

        public char FirstCharacterRequirement { get; set; }
        public char LowercaseRequirement { get; set; }
        public char UppercaseRequirement { get; set; }
        public char NumericRequirement { get; set; }
        public char SymbolRequirement { get; set; }

        public char[] ValidLowercaseCharacters { get; set; }
        public char[] ValidUppercaseCharacters { get; set; }
        public char[] ValidSymbols { get; set; }

        public override string ToString()
        {
            return string.Format(
                  "PasswordRuleID: {1}{0}"
                + "Name: {2}{0}"
                + "Description: {3}{0}"
                + "Minimum Length: {4}{0}"
                + "Maximum Length: {5}{0}"
                + "First Character Requirement: {6}{0}"
                + "Lowercase Requirement: {7}{0}"
                + "Uppercase Requirement: {8}{0}"
                + "Numeric Requirement: {9}{0}"
                + "Symbol Requirement: {10}{0}"
                + "Valid Lowercase Characters ({11}): {12}{0}"
                + "Valid Uppercase Characters ({13}): {14}{0}"
                + "Valid Symbols ({15}): {16}{0}"
                , Environment.NewLine
                , PasswordRuleID, Name, Description
                , MinimumLength, MaximumLength
                , FirstCharacterRequirement, LowercaseRequirement, UppercaseRequirement, NumericRequirement, SymbolRequirement
                , ValidLowercaseCharacters.Length, string.Join("", ValidLowercaseCharacters)
                , ValidUppercaseCharacters.Length, string.Join("", ValidUppercaseCharacters)
                , ValidSymbols.Length, string.Join("", ValidSymbols)
                );
        }
    }


    public sealed class DSSKeyRuleModel
    {
        public int DSSKeyRuleID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public string KeyType { get; set; }
        public int KeySize { get; set; }

        public char EncryptionType { get; set; }
        public int? PasswordRuleID { get; set; }

        public override string ToString()
        {
            return string.Format(
                  "DSSKeyRuleID: {1}{0}"
                + "Name: {2}{0}"
                + "Description: {3}{0}"
                + "KeyType: {4}{0}"
                + "KeySize: {5}{0}"
                + "EncryptionType: {6}{0}"
                + "PasswordRuleID: {7}{0}"
                , Environment.NewLine
                , DSSKeyRuleID, Name, Description
                , KeyType, KeySize
                , EncryptionType, PasswordRuleID
                );
        }
    }


    public sealed class PermissionModel
    {
        public int PermissionID { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return string.Format(
                  "PermissionID: {1}{0}"
                + "Name: {2}{0}"
                , Environment.NewLine
                , PermissionID, Name
                );
        }
    }

    public sealed class PermissionAccessLevelModel
    {
        public int PermissionID { get; set; }
        public int AccessLevelID { get; set; }

        public override string ToString()
        {
            return string.Format(
                  "PermissionID: {1}{0}"
                + "AccesssLevelID: {2}{0}"
                , Environment.NewLine
                , PermissionID, AccessLevelID
                );
        }
    }


    public sealed class PlatformModel
    {
        public int PlatformID { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        
        public bool PortFlag { get; set; }
        public int? DefaultPort { get; set; }
        public string DefaultSessionType { get; set; }

        public bool DomainNameFlag { get; set; }
        public bool AutoManagementFlag { get; set; }
        public bool ManageableFlag { get; set; }
        public bool DSSFlag { get; set; }
        public bool SupportsElevationFlag { get; set; }
        public bool LoginAccountFlag { get; set; }

        public override string ToString()
        {
            return string.Format(
                  "PlatformID: {1}{0}"
                + "Name: {2}{0}"
                + "ShortName: {3}{0}"
                + "PortFlag: {4}{0}"
                + "DefaultPort: {5}{0}"
                + "DefaultSessionType: {6}{0}"
                + "DomainNameFlag: {7}{0}"
                + "AutoManagementFlag: {8}{0}"
                + "ManageableFlag: {9}{0}"
                + "DSSFlag: {10}{0}"
                + "SupportsElevationFlag: {11}{0}"
                + "LoginAccountFlag: {12}{0}"
                , Environment.NewLine
                , PlatformID, Name, ShortName
                , PortFlag, DefaultPort, DefaultSessionType
                , DomainNameFlag, AutoManagementFlag, ManageableFlag, DSSFlag, SupportsElevationFlag, LoginAccountFlag
                );
        }
    }


    public sealed class ReplayModel
    {
        public int tstamp { get; set; }
        public int end { get; set; }
        public int offset { get; set; }
        public int next { get; set; }
        public int speed { get; set; }
        public bool eof { get; set; }
        public int headless { get; set; }
        public int duration { get; set; }

        public override string ToString()
        {
            return string.Format(
                  "tstamp: {1}{0}"
                + "end: {2}{0}"
                + "offset: {3}{0}"
                + "next: {4}{0}"
                + "speed: {5}{0}"
                + "eof: {6}{0}"
                + "duration: {7}{0}"
                , Environment.NewLine
                , tstamp, end, offset, next, speed, eof, duration
                );
        }
    }

    public sealed class ReplayPutModel
    {
        public int speed { get; set; }
        public int offset { get; set; }
        public int next { get; set; }

        public override string ToString()
        {
            return string.Format(
                  "speed: {1}{0}"
                + "offset: {2}{0}"
                + "next: {3}{0}"
                , Environment.NewLine
                , speed, offset, next
                );
        }
    }

    public sealed class ReplayPostModel
    {
        public string id { get; set; }
        public string record_key { get; set; }
        public string protocol { get; set; }
        public bool headless { get; set; }

        public override string ToString()
        {
            return string.Format(
                  "id: {1}{0}"
                + "record_key: {2}{0}"
                + "protocol: {3}{0}"
                + "headless: {4}{0}"
                , Environment.NewLine
                , id, record_key, protocol, headless
                );
        }
    }

    public sealed class ReplayPostResponseModel
    {
        public string id { get; set; }
        public string ticket { get; set; }
        public string protocol { get; set; }

        public override string ToString()
        {
            return string.Format(
                  "id: {1}{0}"
                + "ticket: {2}{0}"
                + "protocol: {3}{0}"
                , Environment.NewLine
                , id, ticket, protocol
                );
        }
    }


    public sealed class RoleModel
    {
        public int RoleID { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return string.Format(
                  "RoleID: {1}{0}"
                + "Name: {2}{0}"
                , Environment.NewLine
                , RoleID, Name
                );
        }
    }

    public sealed class RolePostModel
    {
        public List<RoleModel> Roles { get; set; } = new List<RoleModel>();
        public int? AccessPolicyID { get; set; }

        public override string ToString()
        {
            return string.Format(
                  "Roles: {1}{0}"
                + "AccessPolicyID: {2}{0}"
                , Environment.NewLine
                , string.Join(",", Roles.Select(r => r.RoleID)), AccessPolicyID
                );
        }
    }


    public sealed class SessionModel
    {
        public int SessionID { get; set; }
        public int UserID { get; set; }
        public string NodeID { get; set; }
        public int Status { get; set; }
        public int ArchiveStatus { get; set; }
        public int Protocol { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int? Duration { get; set; }
        public string AssetName { get; set; }
        public int ManagedSystemID { get; set; }
        public int ManagedAccountID { get; set; }
        public string ManagedAccountName { get; set; }
        public string RecordKey { get; set; }
        public string Token { get; set; }

        public override string ToString()
        {
            return string.Format(
                  "SessionID: {1}{0}"
                + "UserID: {2}{0}"
                + "NodeID: {3}{0}"
                + "Status: {4}{0}"
                + "ArchiveStatus: {5}{0}"
                + "Protocol: {6}{0}"
                + "StartTime: {7}{0}"
                + "EndTime: {8}{0}"
                + "Duration: {9}{0}"
                + "AssetName: {10}{0}"
                + "ManagedSystemID: {11}{0}"
                + "ManagedAccountID: {12}{0}"
                + "ManagedAccountName: {13}{0}"
                + "RecordKey: {14}{0}"
                + "Token: {15}{0}"
                , Environment.NewLine
                , SessionID, UserID, NodeID, Status, ArchiveStatus, Protocol
                , StartTime, EndTime, Duration
                , AssetName, ManagedSystemID, ManagedAccountID, ManagedAccountName
                , RecordKey, Token
                );
        }
    }

    public sealed class SessionsPostModel
    {
        public string SessionType { get; set; }
        public string NodeID { get; set; }

        public override string ToString()
        {
            return string.Format(
                  "SessionType: {1}{0}"
                + "NodeID: {2}{0}"
                , Environment.NewLine
                , SessionType, NodeID
                );
        }
    }

    public sealed class SessionsPostResponseModel
    {
        public string ID { get; set; }
        public string Ticket { get; set; }
        public string Host { get; set; }
        public string Port { get; set; }
        public string TicketAtHost { get; set; }
        public string Link { get; set; }
        public string Command { get; set; }
        public int SessionID { get; set; }
        public string NodeID { get; set; }

        public override string ToString()
        {
            return string.Format(
                  "ID: {1}{0}"
                + "Ticket: {2}{0}"
                + "Host: {3}{0}"
                + "Port: {4}{0}"
                + "TicketAtHost: {5}{0}"
                + "Link: {6}{0}"
                + "Command: {7}{0}"
                + "SessionID: {8}{0}"
                + "NodeID: {9}{0}"
                , Environment.NewLine
                , ID, Ticket, Host, Port
                , TicketAtHost, Link, Command
                , SessionID, NodeID
                );
        }
    }

    public sealed class SessionsAdminModel
    {
        public string SessionType { get; set; }
        public string HostName { get; set; }
        public int? Port { get; set; }
        public string DomainName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Reason { get; set; }
        public string Resolution { get; set; }
        public bool RDPAdminSwitch { get; set; }
        public bool SmartSizing { get; set; }
        public string NodeID { get; set; }

        public override string ToString()
        {
            return string.Format(
                  "SessionType: {1}{0}"
                + "HostName: {2}{0}"
                + "Port: {3}{0}"
                + "DomainName: {4}{0}"
                + "UserName: {5}{0}"
                + "Reason: {6}{0}"
                + "Resolution: {7}{0}"
                + "RDPAdminSwitch: {8}{0}"
                + "SmartSizing: {9}{0}"
                + "NodeID: {10}{0}"
                , Environment.NewLine
                , SessionType, HostName, Port, DomainName, UserName
                , Reason, Resolution, RDPAdminSwitch, SmartSizing, NodeID
                );
        }
    }


    public sealed class NodeModel
    {
        public string NodeID { get; set; }
        public string HostName { get; set; }
        public string DisplayName { get; set; }
        public DateTime? LastHeartbeat { get; set; }
        public bool IsActive { get; set; }

        public override string ToString()
        {
            return string.Format(
                  "NodeID: {1}{0}"
                + "HostName: {2}{0}"
                + "DisplayName: {3}{0}"
                + "LastHeartbeat: {4}{0}"
                + "IsActive: {5}{0}"
                , Environment.NewLine
                , NodeID, HostName, DisplayName
                , LastHeartbeat, IsActive
                );
        }
    }


    public class SmartRuleModel
    {
        public int SmartRuleID { get; set; }
        public string OrganizationID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public int Status { get; set; }
        public DateTime? LastProcessedDate { get; set; }
        public bool IsReadOnly { get; set; }
        public string RuleType { get; set; }

        public override string ToString()
        {
            return 
                  $"SmartRuleID: {SmartRuleID}{Environment.NewLine}"
                + $"OrganizationID: {OrganizationID}{Environment.NewLine}"
                + $"Title: {Title}{Environment.NewLine}"
                + $"Description: {Description}{Environment.NewLine}"
                + $"Category: {Category}{Environment.NewLine}"
                + $"Status: {Status}{Environment.NewLine}"
                + $"LastProcessedDate: {LastProcessedDate}{Environment.NewLine}"
                + $"IsReadOnly: {IsReadOnly}{Environment.NewLine}"
                + $"RuleType: {RuleType}{Environment.NewLine}"
                ;
        }
    }

    public sealed class SmartRuleAccessModel
    {
        public int SmartRuleID { get; set; }
        public int AccessLevelID { get; set; }

        public override string ToString()
        {
            return string.Format(
                  "SmartRuleID: {1}{0}"
                + "AccessLevelID: {2}{0}"
                , Environment.NewLine
                , SmartRuleID, AccessLevelID
                );
        }
    }

    public sealed class SmartRuleFilterSingleAccountModel
    {
        public int AccountID { get; set; }
        public string Title { get; set; }

        public override string ToString()
        {
            return string.Format(
                  "AccountID: {1}{0}"
                + "Title: {2}{0}"
                , Environment.NewLine
                , AccountID, Title
                );
        }
    }

    public sealed class SmartRuleFilterAssetAttributeModel
    {
        public List<int> AttributeIDs { get; set; } = new List<int>();
        public string Title { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public bool ProcessImmediately { get; set; }

        public override string ToString()
        {
            return
                  $"AttributeIDs: {string.Join(",", AttributeIDs)}{Environment.NewLine}"
                + $"Title: {Title}{Environment.NewLine}"
                + $"Category: {Category}{Environment.NewLine}"
                + $"Description: {Description}{Environment.NewLine}"
                + $"ProcessImmediately: {ProcessImmediately}{Environment.NewLine}"
                ;
        }
    }

    /// <summary>
    /// Quick Rules are a specialized Smart Rule.
    /// </summary>
    public sealed class QuickRuleModel : SmartRuleModel
    {
    }

    public sealed class QuickRulePostModel
    {
        public List<int> AccountIDs { get; private set; } = new List<int>();
        public string Title { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }

        public override string ToString()
        {
            return string.Format(
                  "Account IDs ({1}): {2}"
                + "Title: {3}{0}"
                + "Category: {3}{0}"
                + "Description: {3}{0}"
                , Environment.NewLine
                , AccountIDs.Count, string.Join(",", AccountIDs)
                , Title, Category, Description
                );
        }
    }

    public sealed class TicketSystemModel
    {
        public int TicketSystemID { get; set; }
        public bool IsActive { get; set; }
        public string TicketSystemName { get; set; }
        public string Description { get; set; }
        public bool IsDefaultSystem { get; set; }

        public override string ToString()
        {
            return 
                  $"TicketSystemID: {TicketSystemID}{Environment.NewLine}"
                + $"IsActive: {IsActive}{Environment.NewLine}"
                + $"TicketSystemName: {TicketSystemName}{Environment.NewLine}"
                + $"Description: {Description}{Environment.NewLine}"
                + $"IsDefaultSystem: {IsDefaultSystem}{Environment.NewLine}"
                ;
        }
    }

    public sealed class UserGroupModel
    {
        public int GroupID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string DistinguishedName { get; set; }
        public string GroupType { get; set; }
        public string AccountAttribute { get; set; }
        public string MembershipAttribute { get; set; }

        public override string ToString()
        {
            return string.Format(
                  "GroupType: {1}{0}"
                + "GroupID: {2}{0}"
                + "Name: {3}{0}"
                + "DistinguishedName: {4}{0}"
                + "AccountAttribute: {5}{0}"
                + "MembershipAttribute: {6}{0}"
                , Environment.NewLine
                , GroupType, GroupID, Name
                , DistinguishedName
                , AccountAttribute, MembershipAttribute
                );
        }
    }

    /// <summary>
    /// Base User Group Post model
    /// </summary>
    public abstract class UserGroupBasePostModel
    {
        public abstract string GroupType { get; }

        public string GroupName { get; set; }
        public string Description { get; set; }

        public List<PermissionAccessLevelModel> Permissions { get; private set; } = new List<PermissionAccessLevelModel>();
        public List<SmartRuleAccessModel> SmartRuleAccess { get; private set; } = new List<SmartRuleAccessModel>();
        public List<int> ApplicationRegistrationIDs { get; private set; } = new List<int>();

        public override string ToString()
        {
            return string.Format(
                  "GroupType: {1}{0}"
                + "GroupName: {2}{0}"
                + "Description: {3}{0}"
                , Environment.NewLine
                , GroupType, GroupName, Description
                );
        }
    }

    /// <summary>
    /// BeyondInsight User Group
    /// </summary>
    public sealed class UserGroupBIModel : UserGroupBasePostModel
    {
        public override string GroupType { get { return "BeyondInsight"; } }
    }

    /// <summary>
    /// ActiveDirectory User Group
    /// </summary>
    public sealed class UserGroupADModel : UserGroupBasePostModel
    {
        public override string GroupType { get { return "ActiveDirectory"; } }

        public string ForestName { get; set; }
        public string DomainName { get; set; }

        public string BindUser { get; set; }
        public string BindPassword { get; set; }
        public bool UseSSL { get; set; }

        public override string ToString()
        {
            return string.Format(
                  "GroupType: {1}{0}"
                + "GroupName: {2}{0}"
                + "Description: {3}{0}"
                + "ForestName: {4}{0}"
                + "DomainName: {5}{0}"
                + "BindUser: {6}{0}"
                + "BindPassword supplied: {7}{0}"
                + "UseSSL: {8}{0}"
                , Environment.NewLine
                , GroupType, GroupName, Description
                , ForestName, DomainName
                , BindUser, !string.IsNullOrEmpty(BindPassword), UseSSL
                );
        }
    }

    /// <summary>
    /// LDAP User Group
    /// </summary>
    public sealed class UserGroupLDAPModel : UserGroupBasePostModel
    {
        public override string GroupType { get { return "LdapDirectory"; } }

        public string GroupDistinguishedName { get; set; }
        public string AccountAttribute { get; set; }
        public string MembershipAttribute { get; set; }

        public string HostName { get; set; }
        public int Port { get; set; }
        public string BindUser { get; set; }
        public string BindPassword { get; set; }
        public bool UseSSL { get; set; }

        public override string ToString()
        {
            return string.Format(
                  "GroupType: {1}{0}"
                + "GroupName: {2}{0}"
                + "Description: {3}{0}"
                + "GroupDistinguishedName: {4}{0}"
                + "AccountAttribute: {5}{0}"
                + "MembershipAttribute: {6}{0}"
                + "HostName: {7}{0}"
                + "Port: {8}{0}"
                + "BindUser: {9}{0}"
                + "BindPassword supplied: {10}{0}"
                + "UseSSL: {11}{0}"
                , Environment.NewLine
                , GroupType, GroupName, Description
                , GroupDistinguishedName, AccountAttribute, MembershipAttribute
                , HostName, Port
                , BindUser, !string.IsNullOrEmpty(BindPassword), UseSSL
                );
        }
    }


    public sealed class UserModel
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string DomainName { get; set; }
        public string DistinguishedName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public bool IsQuarantined { get; set; }

        public override string ToString()
        {
            return string.Format(
                  "UserID: {1}{0}"
                + "UserName: {2}{0}"
                + "DomainName: {3}{0}"
                + "DistinguishedName: {4}{0}"
                + "FirstName: {5}{0}"
                + "LastName: {6}{0}"
                + "EmailAddress: {7}{0}"
                + "IsQuarantined: {8}{0}"
                , Environment.NewLine
                , UserID, UserName, DomainName, DistinguishedName
                , FirstName, LastName, EmailAddress, IsQuarantined
                );
        }
    }

    public sealed class UserPostBIModel
    {
        public string UserType { get { return "BeyondInsight"; } }

        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }

        public override string ToString()
        {
            return string.Format(
                  $"UserType: {UserType}{Environment.NewLine}"
                + $"UserName: {UserName}{Environment.NewLine}"
                + $"FirstName: {FirstName}{Environment.NewLine}"
                + $"LastName: {LastName}{Environment.NewLine}"
                + $"EmailAddress: {EmailAddress}{Environment.NewLine}"
                );
        }
    }

    public sealed class UserPostADModel
    {
        public string UserType { get { return "ActiveDirectory"; } }

        public string UserName { get; set; }
        public string ForestName { get; set; }
        public string DomainName { get; set; }

        public string BindUser { get; set; }
        public string BindPassword { get; set; }
        public bool UseSSL { get; set; }

        public override string ToString()
        {
            return string.Format(
                  $"UserType: {UserType}{Environment.NewLine}"
                + $"UserName: {UserName}{Environment.NewLine}"
                + $"ForestName: {ForestName}{Environment.NewLine}"
                + $"DomainName: {DomainName}{Environment.NewLine}"
                + $"BindUser: {BindUser}{Environment.NewLine}"
                + $"BindPassword supplied: {!string.IsNullOrEmpty(BindPassword)}{Environment.NewLine}"
                + $"UseSSL: {UseSSL}{Environment.NewLine}"
                );
        }
    }

    public sealed class UserPostLDAPModel
    {
        public string UserType { get { return "LdapDirectory"; } }

        public string HostName { get; set; }
        public string DistinguishedName { get; set; }
        public string AccountNameAttribute { get; set; }

        public string BindUser { get; set; }
        public string BindPassword { get; set; }
        public int Port { get; set; }
        public bool UseSSL { get; set; }

        public override string ToString()
        {
            return string.Format(
                  $"UserType: {UserType}{Environment.NewLine}"
                + $"HostName: {HostName}{Environment.NewLine}"
                + $"DistinguishedName: {DistinguishedName}{Environment.NewLine}"
                + $"AccountNameAttribute: {AccountNameAttribute}{Environment.NewLine}"
                + $"BindUser: {BindUser}{Environment.NewLine}"
                + $"BindPassword supplied: {!string.IsNullOrEmpty(BindPassword)}{Environment.NewLine}"
                + $"Port: {Port}{Environment.NewLine}"
                + $"UseSSL: {UseSSL}{Environment.NewLine}"
                );
        }
    }

    public sealed class UserPostPutModel
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }

        public override string ToString()
        {
            return string.Format(
                  "UserName: {1}{0}"
                + "FirstName: {2}{0}"
                + "LastName: {3}{0}"
                + "EmailAddress: {4}{0}"
                , Environment.NewLine
                , UserName, FirstName, LastName, EmailAddress
                );
        }
    }

    public sealed class VulnerabilitiesModel
    {
        public string VulnerabilityID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Severity { get; set; }
        public string Solution { get; set; }
        public decimal BaseScore { get; set; }
        public string BaseVector { get; set; }
        public DateTime LastDiscoveryDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int? Port { get; set; }
        public string Protocol { get; set; }
        public bool IsExploitable { get; set; }
        public bool IsVulnerable { get; set; }
        public decimal? TemporalScore { get; set; }
        public string TemporalVector { get; set; }
        public List<VulnerabilityEventModel> Events { get; set; } = new List<VulnerabilityEventModel>();
        public List<VulnerabilityReferenceModel> References { get; set; } = new List<VulnerabilityReferenceModel>();

        public override string ToString()
        {
            return $"VulnerabilityID: {VulnerabilityID}{Environment.NewLine}"
                + $"Name: {Name}{Environment.NewLine}"
                + $"Description: {Description}{Environment.NewLine}"
                + $"Severity: {Severity}{Environment.NewLine}"
                + $"Solution: {Solution}{Environment.NewLine}"
                + $"BaseScore: {BaseScore}{Environment.NewLine}"
                + $"BaseVector: {BaseVector}{Environment.NewLine}"
                + $"LastDiscoveryDate: {LastDiscoveryDate}{Environment.NewLine}"
                + $"CreatedDate: {CreatedDate}{Environment.NewLine}"
                + $"UpdatedDate: {UpdatedDate}{Environment.NewLine}"
                + $"Port: {Port}{Environment.NewLine}"
                + $"Protocol: {Protocol}{Environment.NewLine}"
                + $"IsExploitable: {IsExploitable}{Environment.NewLine}"
                + $"IsVulnerable: {IsVulnerable}{Environment.NewLine}"
                + $"TemporalScore: {TemporalScore}{Environment.NewLine}"
                + $"TemporalVector: {TemporalVector}{Environment.NewLine}"
                + $"Events count: {Events.Count}{Environment.NewLine}"
                + $"References count: {References.Count}{Environment.NewLine}"
                ;
        }
    }

    public sealed class VulnerabilityEventModel
    {
        public string TestedValue { get; set; }
        public string FoundValue { get; set; }
        public string Context { get; set; }

        public override string ToString()
        {
            return $"TestedValue: {TestedValue}{Environment.NewLine}"
                + $"FoundValue: {FoundValue}{Environment.NewLine}"
                + $"Context: {Context}{Environment.NewLine}"
                ;
        }
    }

    public sealed class VulnerabilityReferenceModel
    {
        public string ReferenceID { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string URL { get; set; }

        public override string ToString()
        {
            return $"ReferenceID: {ReferenceID}{Environment.NewLine}"
                + $"Name: {Name}{Environment.NewLine}"
                + $"Type: {Type}{Environment.NewLine}"
                + $"URL: {URL}{Environment.NewLine}"
                ;
        }
    }

    public sealed class OperatingSystemModel
    {
        public int OperatingSystemID { get; set; }
        public string Name { get; set; }
        public override string ToString()
        {
            return $"OperatingSystemID: {OperatingSystemID}{Environment.NewLine}"
                + $"Name: {Name}{Environment.NewLine}"
                ;
        }
    }

    public sealed class EntityTypeModel
    {
        public int EntityTypeID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public override string ToString()
        {
            return $"EntityTypeID: {EntityTypeID}{Environment.NewLine}"
                + $"Name: {Name}{Environment.NewLine}"
                + $"Description: {Description}{Environment.NewLine}"
                ;
        }
    }

    public sealed class OracleInternetDirectoryModel
    {
        public Guid OracleInternetDirectoryID { get; set; }
        public Guid OrganizationID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public override string ToString()
        {
            return $"OracleInternetDirectoryID: {OracleInternetDirectoryID}{Environment.NewLine}"
                + $"OrganizationID: {OrganizationID}{Environment.NewLine}"
                + $"Name: {Name}{Environment.NewLine}"
                + $"Description: {Description}{Environment.NewLine}"
                ;
        }
    }

    public sealed class OracleInternetDirectoryTestResultModel
    {
        public bool Success { get; set; }
        public override string ToString()
        {
            return $"Success: {Success}{Environment.NewLine}";
        }
    }

    public sealed class OracleInternetDirectoryQuerySevicesResultModel
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public List<OracleInternetDirectoryServiceModel> Services { get; set; } = new List<OracleInternetDirectoryServiceModel>();
        public override string ToString()
        {
            return $"Success: {Success}{Environment.NewLine}"
                + $"Message: {Message}{Environment.NewLine}"
                + $"Services count: {Services.Count}{Environment.NewLine}"
                ;
        }
    }
    public sealed class OracleInternetDirectoryServiceModel
    {
        public string Name { get; set; }
        public override string ToString()
        {
            return $"Name: {Name}{Environment.NewLine}";
        }
    }

    public sealed class DirectoryModel
    {
        public int DirectoryID { get; set; }
        public int WorkgroupID { get; set; }
        public int PlatformID { get; set; }

        public string DomainName { get; set; }
        public string ForestName { get; set; }
        public string NetBiosName { get; set; }
        public bool UseSSL { get; set; }
        public int? Port { get; set; }
        public short Timeout { get; set; }

        public string Description { get; set; }
        public string ContactEmail { get; set; }

        public int PasswordRuleID { get; set; }

        public int ReleaseDuration { get; set; }
        public int MaxReleaseDuration { get; set; }
        public int ISAReleaseDuration { get; set; }

        public int AccountNameFormat { get; set; }

        public bool AutoManagementFlag { get; set; }
        public int? FunctionalAccountID { get; set; }

        public bool CheckPasswordFlag { get; set; }
        public bool ResetPasswordOnMismatchFlag { get; set; }
        public bool ChangePasswordAfterAnyReleaseFlag { get; set; }
        public string ChangeFrequencyType { get; set; }
        public int ChangeFrequencyDays { get; set; }
        public string ChangeTime { get; set; }

        public override string ToString()
        {
            return
                  $"DirectoryID: {DirectoryID}{Environment.NewLine}"
                + $"WorkgroupID: {WorkgroupID}{Environment.NewLine}"
                + $"PlatformID: {PlatformID}{Environment.NewLine}"
                + $"DomainName: {DomainName}{Environment.NewLine}"
                + $"ForestName: {ForestName}{Environment.NewLine}"
                + $"NetBiosName: {NetBiosName}{Environment.NewLine}"
                + $"UseSSL: {UseSSL}{Environment.NewLine}"
                + $"Port: {Port}{Environment.NewLine}"
                + $"Timeout: {Timeout}{Environment.NewLine}"
                + $"Description: {Description}{Environment.NewLine}"
                + $"ContactEmail: {ContactEmail}{Environment.NewLine}"
                + $"PasswordRuleID: {PasswordRuleID}{Environment.NewLine}"
                + $"ReleaseDuration: {ReleaseDuration}{Environment.NewLine}"
                + $"MaxReleaseDuration: {MaxReleaseDuration}{Environment.NewLine}"
                + $"ISAReleaseDuration: {ISAReleaseDuration}{Environment.NewLine}"
                + $"AccountNameFormat: {AccountNameFormat}{Environment.NewLine}"
                + $"AutoManagementFlag: {AutoManagementFlag}{Environment.NewLine}"
                + $"FunctionalAccountID: {FunctionalAccountID}{Environment.NewLine}"
                + $"CheckPasswordFlag: {CheckPasswordFlag}{Environment.NewLine}"
                + $"ResetPasswordOnMismatchFlag: {ResetPasswordOnMismatchFlag}{Environment.NewLine}"
                + $"ChangePasswordAfterAnyReleaseFlag: {ChangePasswordAfterAnyReleaseFlag}{Environment.NewLine}"
                + $"ChangeFrequencyType: {ChangeFrequencyType}{Environment.NewLine}"
                + $"ChangeFrequencyDays: {ChangeFrequencyDays}{Environment.NewLine}"
                + $"ChangeTime: {ChangeTime}{Environment.NewLine}"
                ;
        }
    }

    public sealed class DirectoryPostModel
    {
        public int PlatformID { get; set; }

        public string DomainName { get; set; }
        public string ForestName { get; set; }
        public string NetBiosName { get; set; }
        public bool UseSSL { get; set; }
        public int? Port { get; set; }
        public short Timeout { get; set; }

        public string Description { get; set; }
        public string ContactEmail { get; set; }

        public int PasswordRuleID { get; set; }

        public int ReleaseDuration { get; set; }
        public int MaxReleaseDuration { get; set; }
        public int ISAReleaseDuration { get; set; }

        public int AccountNameFormat { get; set; }

        public bool AutoManagementFlag { get; set; }
        public int? FunctionalAccountID { get; set; }

        public bool CheckPasswordFlag { get; set; }
        public bool ResetPasswordOnMismatchFlag { get; set; }
        public bool ChangePasswordAfterAnyReleaseFlag { get; set; }
        public string ChangeFrequencyType { get; set; }
        public int ChangeFrequencyDays { get; set; }
        public string ChangeTime { get; set; }

        public override string ToString()
        {
            return
                  $"PlatformID: {PlatformID}{Environment.NewLine}"
                + $"DomainName: {DomainName}{Environment.NewLine}"
                + $"ForestName: {ForestName}{Environment.NewLine}"
                + $"NetBiosName: {NetBiosName}{Environment.NewLine}"
                + $"UseSSL: {UseSSL}{Environment.NewLine}"
                + $"Port: {Port}{Environment.NewLine}"
                + $"Timeout: {Timeout}{Environment.NewLine}"
                + $"Description: {Description}{Environment.NewLine}"
                + $"ContactEmail: {ContactEmail}{Environment.NewLine}"
                + $"PasswordRuleID: {PasswordRuleID}{Environment.NewLine}"
                + $"ReleaseDuration: {ReleaseDuration}{Environment.NewLine}"
                + $"MaxReleaseDuration: {MaxReleaseDuration}{Environment.NewLine}"
                + $"ISAReleaseDuration: {ISAReleaseDuration}{Environment.NewLine}"
                + $"AccountNameFormat: {AccountNameFormat}{Environment.NewLine}"
                + $"AutoManagementFlag: {AutoManagementFlag}{Environment.NewLine}"
                + $"FunctionalAccountID: {FunctionalAccountID}{Environment.NewLine}"
                + $"CheckPasswordFlag: {CheckPasswordFlag}{Environment.NewLine}"
                + $"ResetPasswordOnMismatchFlag: {ResetPasswordOnMismatchFlag}{Environment.NewLine}"
                + $"ChangePasswordAfterAnyReleaseFlag: {ChangePasswordAfterAnyReleaseFlag}{Environment.NewLine}"
                + $"ChangeFrequencyType: {ChangeFrequencyType}{Environment.NewLine}"
                + $"ChangeFrequencyDays: {ChangeFrequencyDays}{Environment.NewLine}"
                + $"ChangeTime: {ChangeTime}{Environment.NewLine}"
                ;
        }
    }


    public sealed class AddressGroupModel
    {
        public int AddressGroupID { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return
                  $"AddressGroupID: {AddressGroupID}{Environment.NewLine}"
                + $"Name: {Name}{Environment.NewLine}"
                ;
        }
    }

    public sealed class AddressModel
    {
        public int AddressID { get; set; }
        public int AddressGroupID { get; set; }
        public bool Omit { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
        public DateTime LastUpdateDate { get; set; }

        public override string ToString()
        {
            return
                  $"AddressID: {AddressID}{Environment.NewLine}"
                + $"AddressGroupID: {AddressGroupID}{Environment.NewLine}"
                + $"Omit: {Omit}{Environment.NewLine}"
                + $"Type: {Type}{Environment.NewLine}"
                + $"Value: {Value}{Environment.NewLine}"
                + $"LastUpdateDate: {LastUpdateDate}{Environment.NewLine}"
                ;
        }
    }

}
