namespace UCStatistics.Shared.DTOs
{
    public class FilterDialogModel
    {
        public List<OfficeInfo> Areas { get; set; } = new();
        public FilterCriteria Criteria { get; set; } = new();
    }
}
