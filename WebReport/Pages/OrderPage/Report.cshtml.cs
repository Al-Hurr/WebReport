using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebReport.Models;

namespace WebReport.Pages.OrderPage
{
    public class ReportModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public ReportModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<ReportClass> ReportClasses { get; set; }

        [ModelBinder]
        public string ReportDateFrom { get; set; }

        [ModelBinder]
        public string ReportDateTo { get; set; }

        public async Task OnGet()
        {
            var orders = await _context.Orders.ToListAsync();

            ReportClasses = await GetReports();
        }

        public async Task<IActionResult> OnPost()
        {
            ReportClasses = await GetReports();

            using (XLWorkbook workbook = new XLWorkbook(XLEventTracking.Disabled))
            {
                var worksheet = workbook.Worksheets.Add("Report");

                worksheet.Cell("A1").Value = "Дата";
                worksheet.Column("A").Width = 10;
                worksheet.Cell("B1").Value = "Количество заказов с суммой от 0 до 1000";
                worksheet.Column("B").Width = 50;
                worksheet.Cell("C1").Value = "Количество заказов с суммой от 1001 до 5000";
                worksheet.Column("C").Width = 50;
                worksheet.Cell("D1").Value = "Количество заказов с суммой от 5001";
                worksheet.Column("D").Width = 50;
                worksheet.Row(1).Style.Font.Bold = true;

                //нумерация строк/столбцов начинается с индекса 1 (не 0)
                for (int i = 0; i < ReportClasses.Count(); i++)
                {
                    worksheet.Cell(i + 2, 1).Value = ReportClasses.ToArray()[i].OrdersDate;
                    worksheet.Cell(i + 2, 1).Style.NumberFormat.Format = "MM/DD/YYYY";
                    worksheet.Cell(i + 2, 2).Value = ReportClasses.ToArray()[i].LowAmountCount;
                    worksheet.Cell(i + 2, 3).Value = ReportClasses.ToArray()[i].AverageAmountCount;
                    worksheet.Cell(i + 2, 4).Value = ReportClasses.ToArray()[i].HighAmountCount;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Flush();

                    return new FileContentResult(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        FileDownloadName = $"Report_{DateTime.UtcNow.ToShortDateString()}.xlsx"
                    };
                }
            }
        }

        public async Task<IEnumerable<ReportClass>> GetReports()
        {
            var orders = await _context.Orders.ToListAsync();

            if (!string.IsNullOrEmpty(ReportDateFrom))
            {
                orders = orders.Where(x => x.OrderDate.Date >= Convert.ToDateTime(ReportDateFrom).Date).ToList();
            }

            if (!string.IsNullOrEmpty(ReportDateTo))
            {
                orders = orders.Where(x => x.OrderDate.Date <= Convert.ToDateTime(ReportDateTo).Date).ToList();
            }

            return orders.GroupBy(x => x.OrderDate, x => x.Amount, (orders, amounts) => new ReportClass
            {
                OrdersDate = orders,
                LowAmountCount = amounts.Where(x => x >= 0 && x <= 1000).Count(),
                AverageAmountCount = amounts.Where(x => x > 1000 && x <= 5000).Count(),
                HighAmountCount = amounts.Where(x => x > 5000).Count()
            }).OrderBy(x => x.OrdersDate).ToList();
        }
    }
}
