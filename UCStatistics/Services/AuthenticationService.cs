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
        private readonly IWebHostEnvironment _environment;

        public AuthenticationService(
            ActiveDirectorySettings activeDirectorySettings,
            ILogger<AuthenticationService> logger,
            IWebHostEnvironment environment)
        {
            _activeDirectorySettings = activeDirectorySettings;
            _logger = logger;
            _environment = environment;
        }

        public ActiveDirectoryUserDto? GetAuthenticatedUser(HttpContext httpContext)
        {
            try
            {
                // FOR DEVELOPMENT TESTING ONLY
                if (_environment.IsDevelopment())
                {
                    return GetTestUser();
                }

                var user = httpContext.User?.Identity as WindowsIdentity;
                if (user == null || !user.IsAuthenticated)
                {
                    _logger.LogWarning("User not authenticated or WindowsIdentity not found");
                    return null;
                }

                var domain = user.Name.Split("\\")[0];
                var userName = user.Name.Split("\\")[1];
                var departmentPropertyName = _activeDirectorySettings.DepartmentPropertyName;

                using var context = new PrincipalContext(ContextType.Domain, domain);
                UserPrincipal userPrincipal = UserPrincipal.FindByIdentity(context, userName);

                if (userPrincipal == null)
                {
                    _logger.LogWarning("User principal not found for {UserName}", userName);
                    return null;
                }

                var directoryEntry = userPrincipal.GetUnderlyingObject() as DirectoryEntry;
                var groups = userPrincipal.GetGroups();
                var groupNames = groups.Select(g => g.Name).ToList();

                // Check for required groups with role assignment
                var userRole = GetUserRole(groupNames);
                if (userRole == UserRole.NoAccess)
                {
                    _logger.LogWarning("User {UserName} does not have access to the application. Groups: {Groups}",
                        userName, string.Join(", ", groupNames));
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

                // For Supervisor role, extract branch from department
                string? supervisorBranch = null;
                if (userRole == UserRole.Supervisor && !string.IsNullOrEmpty(department))
                {
                    supervisorBranch = ExtractBranchFromDepartment(department);
                }

                var userDto = new ActiveDirectoryUserDto()
                {
                    Domain = domain,
                    UserName = userName,
                    Email = userPrincipal.UserPrincipalName ?? "",
                    FirstName = userPrincipal.GivenName ?? "",
                    LastName = userPrincipal.Surname ?? "",
                    Title = title,
                    Groups = groupNames.ToArray(),
                    Branch = department,
                    Role = userRole,
                    SupervisorBranch = supervisorBranch
                };

                _logger.LogInformation("User {UserName} authenticated successfully with role {Role}",
                    userName, userRole);

                return userDto;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error authenticating user");
                return null;
            }
        }

        // FOR DEVELOPMENT TESTING ONLY - REMOVE IN PRODUCTION
        private ActiveDirectoryUserDto GetTestUser()
        {
            // Change this to test different roles
            var testRole = UserRole.Supervisor; // Change to UserRole.Supervisor, UserRole.StatisticsUser, etc.

            _logger.LogInformation("Creating test user with role: {Role}", testRole);

            return testRole switch
            {
                UserRole.Administrator => new ActiveDirectoryUserDto
                {
                    Domain = "TEST",
                    UserName = "admin.test",
                    Email = "admin.test@unicredit.gr",
                    FirstName = "Admin",
                    LastName = "Test",
                    Title = "Administrator",
                    Groups = new[] { "NEMOQ_QMS_Administrator OPU (AG)" },
                    Branch = "Default Office 1",
                    Role = UserRole.Administrator,
                    SupervisorBranch = null
                },
                UserRole.StatisticsUser => new ActiveDirectoryUserDto
                {
                    Domain = "TEST",
                    UserName = "stats.test",
                    Email = "stats.test@unicredit.gr",
                    FirstName = "Statistics",
                    LastName = "User",
                    Title = "Statistics Analyst",
                    Groups = new[] { "NEMOQ_QMS_Statistics user OPU (AG)" },
                    Branch = "Default Office 1",
                    Role = UserRole.StatisticsUser,
                    SupervisorBranch = null
                },
                UserRole.Supervisor => new ActiveDirectoryUserDto
                {
                    Domain = "TEST",
                    UserName = "supervisor.test",
                    Email = "supervisor.test@unicredit.gr",
                    FirstName = "Supervisor",
                    LastName = "Test",
                    Title = "Branch Supervisor",
                    Groups = new[] { "NEMOQ_QMS_Supervisor (AG)" },
                    Branch = "Default Office 1",
                    Role = UserRole.Supervisor,
                    SupervisorBranch = "218" // Only access to branch 218
                },
                _ => new ActiveDirectoryUserDto
                {
                    Domain = "TEST",
                    UserName = "noaccess.test",
                    Email = "noaccess.test@unicredit.gr",
                    FirstName = "No Access",
                    LastName = "User",
                    Title = "Branch User",
                    Groups = new[] { "NEMOQ_QMS_Branch user (AG)" },
                    Branch = "001 - Test Branch",
                    Role = UserRole.NoAccess,
                    SupervisorBranch = null
                }
            };
        }

        private UserRole GetUserRole(List<string> groupNames)
        {
            // Check for highest privilege first (Administrator)
            if (groupNames.Any(g => g.Equals("NEMOQ_QMS_Administrator OPU (AG)", StringComparison.OrdinalIgnoreCase)))
            {
                return UserRole.Administrator;
            }

            // Check for Statistics user
            if (groupNames.Any(g => g.Contains("NEMOQ_QMS") && g.Contains("Statistics") && g.Contains("user") && g.Contains("OPU") && g.Contains("(AG)")))
            {
                return UserRole.StatisticsUser;
            }

            // Check for Supervisor
            if (groupNames.Any(g => g.Equals("NEMOQ_QMS_Supervisor (AG)", StringComparison.OrdinalIgnoreCase)))
            {
                return UserRole.Supervisor;
            }

            // Branch users are explicitly denied access
            if (groupNames.Any(g => g.Equals("NEMOQ_QMS_Branch user (AG)", StringComparison.OrdinalIgnoreCase)))
            {
                return UserRole.NoAccess;
            }

            return UserRole.NoAccess;
        }

        private string? ExtractBranchFromDepartment(string department)
        {
            var parts = department.Split(" - ");
            return parts.Length > 0 ? parts[0] : null;
        }

        public bool CanAccessAllBranches(ActiveDirectoryUserDto user)
        {
            return user.Role == UserRole.Administrator || user.Role == UserRole.StatisticsUser;
        }

        public bool CanAccessBranch(ActiveDirectoryUserDto user, int? branchNumber)
        {
            if (CanAccessAllBranches(user))
                return true;

            if (user.Role == UserRole.Supervisor && !string.IsNullOrEmpty(user.SupervisorBranch))
            {
                return branchNumber?.ToString() == user.SupervisorBranch;
            }

            return false;
        }
    }

    public enum UserRole
    {
        NoAccess,
        Supervisor,
        StatisticsUser,
        Administrator
    }
}