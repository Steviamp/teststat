namespace UCStatistics.Shared.DTOs
{
    public class IndividualTicketDto
    {
        public int BranchCode { get; set; }
        public string BranchName { get; set; } = string.Empty;
        public string TicketNr { get; set; } = string.Empty;
        public string ServiceName { get; set; } = string.Empty;
        public string GoldenClient { get; set; } = string.Empty;
        public string PlasticCard { get; set; } = string.Empty;
        public string DigitalCard { get; set; } = string.Empty;
        public string PACustomer { get; set; } = string.Empty;
        public string PersonalQueue { get; set; } = string.Empty;
        public string DigitalTicket { get; set; } = string.Empty;
        public int? Workstation { get; set; }
        public int? UserName { get; set; }
        public string TicketCU { get; set; } = string.Empty;
        public bool Lost { get; set; }
        public DateTime DateDdMmYyyy { get; set; }
        public DateTime? Time { get; set; }
        public DateTime? TicketTime { get; set; }
        public DateTime? ForwardTime { get; set; }
        public DateTime? EndOfServiceTime { get; set; }
        public TimeSpan? TotalTimeInBranch { get; set; }
        public TimeSpan? ServiceTimeForwardSt { get; set; }
        public string EndOfServiceTimeFormatted { get; set; } = string.Empty;
        public TimeSpan? WaitingTime { get; set; }
        public DateTime? LastUpdateTime { get; set; }
    }
}
