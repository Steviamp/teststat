using Dapper;
using UCStatistics.Data;
using UCStatistics.Shared.DTOs;

namespace UCStatistics.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly DapperContext _db;
        public ReportRepository(DapperContext db) => _db = db;

        public async Task<IEnumerable<OfficeInfo>> GetOfficesAsync()
        {
            const string sql = @"
            SELECT DISTINCT
                OFFICE_NR     AS OfficeNr,
                OFFICE_NAME   AS OfficeName,
                LEVEL2_NR     AS Level2Nr,
                LEVEL2_NAME   AS Level2Name,
                LEVEL3_NR     AS Level3Nr,
                LEVEL3_NAME   AS Level3Name
            FROM CUSTOMER_QUEUE_INFO_DAILY
            ORDER BY Level3Name, Level2Name, OfficeName;
            ";

            using var conn = _db.CreateConnection();
            return await conn.QueryAsync<OfficeInfo>(sql);
        }

        public async Task<IEnumerable<ServiceInfo>> GetServicesAsync()
        {
            const string sql = @"
                SELECT DISTINCT
                  h.SERVICETYPE_NR AS ServiceCode,
                  svc.SERVICETYPE_NAME AS ServiceName
                FROM CUSTOMER_QUEUE_INFO_DAILY h
                INNER JOIN SERVICETYPE_QUEUE_INFO svc 
                  ON h.SERVICETYPE_NR = svc.SERVICETYPE_NR
                WHERE svc.SERVICETYPE_NAME IS NOT NULL 
                  AND svc.SERVICETYPE_NAME != ''
                ORDER BY ServiceName;
                ";

            using var conn = _db.CreateConnection();
            return await conn.QueryAsync<ServiceInfo>(sql);
        }

        public async Task<IEnumerable<SummaryDto>> GetHistoricalAsync(FilterCriteria criteria)
        {
            int ObjectiveWaitingSeconds = 300;     // Στόχος αναμονής (σε sec)
            int ObjectiveServiceSeconds = 600;     // Στόχος εξυπηρέτησης (σε sec)

            var sql = @"SELECT * FROM fn_GetCustomerQueueStats(
                            @DateFrom,
                            @DateTo,
                            @Level2Nr,
                            @Level3Nr,
                            @OfficeNr,
                            @ObjectiveWaitingSeconds,
                            @ObjectiveServiceSeconds
                        )";

            using var conn = _db.CreateConnection();
            return await conn.QueryAsync<SummaryDto>(sql, new
            {
                criteria.DateFrom,
                criteria.DateTo,
                criteria.Level2Nr,
                criteria.Level3Nr,
                criteria.OfficeNr, 
                ObjectiveWaitingSeconds,
                ObjectiveServiceSeconds
            });
        }

        public async Task<IEnumerable<ServiceSummaryDto>> GetServiceSummaryAsync(FilterCriteria criteria)
        {
            int ObjectiveWaitingSeconds = 300;     // Στόχος αναμονής (σε sec)
            int ObjectiveServiceSeconds = 600;     // Στόχος εξυπηρέτησης (σε sec)

            var sql = @"SELECT * FROM fn_GetCustomerQueueServiceCodeStats(
                            @DateFrom,
                            @DateTo,
                            @Level2Nr,
                            @Level3Nr,
                            @OfficeNr,
                            @ObjectiveWaitingSeconds,
                            @ObjectiveServiceSeconds,
                            @ServiceCode
                        )";

            using var conn = _db.CreateConnection();
            return await conn.QueryAsync<ServiceSummaryDto>(sql, new
            {
                criteria.DateFrom,
                criteria.DateTo,
                criteria.Level2Nr,
                criteria.Level3Nr,
                criteria.OfficeNr, 
                ObjectiveWaitingSeconds,
                ObjectiveServiceSeconds,
                criteria.ServiceCode
            });
        }

        public async Task<IEnumerable<IndividualTicketDto>> GetIndividualTicketDetailsAsync(FilterCriteria criteria)
        {
            var sql = @"SELECT * FROM fn_GetIndividualTickets(
                            @DateFrom,
                            @DateTo,
                            @Level2Nr,
                            @Level3Nr,
                            @OfficeNr
                        )";

            using var conn = _db.CreateConnection();
            return await conn.QueryAsync<IndividualTicketDto>(sql, new
            {
                criteria.DateFrom,
                criteria.DateTo,
                criteria.Level2Nr,
                criteria.Level3Nr,
                criteria.OfficeNr
            });
        }

        public async Task<IEnumerable<LiveBranchDto>> GetLiveSummaryAsync(FilterCriteria criteria)
        {
            var sql = @"SELECT * FROM fn_GetLiveBranchStatistics(
                            @Level2Nr,
                            @Level3Nr,
                            @OfficeNr
                        )";

            using var conn = _db.CreateConnection();
            return await conn.QueryAsync<LiveBranchDto>(sql, new
            {
                criteria.Level2Nr,
                criteria.Level3Nr,
                criteria.OfficeNr
            });
        }

        public async Task<IEnumerable<LiveServiceDto>> GetLiveServiceSummaryAsync(FilterCriteria criteria)
        {
            var sql = @"SELECT * FROM fn_GetLiveServiceStatistics(
                            @Level2Nr,
                            @Level3Nr,
                            @OfficeNr,
                            @ServiceCode
                        )";

            using var conn = _db.CreateConnection();
            return await conn.QueryAsync<LiveServiceDto>(sql, new
            {
                criteria.Level2Nr,
                criteria.Level3Nr,
                criteria.OfficeNr,
                criteria.ServiceCode
            });
        }

        public async Task<IEnumerable<IndividualTicketDto>> GetLiveIndividualTicketDetailsAsync(FilterCriteria criteria)
        {
            var sql = @"SELECT * FROM fn_fn_GetLiveIndividualTickets(
                            @DateFrom,
                            @DateTo,
                            @Level2Nr,
                            @Level3Nr,
                            @OfficeNr
                        )";

            using var conn = _db.CreateConnection();
            return await conn.QueryAsync<IndividualTicketDto>(sql, new
            {
                criteria.DateFrom,
                criteria.DateTo,
                criteria.Level2Nr,
                criteria.Level3Nr,
                criteria.OfficeNr
            });
        }
    }
}
