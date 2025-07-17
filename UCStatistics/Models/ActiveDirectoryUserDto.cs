namespace UCStatistics.Models
{
    public class ActiveDirectoryUserDto
    {
        public string Domain { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Branch { get; set; } = string.Empty;
        public string[] Groups { get; set; } = Array.Empty<string>();
    }
}
