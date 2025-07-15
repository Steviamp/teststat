namespace UCStatistics.Shared.DTOs
{
    public class IndividualTicketDto
    {
        public int OfficeNr { get; set; }
        public string OfficeName { get; set; } = string.Empty;
        public string TicketNumber { get; set; } = string.Empty;
        public string ServiceName { get; set; } = string.Empty;
        public int GoldenClients { get; set; }
        public int PlasticCards { get; set; }
        public int DigitalCards { get; set; }
        public int PACustomers { get; set; }
        public int PersonalQueue { get; set; }
        public int DigitalTickets { get; set; }
        public int? Workstation { get; set; }
        public int? UserName { get; set; }
        public string TicketICU { get; set; } = string.Empty; 
        public bool IsLostTicket { get; set; }
        public DateTime? TicketDateTime { get; set; }
        public DateTime? ForwardingTime { get; set; } 
        public DateTime? EndOfServiceTime { get; set; } 
        public TimeSpan? TotalTime { get; set; } 
        public TimeSpan? WaitingTime { get; set; }
        public TimeSpan? ServiceTime { get; set; }
        public DateTime? LastUpdateTime { get; set; }
    }
}
