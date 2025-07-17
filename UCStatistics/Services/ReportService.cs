using UCStatistics.Repositories;
using UCStatistics.Shared.DTOs;

namespace UCStatistics.Services
{
    public class ReportService
    {
        private readonly IReportRepository _repository;

        public ReportService(IReportRepository repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<OfficeInfo>> GetOfficesAsync()
             => _repository.GetOfficesAsync();

        public Task<IEnumerable<ServiceInfo>> GetServicesAsync()
               => _repository.GetServicesAsync();
        public Task<IEnumerable<SummaryDto>> GetHistoricalAsync(FilterCriteria criteria)
        {
            return _repository.GetHistoricalAsync(criteria);
        }

        public Task<IEnumerable<ServiceSummaryDto>> GetServiceSummaryAsync(FilterCriteria criteria)
        {
            return _repository.GetServiceSummaryAsync(criteria);
        }

        public Task<IEnumerable<IndividualTicketDto>> GetIndividualTicketDetailsAsync(FilterCriteria criteria)
        {
            return _repository.GetIndividualTicketDetailsAsync(criteria);
        }

        public Task<IEnumerable<LiveBranchDto>> GetLiveSummaryAsync(FilterCriteria criteria)
        {
            return _repository.GetLiveSummaryAsync(criteria);
        }

        public Task<IEnumerable<LiveServiceDto>> GetLiveServiceSummaryAsync(FilterCriteria criteria)
        {
            return _repository.GetLiveServiceSummaryAsync(criteria);
        }

        public async Task<byte[]> ExportSummaryToExcelAsync(FilterCriteria criteria)
        {
            var data = await GetHistoricalAsync(criteria);
            var excelService = new ExcelExportService();
            return excelService.ExportSummaryToExcel(data, "Branch Summary");
        }

        public async Task<byte[]> ExportServiceSummaryToExcelAsync(FilterCriteria criteria)
        {
            var data = await GetServiceSummaryAsync(criteria);
            var excelService = new ExcelExportService();
            return excelService.ExportServiceSummaryToExcel(data, "Service Summary");
        }

        public async Task<byte[]> ExportIndividualTicketsToExcelAsync(FilterCriteria criteria)
        {
            var data = await GetIndividualTicketDetailsAsync(criteria);
            var excelService = new ExcelExportService();
            return excelService.ExportIndividualTicketsToExcel(data, "Individual Tickets");
        }

        public async Task<byte[]> ExportLiveSummaryToExcelAsync(FilterCriteria criteria)
        {
            var data = await GetLiveSummaryAsync(criteria);
            var excelService = new ExcelExportService();
            return excelService.ExportLiveSummaryToExcel(data, "Branch Summary");
        }

        public async Task<byte[]> ExportLiveServiceSummaryToExcelAsync(FilterCriteria criteria)
        {
            var data = await GetLiveServiceSummaryAsync(criteria);
            var excelService = new ExcelExportService();
            return excelService.ExportLiveServiceSummaryToExcel(data, "Service Summary");
        }
    }
}
