using UCStatistics.Shared.DTOs;

namespace UCStatistics.Repositories
{
    public interface IReportRepository
    {
        Task<IEnumerable<OfficeInfo>> GetOfficesAsync();
        Task<IEnumerable<ServiceInfo>> GetServicesAsync();
        Task<IEnumerable<SummaryDto>> GetHistoricalAsync(FilterCriteria criteria);
        Task<IEnumerable<ServiceSummaryDto>> GetServiceSummaryAsync(FilterCriteria criteria);
        Task<IEnumerable<IndividualTicketDto>> GetIndividualTicketDetailsAsync(FilterCriteria criteria);
    }
}
