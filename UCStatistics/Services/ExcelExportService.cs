using ClosedXML.Excel;
using UCStatistics.Shared.DTOs;

namespace UCStatistics.Services
{
    public class ExcelExportService
    {
        public byte[] ExportSummaryToExcel(IEnumerable<SummaryDto> data, string sheetName = "Branch Summary")
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add(sheetName);

            // Headers
            worksheet.Cell(1, 1).Value = "Office Nr";
            worksheet.Cell(1, 2).Value = "Branch Name";
            worksheet.Cell(1, 3).Value = "Incoming Customers";
            worksheet.Cell(1, 4).Value = "Unattended Customers";
            worksheet.Cell(1, 5).Value = "Served Customers";
            worksheet.Cell(1, 6).Value = "Plastic Cards";
            worksheet.Cell(1, 7).Value = "Digital Cards";
            worksheet.Cell(1, 8).Value = "Golden Clients";
            worksheet.Cell(1, 9).Value = "PA Customers";
            worksheet.Cell(1, 10).Value = "Digital Tickets";
            worksheet.Cell(1, 11).Value = "Avg Waiting Time";
            worksheet.Cell(1, 12).Value = "Avg Service Time";
            worksheet.Cell(1, 13).Value = "Avg Customer Time";
            worksheet.Cell(1, 14).Value = "Objective Waiting %";
            worksheet.Cell(1, 15).Value = "Objective Service %";
            worksheet.Cell(1, 16).Value = "Max Waiting Time";
            worksheet.Cell(1, 17).Value = "Max Service Time";

            // Style headers
            var headerRange = worksheet.Range(1, 1, 1, 17);
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;

            // Data
            int row = 2;
            foreach (var item in data)
            {
                worksheet.Cell(row, 1).Value = item.OfficeNr;
                worksheet.Cell(row, 2).Value = item.OfficeName;
                worksheet.Cell(row, 3).Value = item.IncomingCustomers;
                worksheet.Cell(row, 4).Value = item.UnattendedCustomers;
                worksheet.Cell(row, 5).Value = item.ServedCustomers;
                worksheet.Cell(row, 6).Value = item.PlasticCards;
                worksheet.Cell(row, 7).Value = item.DigitalCards;
                worksheet.Cell(row, 8).Value = item.GoldenClients;
                worksheet.Cell(row, 9).Value = item.PACustomers;
                worksheet.Cell(row, 10).Value = item.DigitalTickets;
                worksheet.Cell(row, 11).Value = item.AvgWaitingTime.ToString(@"hh\:mm\:ss");
                worksheet.Cell(row, 12).Value = item.AvgServiceTime.ToString(@"hh\:mm\:ss");
                worksheet.Cell(row, 13).Value = item.AvgCustomerTime.ToString(@"hh\:mm\:ss");
                worksheet.Cell(row, 14).Value = item.ObjectiveWaitingPercent;
                worksheet.Cell(row, 15).Value = item.ObjectiveServicePercent;
                worksheet.Cell(row, 16).Value = item.MaxWaitingTime.ToString(@"hh\:mm\:ss");
                worksheet.Cell(row, 17).Value = item.MaxServiceTime.ToString(@"hh\:mm\:ss");
                row++;
            }

            // Auto-fit columns
            worksheet.ColumnsUsed().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }

        public byte[] ExportServiceSummaryToExcel(IEnumerable<ServiceSummaryDto> data, string sheetName = "Service Summary")
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add(sheetName);

            // Headers
            worksheet.Cell(1, 1).Value = "Office Nr";
            worksheet.Cell(1, 2).Value = "Branch Name";
            worksheet.Cell(1, 3).Value = "Service Code";
            worksheet.Cell(1, 4).Value = "Service Name";
            worksheet.Cell(1, 5).Value = "Incoming Customers";
            worksheet.Cell(1, 6).Value = "Unattended Customers";
            worksheet.Cell(1, 7).Value = "Served Customers";
            worksheet.Cell(1, 8).Value = "Plastic Cards";
            worksheet.Cell(1, 9).Value = "Digital Cards";
            worksheet.Cell(1, 10).Value = "Golden Clients";
            worksheet.Cell(1, 11).Value = "PA Customers";
            worksheet.Cell(1, 12).Value = "Digital Tickets";
            worksheet.Cell(1, 13).Value = "Avg Waiting Time";
            worksheet.Cell(1, 14).Value = "Avg Service Time";
            worksheet.Cell(1, 15).Value = "Avg Customer Time";
            worksheet.Cell(1, 16).Value = "Objective Waiting %";
            worksheet.Cell(1, 17).Value = "Objective Service %";
            worksheet.Cell(1, 18).Value = "Max Waiting Time";
            worksheet.Cell(1, 19).Value = "Max Service Time";

            // Style headers
            var headerRange = worksheet.Range(1, 1, 1, 19);
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;

            // Data
            int row = 2;
            foreach (var item in data)
            {
                worksheet.Cell(row, 1).Value = item.OfficeNr;
                worksheet.Cell(row, 2).Value = item.OfficeName;
                worksheet.Cell(row, 3).Value = item.ServiceCode;
                worksheet.Cell(row, 4).Value = item.ServiceName;
                worksheet.Cell(row, 5).Value = item.IncomingCustomers;
                worksheet.Cell(row, 6).Value = item.UnattendedCustomers;
                worksheet.Cell(row, 7).Value = item.ServedCustomers;
                worksheet.Cell(row, 8).Value = item.PlasticCards;
                worksheet.Cell(row, 9).Value = item.DigitalCards;
                worksheet.Cell(row, 10).Value = item.GoldenClients;
                worksheet.Cell(row, 11).Value = item.PACustomers;
                worksheet.Cell(row, 12).Value = item.DigitalTickets;
                worksheet.Cell(row, 13).Value = item.AvgWaitingTime.ToString(@"hh\:mm\:ss");
                worksheet.Cell(row, 14).Value = item.AvgServiceTime.ToString(@"hh\:mm\:ss");
                worksheet.Cell(row, 15).Value = item.AvgCustomerTime.ToString(@"hh\:mm\:ss");
                worksheet.Cell(row, 16).Value = item.ObjectiveWaitingPercent;
                worksheet.Cell(row, 17).Value = item.ObjectiveServicePercent;
                worksheet.Cell(row, 18).Value = item.MaxWaitingTime.ToString(@"hh\:mm\:ss");
                worksheet.Cell(row, 19).Value = item.MaxServiceTime.ToString(@"hh\:mm\:ss");
                row++;
            }

            // Auto-fit columns
            worksheet.ColumnsUsed().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }
        public byte[] ExportIndividualTicketsToExcel(IEnumerable<IndividualTicketDto> data, string sheetName = "Individual Tickets")
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add(sheetName);

            // Headers
            worksheet.Cell(1, 1).Value = "Branch Code";
            worksheet.Cell(1, 2).Value = "Branch Name";
            worksheet.Cell(1, 3).Value = "Ticket Nr";
            worksheet.Cell(1, 4).Value = "Service Name";
            worksheet.Cell(1, 5).Value = "Golden Client";
            worksheet.Cell(1, 6).Value = "Plastic Card";
            worksheet.Cell(1, 7).Value = "Digital Card";
            worksheet.Cell(1, 8).Value = "PA Customer";
            worksheet.Cell(1, 9).Value = "Personal Queue";
            worksheet.Cell(1, 10).Value = "Digital Ticket";
            worksheet.Cell(1, 11).Value = "Workstation";
            worksheet.Cell(1, 12).Value = "User Name";
            worksheet.Cell(1, 13).Value = "Ticket ICU";
            worksheet.Cell(1, 14).Value = "Lost";
            worksheet.Cell(1, 15).Value = "Date";
            worksheet.Cell(1, 16).Value = "Time";
            worksheet.Cell(1, 17).Value = "Forward Time";
            worksheet.Cell(1, 18).Value = "End Service Time";
            worksheet.Cell(1, 19).Value = "Total Time";
            worksheet.Cell(1, 20).Value = "Service Time";
            worksheet.Cell(1, 21).Value = "End Time";
            worksheet.Cell(1, 22).Value = "Waiting Time";
            worksheet.Cell(1, 23).Value = "Last Update";

            // Style headers
            var headerRange = worksheet.Range(1, 1, 1, 24);
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;
            headerRange.Style.Alignment.WrapText = false;
            headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            // Data
            int row = 2;
            foreach (var item in data)
            {
                worksheet.Cell(row, 1).Value = item.OfficeNr; // Branch Code
                worksheet.Cell(row, 2).Value = item.OfficeName;
                worksheet.Cell(row, 3).Value = item.TicketNumber;
                worksheet.Cell(row, 4).Value = item.ServiceName;
                worksheet.Cell(row, 5).Value = item.GoldenClients;
                worksheet.Cell(row, 6).Value = item.PlasticCards;
                worksheet.Cell(row, 7).Value = item.DigitalCards;
                worksheet.Cell(row, 8).Value = item.PACustomers;
                worksheet.Cell(row, 9).Value = item.PersonalQueue;
                worksheet.Cell(row, 10).Value = item.DigitalTickets;
                worksheet.Cell(row, 11).Value = item.Workstation?.ToString() ?? "";
                worksheet.Cell(row, 12).Value = item.UserName?.ToString() ?? "";
                worksheet.Cell(row, 13).Value = item.TicketICU;
                worksheet.Cell(row, 14).Value = item.IsLostTicket ? "TRUE" : "FALSE";
                worksheet.Cell(row, 15).Value = item.TicketDateTime?.ToString("dd.MM.yyyy") ?? "";
                worksheet.Cell(row, 16).Value = item.TicketDateTime?.ToString("HH:mm:ss") ?? "";
                worksheet.Cell(row, 17).Value = item.ForwardingTime?.ToString("dd.MM.yyyy HH:mm:ss") ?? "";
                worksheet.Cell(row, 18).Value = item.EndOfServiceTime?.ToString("dd.MM.yyyy HH:mm") ?? "";
                worksheet.Cell(row, 19).Value = item.TotalTime?.ToString(@"hh\:mm\:ss") ?? "";
                worksheet.Cell(row, 20).Value = item.ServiceTime?.ToString(@"hh\:mm\:ss") ?? "";
                worksheet.Cell(row, 21).Value = item.EndOfServiceTime;
                worksheet.Cell(row, 22).Value = item.WaitingTime?.ToString(@"hh\:mm\:ss") ?? "";
                worksheet.Cell(row, 23).Value = item.LastUpdateTime?.ToString("dd.MM.yyyy HH:mm:ss") ?? "";
                row++;
            }

            // Auto-fit columns
            worksheet.ColumnsUsed().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }
    }
}