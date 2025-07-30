namespace UCStatistics.Models
{
    public class ActiveDirectorySettings
    {
        public string DepartmentPropertyName { get; set; } = "department";
        public string RequiredGroupToAuthenticate { get; set; } = "Domain Users";
    }
}
