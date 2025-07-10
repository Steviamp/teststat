namespace UCStatistics.Shared.DTOs
{
    public class FilterCriteria
    {
        public DateTime DateFrom { get; set; } = DateTime.Today.AddDays(-400);
        public DateTime DateTo { get; set; } = DateTime.Today;
        public TimeSpan TimeFrom { get; set; }
        public TimeSpan TimeTo { get; set; }
        public int? Level2Nr { get; set; }
        public int? Level3Nr { get; set; }
        public int? OfficeNr { get; set; }
        public int? ServiceCode { get; set; }
    }
}
