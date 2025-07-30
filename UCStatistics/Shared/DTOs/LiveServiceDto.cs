namespace UCStatistics.Shared.DTOs
{
    public class LiveServiceDto
    {
        public int ServiceCode { get; set; }
        public string ServiceName { get; set; } = string.Empty;
        public string OfficeName { get; set; } = String.Empty;
        public int ActiveCashiers { get; set; }
        public int WaitingCustomers { get; set; }
        public int ServedCustomers { get; set; }
        public TimeSpan AvgWaitingTime { get; set; }
        public TimeSpan AvgServingTime { get; set; }
    }
}
