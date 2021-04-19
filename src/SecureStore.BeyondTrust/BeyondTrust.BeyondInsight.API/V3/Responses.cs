using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;

namespace BeyondTrust.BeyondInsight.PasswordSafe.API.Client.V3
{
    /// <summary>
    /// Base class for a Password Safe API Result.
    /// </summary>
    /// <typeparam name="T">The type of expected content result.</typeparam>
    public abstract class APIResult<T>
    {
        protected HttpResponseMessage _Response = null;
        private Lazy<T> _Value;

        protected APIResult(HttpResponseMessage r)
        {
            _Response = r;
            _Value = new Lazy<T>(InitializeValue);
        }

        /// <summary>
        /// Returns true if the API call was successful, otherwise false.
        /// </summary>
        public bool IsSuccess
        {
            get { return _Response.IsSuccessStatusCode; }
        }

        /// <summary>
        /// Returns the API status code.
        /// </summary>
        public HttpStatusCode StatusCode
        {
            get { return _Response.StatusCode; }
        }

        /// <summary>
        /// Returns the error message or code returned from a failed API call.  Use when <see cref="IsSuccess"/> is false.
        /// </summary>
        public string Message
        {
            get
            {
                string r = string.Empty;
                
                // defend against misuse, we only expect a message on (some) failure(s)
                if (!IsSuccess)
                    r = _Response.Content.ReadAsStringAsync().Result;
                
                return r;
            }
        }

        /// <summary>
        /// Returns the resulting value of a successful API call.  Use when <see cref="IsSuccess"/> is true.
        /// </summary>
        public T Value
        {
            get
            {
                return _Value.Value;
            }
        }

        /// <summary>
        /// Lazy initializer for the HttpResponseMessage content result.  Derived classes can transform the value by overriding this method.
        /// </summary>
        /// <returns>The HttpResponseMessage result, deserialized as <typeparamref name="T"/>.</returns>
        protected virtual T InitializeValue()
        {
            T value = default(T);

            // defend against misuse, we only want the value if it succeeds
            if (IsSuccess)
            {   // Note: reading it synchronously
                string r = _Response.Content.ReadAsStringAsync().Result;
                value = Utilities.DeserializeContent<T>(r);
            }

            return value;
        }

        /// <summary>
        /// Returns the result as error text.
        /// </summary>
        public string ToErrorText()
        {
            if (IsSuccess)
                return string.Empty;
            else
                return string.Format("{0}: {1}", StatusCode, Message);
        }

    }

    /// <summary>
    /// Result of APIs that return a stream.
    /// </summary>
    public class APIStreamResult : APIResult<System.IO.Stream>
    {
        internal APIStreamResult(HttpResponseMessage r) : base(r) { }

        protected override System.IO.Stream InitializeValue()
        {
            System.IO.Stream value = default(System.IO.Stream);

            // defend against misuse, we only want the value if it succeeds
            if (IsSuccess)
            {   // Note: reading it synchronously
                value = _Response.Content.ReadAsStreamAsync().Result;
            }

            return value;
        }
    }


    /// <summary>
    /// Result of Auth/SignAppin.
    /// </summary>
    public sealed class AuthenticationResult : APIResult<SignAppInUserModel>
    {
        internal AuthenticationResult(HttpResponseMessage r) : base(r) { }
    }

    /// <summary>
    /// Result of Auth/SignOut.
    /// </summary>
    public sealed class SignOutResult : APIResult<string>
    {
        internal SignOutResult(HttpResponseMessage r) : base(r) { }
    }

    /// <summary>
    /// Generic result of a Delete operation.
    /// </summary>
    public sealed class DeleteResult : APIResult<string>
    {
        internal DeleteResult(HttpResponseMessage r) : base(r) { }
    }

    /// <summary>
    /// Generic result of an API returning 204 (NoContent).
    /// </summary>
    public sealed class NoContentResult : APIResult<string>
    {
        internal NoContentResult(HttpResponseMessage r) : base(r) { }
    }

    /// <summary>
    /// Result of Get (Requestable) ManagedAccounts.
    /// </summary>
    public sealed class RequestableManagedAccountsResult : APIResult<List<RequestableManagedAccountModel>>
    {
        internal RequestableManagedAccountsResult(HttpResponseMessage r) : base(r) { }
    }

    /// <summary>
    /// Result of Get (Requestable) ManagedAccount.
    /// </summary>
    public sealed class RequestableManagedAccountResult : APIResult<RequestableManagedAccountModel>
    {
        internal RequestableManagedAccountResult(HttpResponseMessage r) : base(r) { }
    }

    /// <summary>
    /// Result of Get Requests.
    /// </summary>
    public sealed class RequestsResult : APIResult<List<RequestModel>>
    {
        internal RequestsResult(HttpResponseMessage r) : base(r) { }
    }

    /// <summary>
    /// Result of Post Requests.
    /// </summary>
    public sealed class RequestsPostResult : APIResult<int>
    {
        internal RequestsPostResult(HttpResponseMessage r) : base(r) { }
    }

    /// <summary>
    /// Result of Get RequestSets.
    /// </summary>
    public sealed class RequestSetsResult : APIResult<List<RequestSetModel>>
    {
        internal RequestSetsResult(HttpResponseMessage r) : base(r) { }
    }

    /// <summary>
    /// Result of Post RequestSets.
    /// </summary>
    public sealed class RequestSetResult : APIResult<RequestSetModel>
    {
        internal RequestSetResult(HttpResponseMessage r) : base(r) { }
    }

    /// <summary>
    /// Result of Get Credentials.
    /// </summary>
    public class CredentialsResult : APIResult<string>
    {
        internal CredentialsResult(HttpResponseMessage r) : base(r) { }
    }

    /// <summary>
    /// Result of Requests/Checkin.
    /// </summary>
    public sealed class RequestCheckinResult : APIResult<string>
    {
        internal RequestCheckinResult(HttpResponseMessage r) : base(r) { }
    }

    /// <summary>
    /// Result of Requests/Approve.
    /// </summary>
    public sealed class RequestApproveResult : APIResult<string>
    {
        internal RequestApproveResult(HttpResponseMessage r) : base(r) { }
    }

    /// <summary>
    /// Result of Requests/Deny.
    /// </summary>
    public sealed class RequestDenyResult : APIResult<string>
    {
        internal RequestDenyResult(HttpResponseMessage r) : base(r) { }
    }

    /// <summary>
    /// Result of Post ISARequests.
    /// </summary>
    public sealed class ISARequestsResult : CredentialsResult
    {
        internal ISARequestsResult(HttpResponseMessage r) : base(r) { }
    }

    /// <summary>
    /// Result of Post ISASessions.
    /// </summary>
    public sealed class ISASessionsResult : SessionsPostResult
    {
        internal ISASessionsResult(HttpResponseMessage r) : base(r) { }
    }

    /// <summary>
    /// Result of Get Workgroups.
    /// </summary>
    public sealed class WorkgroupsResult : APIResult<List<WorkgroupModel>>
    {
        internal WorkgroupsResult(HttpResponseMessage r) : base(r) { }
    }

    /// <summary>
    /// Result of Get Workgroup.
    /// </summary>
    public sealed class WorkgroupResult : APIResult<WorkgroupModel>
    {
        internal WorkgroupResult(HttpResponseMessage r) : base(r) { }
    }

    /// <summary>
    /// Result of Post Imports.
    /// </summary>
    public sealed class ImportsResult : APIResult<int>
    {
        internal ImportsResult(HttpResponseMessage r) : base(r) { }
    }

    /// <summary>
    /// Result of Put ManagedAccounts/{id}/Credentials.
    /// </summary>
    public sealed class CredentialsPutResult : APIResult<string>
    {
        internal CredentialsPutResult(HttpResponseMessage r) : base(r) { }
    }

    /// <summary>
    /// Result of POST ManagedAccounts/{accountId}/Credentials/Test.
    /// </summary>
    public sealed class CredentialsTestResult : APIResult<CredentialsTestResponseModel>
    {
        internal CredentialsTestResult(HttpResponseMessage r) : base(r) { }
    }

    /// <summary>
    /// Result of Get Aliases.
    /// </summary>
    public sealed class AliasesResult : APIResult<List<AliasModel>>
    {
        internal AliasesResult(HttpResponseMessage r) : base(r) { }
    }

    /// <summary>
    /// Result of Get Aliases/{name}.
    /// </summary>
    public sealed class AliasResult : APIResult<AliasModel>
    {
        internal AliasResult(HttpResponseMessage r) : base(r) { }
    }

    /// <summary>
    /// Result of Post Aliases/{id}/Requests.
    /// </summary>
    public sealed class RequestAliasResult : APIResult<int>
    {
        internal RequestAliasResult(HttpResponseMessage r) : base(r) { }
    }

    /// <summary>
    /// Result of Get Aliases/{id}/Credentials.
    /// </summary>
    public sealed class AliasCredentialsResult : APIResult<AliasCredentialModel>
    {
        internal AliasCredentialsResult(HttpResponseMessage r) : base(r) { }
    }

    /// <summary>
    /// Result of Get AccessLevels.
    /// </summary>
    public sealed class AccessLevelsResult : APIResult<List<AccessLevelModel>>
    {
        internal AccessLevelsResult(HttpResponseMessage r) : base(r) { }
    }

    /// <summary>
    /// Result of Get AccessLevel.
    /// </summary>
    public sealed class AccessLevelResult : APIResult<AccessLevelModel>
    {
        internal AccessLevelResult(HttpResponseMessage r) : base(r) { }
    }

    /// <summary>
    /// Result of Get AccessPolicies.
    /// </summary>
    public sealed class AccessPoliciesGetResult : APIResult<List<AccessPolicyModel>>
    {
        internal AccessPoliciesGetResult(HttpResponseMessage r) : base(r) { }
    }

    /// <summary>
    /// Result of Post AccessPolicies/Test.
    /// </summary>
    public sealed class AccessPoliciesTestResult : APIResult<List<AccessPolicyModel>>
    {
        internal AccessPoliciesTestResult(HttpResponseMessage r) : base(r) { }
    }

    /// <summary>
    /// Result of GET Applications.
    /// </summary>
    public sealed class ApplicationsResult : APIResult<List<ApplicationModel>>
    {
        internal ApplicationsResult(HttpResponseMessage r) : base(r) { }
    }

    /// <summary>
    /// Result of GET Applications/{id}.
    /// </summary>
    public sealed class ApplicationResult : APIResult<ApplicationModel>
    {
        internal ApplicationResult(HttpResponseMessage r) : base(r) { }
    }


    /// <summary>
    /// Result of Get AttributeTypes.
    /// </summary>
    public sealed class AttributeTypesResult : APIResult<List<AttributeTypeModel>>
    {
        internal AttributeTypesResult(HttpResponseMessage r) : base(r) { }
    }

    /// <summary>
    /// Result of Get AttributeType.
    /// </summary>
    public sealed class AttributeTypeResult : APIResult<AttributeTypeModel>
    {
        internal AttributeTypeResult(HttpResponseMessage r) : base(r) { }
    }

    /// <summary>
    /// Result of Get Attributes.
    /// </summary>
    public sealed class AttributesResult : APIResult<List<AttributeModel>>
    {
        internal AttributesResult(HttpResponseMessage r) : base(r) { }
    }

    /// <summary>
    /// Result of Get Attribute.
    /// </summary>
    public sealed class AttributeResult : APIResult<AttributeModel>
    {
        internal AttributeResult(HttpResponseMessage r) : base(r) { }
    }

    /// <summary>
    /// Result of Get Assets (paged).
    /// </summary>
    public sealed class AssetsPagedResult : APIResult<AssetContainer>
    {
        internal AssetsPagedResult(HttpResponseMessage r) : base(r) { }
    }

    /// <summary>
    /// Result of Get Assets (non-paged).
    /// </summary>
    public sealed class AssetsResult : APIResult<List<AssetModel>>
    {
        internal AssetsResult(HttpResponseMessage r) : base(r) { }
    }

    /// <summary>
    /// Result of Get Asset.
    /// </summary>
    public sealed class AssetResult : APIResult<AssetModel>
    {
        internal AssetResult(HttpResponseMessage r) : base(r) { }
    }

    /// <summary>
    /// Result of Get Databases.
    /// </summary>
    public sealed class DatabasesResult : APIResult<List<DatabaseModel>>
    {
        internal DatabasesResult(HttpResponseMessage r) : base(r) { }
    }

    /// <summary>
    /// Result of Get Database.
    /// </summary>
    public sealed class DatabaseResult : APIResult<DatabaseModel>
    {
        internal DatabaseResult(HttpResponseMessage r) : base(r) { }
    }

    /// <summary>
    /// Result of Get Keystrokes.
    /// </summary>
    public sealed class KeystrokesResult : APIResult<List<KeystrokeModel>>
    {
        internal KeystrokesResult(HttpResponseMessage r) : base(r) { }
    }

    /// <summary>
    /// Result of Get Keystroke.
    /// </summary>
    public sealed class KeystrokeResult : APIResult<KeystrokeModel>
    {
        internal KeystrokeResult(HttpResponseMessage r) : base(r) { }
    }

    /// <summary>
    /// Result of Get ManagedAccounts.
    /// </summary>
    public sealed class ManagedAccountsResult : APIResult<List<ManagedAccountModel>>
    {
        internal ManagedAccountsResult(HttpResponseMessage r) : base(r) { }
    }

    /// <summary>
    /// Result of Get ManagedAccount.
    /// </summary>
    public sealed class ManagedAccountResult : APIResult<ManagedAccountModel>
    {
        internal ManagedAccountResult(HttpResponseMessage r) : base(r) { }
    }

    /// <summary>
    /// Result of Get ManagedSystems (paged).
    /// </summary>
    public sealed class ManagedSystemsPagedResult : APIResult<ManagedSystemContainer>
    {
        internal ManagedSystemsPagedResult(HttpResponseMessage r) : base(r) { }
    }

    /// <summary>
    /// Result of Get ManagedSystems (non-paged).
    /// </summary>
    public sealed class ManagedSystemsResult : APIResult<List<ManagedSystemModel>>
    {
        internal ManagedSystemsResult(HttpResponseMessage r) : base(r) { }
    }

    /// <summary>
    /// Result of Get ManagedSystem.
    /// </summary>
    public sealed class ManagedSystemResult : APIResult<ManagedSystemModel>
    {
        internal ManagedSystemResult(HttpResponseMessage r) : base(r) { }
    }

    /// <summary>
    /// Result of Get FunctionalAccounts.
    /// </summary>
    public sealed class FunctionalAccountsResult : APIResult<List<FunctionalAccountModel>>
    {
        internal FunctionalAccountsResult(HttpResponseMessage r) : base(r) { }
    }

    /// <summary>
    /// Result of Get FunctionalAccount.
    /// </summary>
    public sealed class FunctionalAccountResult : APIResult<FunctionalAccountModel>
    {
        internal FunctionalAccountResult(HttpResponseMessage r) : base(r) { }
    }

    /// <summary>
    /// Result of Get PasswordRules.
    /// </summary>
    public sealed class PasswordRulesResult : APIResult<List<PasswordRuleModel>>
    {
        internal PasswordRulesResult(HttpResponseMessage r) : base(r) { }
    }

    /// <summary>
    /// Result of Get PasswordRule.
    /// </summary>
    public sealed class PasswordRuleResult : APIResult<PasswordRuleModel>
    {
        internal PasswordRuleResult(HttpResponseMessage r) : base(r) { }
    }

    /// <summary>
    /// Result of Get DSSKeyRules.
    /// </summary>
    public sealed class DSSKeyRulesResult : APIResult<List<DSSKeyRuleModel>>
    {
        internal DSSKeyRulesResult(HttpResponseMessage r) : base(r) { }
    }

    /// <summary>
    /// Result of Get DSSKeyRule.
    /// </summary>
    public sealed class DSSKeyRuleResult : APIResult<DSSKeyRuleModel>
    {
        internal DSSKeyRuleResult(HttpResponseMessage r) : base(r) { }
    }

    /// <summary>
    /// Result of Get Permissions.
    /// </summary>
    public sealed class PermissionsResult : APIResult<List<PermissionModel>>
    {
        internal PermissionsResult(HttpResponseMessage r) : base(r) { }
    }

    /// <summary>
    /// Result of Get Permissions with Access Level.
    /// </summary>
    public sealed class PermissionsAccessLevelResult : APIResult<List<PermissionAccessLevelModel>>
    {
        internal PermissionsAccessLevelResult(HttpResponseMessage r) : base(r) { }
    }

    /// <summary>
    /// Result of Post Permissions.
    /// </summary>
    public sealed class PermissionsPostResult : APIResult<string>
    {
        internal PermissionsPostResult(HttpResponseMessage r) : base(r) { }
    }

    /// <summary>
    /// Result of Get Platforms.
    /// </summary>
    public sealed class PlatformsResult : APIResult<List<PlatformModel>>
    {
        internal PlatformsResult(HttpResponseMessage r) : base(r) { }
    }

    /// <summary>
    /// Result of Get Platform.
    /// </summary>
    public sealed class PlatformResult : APIResult<PlatformModel>
    {
        internal PlatformResult(HttpResponseMessage r) : base(r) { }
    }

    /// <summary>
    /// Result of Get Replay.
    /// </summary>
    public sealed class ReplayResult : APIResult<ReplayModel>
    {
        internal ReplayResult(HttpResponseMessage r) : base(r) { }
    }

    /// <summary>
    /// Result of Post Replay.
    /// </summary>
    public sealed class ReplayPostResult : APIResult<ReplayPostResponseModel>
    {
        internal ReplayPostResult(HttpResponseMessage r) : base(r) { }
    }

    /// <summary>
    /// Result of Get Roles.
    /// </summary>
    public sealed class RolesResult : APIResult<List<RoleModel>>
    {
        internal RolesResult(HttpResponseMessage r) : base(r) { }
    }

    /// <summary>
    /// Result of Post Roles.
    /// </summary>
    public sealed class RolesPostResult : APIResult<string>
    {
        internal RolesPostResult(HttpResponseMessage r) : base(r) { }
    }

    /// <summary>
    /// Result of Get Sessions.
    /// </summary>
    public sealed class SessionsResult : APIResult<List<SessionModel>>
    {
        internal SessionsResult(HttpResponseMessage r) : base(r) { }
    }

    /// <summary>
    /// Result of Get Session.
    /// </summary>
    public sealed class SessionResult : APIResult<SessionModel>
    {
        internal SessionResult(HttpResponseMessage r) : base(r) { }
    }

    /// <summary>
    /// Result of Post Sessions.
    /// </summary>
    public class SessionsPostResult : APIResult<SessionsPostResponseModel>
    {
        internal SessionsPostResult(HttpResponseMessage r) : base(r) { }
    }

    /// <summary>
    /// Result of POST Sessions/{id}/Lock.
    /// </summary>
    public sealed class SessionsLockResult : APIResult<string>
    {
        internal SessionsLockResult(HttpResponseMessage r) : base(r) { }
    }

    /// <summary>
    /// Result of Get Nodes.
    /// </summary>
    public sealed class NodesResult : APIResult<List<NodeModel>>
    {
        internal NodesResult(HttpResponseMessage r) : base(r) { }
    }

    /// <summary>
    /// Result of Get Node.
    /// </summary>
    public sealed class NodeResult : APIResult<NodeModel>
    {
        internal NodeResult(HttpResponseMessage r) : base(r) { }
    }

    /// <summary>
    /// Result of Get SmartRules.
    /// </summary>
    public sealed class SmartRulesResult : APIResult<List<SmartRuleModel>>
    {
        internal SmartRulesResult(HttpResponseMessage r) : base(r) { }
    }

    /// <summary>
    /// Result of Get SmartRule.
    /// </summary>
    public sealed class SmartRuleResult : APIResult<SmartRuleModel>
    {
        internal SmartRuleResult(HttpResponseMessage r) : base(r) { }
    }

    /// <summary>
    /// Result of Get QuickRules.
    /// </summary>
    public sealed class QuickRulesResult : APIResult<List<QuickRuleModel>>
    {
        internal QuickRulesResult(HttpResponseMessage r) : base(r) { }
    }

    /// <summary>
    /// Result of Get QuickRule.
    /// </summary>
    public sealed class QuickRuleResult : APIResult<QuickRuleModel>
    {
        internal QuickRuleResult(HttpResponseMessage r) : base(r) { }
    }

    /// <summary>
    /// Result of Get TicketSystems.
    /// </summary>
    public sealed class TicketSystemsResult : APIResult<List<TicketSystemModel>>
    {
        internal TicketSystemsResult(HttpResponseMessage r) : base(r) { }
    }

    /// <summary>
    /// Result of Get UserGroups.
    /// </summary>
    public sealed class UserGroupsResult : APIResult<List<UserGroupModel>>
    {
        internal UserGroupsResult(HttpResponseMessage r) : base(r) { }
    }

    /// <summary>
    /// Result of Get User Group.
    /// </summary>
    public sealed class UserGroupResult : APIResult<UserGroupModel>
    {
        internal UserGroupResult(HttpResponseMessage r) : base(r) { }
    }

    /// <summary>
    /// Result of Get Users.
    /// </summary>
    public sealed class UsersResult : APIResult<List<UserModel>>
    {
        internal UsersResult(HttpResponseMessage r) : base(r) { }
    }

    /// <summary>
    /// Result of Get User.
    /// </summary>
    public sealed class UserResult : APIResult<UserModel>
    {
        internal UserResult(HttpResponseMessage r) : base(r) { }
    }

    /// <summary>
    /// Result of Get Vulnerabilities.
    /// </summary>
    public sealed class VulnerabilitiesResult : APIResult<List<VulnerabilitiesModel>>
    {
        internal VulnerabilitiesResult(HttpResponseMessage r) : base(r) { }
    }

    /// <summary>
    /// Result of Get OperatingSystems.
    /// </summary>
    public sealed class OperatingSystemsResult : APIResult<List<OperatingSystemModel>>
    {
        internal OperatingSystemsResult(HttpResponseMessage r) : base(r) { }
    }

    /// <summary>
    /// Result of Get EntityTypes.
    /// </summary>
    public sealed class EntityTypesResult : APIResult<List<EntityTypeModel>>
    {
        internal EntityTypesResult(HttpResponseMessage r) : base(r) { }
    }

    /// <summary>
    /// Result of Get Configuration/Version.
    /// </summary>
    public sealed class ConfigurationVersionResult : APIResult<VersionModel>
    {
        internal ConfigurationVersionResult(HttpResponseMessage r) : base(r) { }
    }

    /// <summary>
    /// Result of Get OracleInternetDirectories.
    /// </summary>
    public sealed class OracleInternetDirectoriesResult : APIResult<List<OracleInternetDirectoryModel>>
    {
        internal OracleInternetDirectoriesResult(HttpResponseMessage r) : base(r) { }
    }

    /// <summary>
    /// Result of Get OracleInternetDirectories.
    /// </summary>
    public sealed class OracleInternetDirectoryResult : APIResult<OracleInternetDirectoryModel>
    {
        internal OracleInternetDirectoryResult(HttpResponseMessage r) : base(r) { }
    }

    /// <summary>
    /// Result of Post OracleInternetDirectories/{id}/Test.
    /// </summary>
    public sealed class OracleInternetDirectoryTestResult : APIResult<OracleInternetDirectoryTestResultModel>
    {
        internal OracleInternetDirectoryTestResult(HttpResponseMessage r) : base(r) { }
    }

    /// <summary>
    /// Result of Post OracleInternetDirectories/{id}/Services/Query.
    /// </summary>
    public sealed class OracleInternetDirectoryQueryServicesResult : APIResult<OracleInternetDirectoryQuerySevicesResultModel>
    {
        internal OracleInternetDirectoryQueryServicesResult(HttpResponseMessage r) : base(r) { }
    }

    /// <summary>
    /// Result of Get Directories.
    /// </summary>
    public sealed class DirectoriesResult : APIResult<List<DirectoryModel>>
    {
        internal DirectoriesResult(HttpResponseMessage r) : base(r) { }
    }

    /// <summary>
    /// Result of Get Directory.
    /// </summary>
    public sealed class DirectoryResult : APIResult<DirectoryModel>
    {
        internal DirectoryResult(HttpResponseMessage r) : base(r) { }
    }

    /// <summary>
    /// Result of Get AddressGroups.
    /// </summary>
    public sealed class AddressGroupsResult : APIResult<List<AddressGroupModel>>
    {
        internal AddressGroupsResult(HttpResponseMessage r) : base(r) { }
    }

    /// <summary>
    /// Result of Get AddressGroup.
    /// </summary>
    public sealed class AddressGroupResult : APIResult<AddressGroupModel>
    {
        internal AddressGroupResult(HttpResponseMessage r) : base(r) { }
    }

    /// <summary>
    /// Result of Get Addresses.
    /// </summary>
    public sealed class AddressesResult : APIResult<List<AddressModel>>
    {
        internal AddressesResult(HttpResponseMessage r) : base(r) { }
    }

    /// <summary>
    /// Result of Get Address
    /// </summary>
    public sealed class AddressResult : APIResult<AddressModel>
    {
        internal AddressResult(HttpResponseMessage r) : base(r) { }
    }

}
