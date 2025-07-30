namespace UCStatistics.Shared.DTOs
{
    public class LiveBranchDto
    {
        public int OfficeNr { get; set; }
        public string OfficeName { get; set; } = string.Empty;
        public int InstalledCashiers { get; set; }
        public int ActiveCashiers { get; set; }
        public int WaitingCustomers { get; set; }
        public int ServedCustomers { get; set; }
        public int NumberOfCheckIns { get; set; }
        public int NumberOfPlasticCards { get; set; }
        public int NumberOfDigitalCards { get; set; }
        public int NumberOfGoldenClientsWaiting { get; set; }
        public int NumberOfGoldenClientsServed { get; set; }
        public int PersonalQueueWaiting { get; set; }
        public int PersonalQueueServed { get; set; }
        public int NumberOfDigitalTickets { get; set; }
        public TimeSpan AvgWaitingTime { get; set; }
        public TimeSpan AvgServiceTime { get; set; }
    }
}
