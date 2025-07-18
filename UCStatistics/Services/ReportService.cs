using UCStatistics.Repositories;
using UCStatistics.Shared.DTOs;
using UCStatistics.Models;
using UCStatistics.Services;

namespace UCStatistics.Services
{
    public class ReportService
    {
        private readonly IReportRepository _repository;
        private readonly AuthenticationService _authService;

        public ReportService(IReportRepository repository, AuthenticationService authService)
        {
            _repository = repository;
            _authService = authService;
        }

        public Task<IEnumerable<OfficeInfo>> GetOfficesAsync()
             => _repository.GetOfficesAsync();

        public Task<IEnumerable<ServiceInfo>> GetServicesAsync()
               => _repository.GetServicesAsync();
        public Task<IEnumerable<SummaryDto>> GetHistoricalAsync(FilterCriteria criteria, ActiveDirectoryUserDto? currentUser = null)
        {
            var filteredCriteria = ApplyRoleBasedFiltering(criteria, currentUser);
            return _repository.GetHistoricalAsync(filteredCriteria);
        }

        public Task<IEnumerable<ServiceSummaryDto>> GetServiceSummaryAsync(FilterCriteria criteria, ActiveDirectoryUserDto? currentUser = null)
        {
            var filteredCriteria = ApplyRoleBasedFiltering(criteria, currentUser);
            return _repository.GetServiceSummaryAsync(filteredCriteria);
        }

        public Task<IEnumerable<IndividualTicketDto>> GetIndividualTicketDetailsAsync(FilterCriteria criteria, ActiveDirectoryUserDto? currentUser = null)
        {
            var filteredCriteria = ApplyRoleBasedFiltering(criteria, currentUser);
            return _repository.GetIndividualTicketDetailsAsync(filteredCriteria);
        }

        public Task<IEnumerable<LiveBranchDto>> GetLiveSummaryAsync(FilterCriteria criteria, ActiveDirectoryUserDto? currentUser = null)
        {
            var filteredCriteria = ApplyRoleBasedFiltering(criteria, currentUser);
            return _repository.GetLiveSummaryAsync(filteredCriteria);
        }

        public Task<IEnumerable<LiveServiceDto>> GetLiveServiceSummaryAsync(FilterCriteria criteria, ActiveDirectoryUserDto? currentUser = null)
        {
            var filteredCriteria = ApplyRoleBasedFiltering(criteria, currentUser);
            return _repository.GetLiveServiceSummaryAsync(filteredCriteria);
        }

        public async Task<byte[]> ExportSummaryToExcelAsync(FilterCriteria criteria, ActiveDirectoryUserDto? currentUser = null)
        {
            var filteredCriteria = ApplyRoleBasedFiltering(criteria, currentUser);
            var data = await GetHistoricalAsync(filteredCriteria);
            var excelService = new ExcelExportService();
            return excelService.ExportSummaryToExcel(data, "Branch Summary");
        }

        public async Task<byte[]> ExportServiceSummaryToExcelAsync(FilterCriteria criteria, ActiveDirectoryUserDto? currentUser = null)
        {
            var filteredCriteria = ApplyRoleBasedFiltering(criteria, currentUser);
            var data = await GetServiceSummaryAsync(filteredCriteria);
            var excelService = new ExcelExportService();
            return excelService.ExportServiceSummaryToExcel(data, "Service Summary");
        }

        public async Task<byte[]> ExportIndividualTicketsToExcelAsync(FilterCriteria criteria, ActiveDirectoryUserDto? currentUser = null)
        {
            var filteredCriteria = ApplyRoleBasedFiltering(criteria, currentUser);
            var data = await GetIndividualTicketDetailsAsync(filteredCriteria);
            var excelService = new ExcelExportService();
            return excelService.ExportIndividualTicketsToExcel(data, "Individual Tickets");
        }

        public async Task<byte[]> ExportLiveSummaryToExcelAsync(FilterCriteria criteria, ActiveDirectoryUserDto? currentUser = null)
        {
            var filteredCriteria = ApplyRoleBasedFiltering(criteria, currentUser);
            var data = await GetLiveSummaryAsync(filteredCriteria);
            var excelService = new ExcelExportService();
            return excelService.ExportLiveSummaryToExcel(data, "Branch Summary");
        }

        public async Task<byte[]> ExportLiveServiceSummaryToExcelAsync(FilterCriteria criteria, ActiveDirectoryUserDto? currentUser = null)
        {
            var filteredCriteria = ApplyRoleBasedFiltering(criteria, currentUser);
            var data = await GetLiveServiceSummaryAsync(filteredCriteria);
            var excelService = new ExcelExportService();
            return excelService.ExportLiveServiceSummaryToExcel(data, "Service Summary");
        }
        
        // Applies role-based filtering to the criteria based on the current user's permissions
        private FilterCriteria ApplyRoleBasedFiltering(FilterCriteria criteria, ActiveDirectoryUserDto? currentUser)
        {
            // Create a copy of the criteria to avoid modifying the original
            var filteredCriteria = new FilterCriteria
            {
                DateFrom = criteria.DateFrom,
                DateTo = criteria.DateTo,
                TimeFrom = criteria.TimeFrom,
                TimeTo = criteria.TimeTo,
                Level2Nr = criteria.Level2Nr,
                Level3Nr = criteria.Level3Nr,
                OfficeNr = criteria.OfficeNr,
                ServiceCode = criteria.ServiceCode
            };

            // If no user is provided, return original criteria
            if (currentUser == null)
                return filteredCriteria;

            // For Supervisor role, restrict to their specific branch
            if (currentUser.Role == UserRole.Supervisor && !string.IsNullOrEmpty(currentUser.SupervisorBranch))
            {
                if (int.TryParse(currentUser.SupervisorBranch, out int branchNumber))
                {
                    filteredCriteria.OfficeNr = branchNumber;
                }
            }

            // Administrator and StatisticsUser have access to all data
            // No additional filtering needed for these roles

            return filteredCriteria;
        }
    }
}
