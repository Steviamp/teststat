using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Security.Principal;
using UCStatistics.Models;

namespace UCStatistics.Services
{
    [System.Runtime.Versioning.SupportedOSPlatform("windows")]
    public class AuthenticationService
    {
        private readonly ActiveDirectorySettings _activeDirectorySettings;
        private readonly ILogger<AuthenticationService> _logger;

        public AuthenticationService(
            ActiveDirectorySettings activeDirectorySettings,
            ILogger<AuthenticationService> logger)
        {
            _activeDirectorySettings = activeDirectorySettings;
            _logger = logger;
        }

        public ActiveDirectoryUserDto? GetAuthenticatedUser(HttpContext httpContext)
        {
            try
            {
                var user = httpContext.User?.Identity as WindowsIdentity;
                if (user == null || !user.IsAuthenticated)
                {
                    return null;
                }

                var domain = user.Name.Split("\\")[0];
                var userName = user.Name.Split("\\")[1];
                var departmentPropertyName = _activeDirectorySettings.DepartmentPropertyName;

                using var context = new PrincipalContext(ContextType.Domain, domain);
                UserPrincipal userPrincipal = UserPrincipal.FindByIdentity(context, userName);

                if (userPrincipal == null)
                {
                    return null;
                }

                var directoryEntry = userPrincipal.GetUnderlyingObject() as DirectoryEntry;
                var groups = userPrincipal.GetGroups();

                bool isMemberOfDomainUsers = groups.Any(group =>
                    group.Name.Equals(_activeDirectorySettings.RequiredGroupToAuthenticate,
                        StringComparison.OrdinalIgnoreCase));

                if (!isMemberOfDomainUsers)
                {
                    return null;
                }

                var department = "";
                if (directoryEntry != null && directoryEntry.Properties.Contains(departmentPropertyName))
                {
                    department = directoryEntry.Properties[departmentPropertyName].Value?.ToString() ?? "";
                }

                var title = "";
                if (directoryEntry != null && directoryEntry.Properties.Contains("Title"))
                {
                    title = directoryEntry.Properties["Title"].Value?.ToString() ?? "";
                }

                return new ActiveDirectoryUserDto()
                {
                    Domain = domain,
                    UserName = userName,
                    Email = userPrincipal.UserPrincipalName ?? "",
                    FirstName = userPrincipal.GivenName ?? "",
                    LastName = userPrincipal.Surname ?? "",
                    Title = title,
                    Groups = groups.Select(group => group.Name).ToArray(),
                    Branch = department
                };
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error authenticating user");
                return null;
            }
        }
    }
}
