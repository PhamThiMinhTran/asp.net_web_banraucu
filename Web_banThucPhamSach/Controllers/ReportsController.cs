using AspNetCore.Reporting;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Text;
using Web_banThucPhamSach.Data;
using System.IO.Packaging;

namespace Web_banThucPhamSach.Controllers
{
    public class ReportsController : Controller
    {
        private readonly WebBanThucPhamSachContext _context;

        public ReportsController(WebBanThucPhamSachContext context)
        {
            _context = context;
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }
        public IActionResult ReportStaff()
        {
            var Staff = _context.Staff.ToList();
            var format = Request.Query["format"].ToString();
            DataTable dataTable = new DataTable("dsStaff");
            dataTable.Columns.Add("Id", typeof(string));
            dataTable.Columns.Add("fullname", typeof(string));
            dataTable.Columns.Add("email", typeof(string));
            dataTable.Columns.Add("phone_number", typeof(string));
            dataTable.Columns.Add("address", typeof(string));
            dataTable.Columns.Add("create_at", typeof(DateTime));
            dataTable.Columns.Add("update_at", typeof(DateTime));
            foreach (var staff in Staff)
            {
                dataTable.Rows.Add(
                    staff.Id,
                    staff.FullName,
                    staff.Email,
                    staff.PhoneNumber,
                    staff.Address,
                    staff.CreateAt,
                    staff.UpdateAt
                );
            }
            string rdlcPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Reports", "StaffRPT.rdlc");
            var report = new LocalReport(rdlcPath);
            report.AddDataSource("DataSet1", dataTable);
            /* var result = report.Execute(RenderType.Pdf, 1);
            return File(result.MainStream, "application/pdf", "BookReport.pdf"); */
            byte[] result = null;
            string fileName = "StaffReport";

            // Kiểm tra định dạng yêu cầu và xuất báo cáo tương ứng
            switch (format?.ToLower())
            {
                case "pdf":
                    result = report.Execute(RenderType.Pdf, 1).MainStream;
                    fileName = "StaffReport.pdf";
                    return File(result, "application/pdf", fileName);

                case "excel":
                    result = report.Execute(RenderType.ExcelOpenXml, 1).MainStream;
                    fileName = "StaffReport.xlsx";
                    return File(result, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);

                case "word":
                    result = report.Execute(RenderType.WordOpenXml, 1).MainStream;
                    fileName = "StaffReport.docx";
                    return File(result, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", fileName);

                default:
                    return BadRequest("Invalid format specified. Supported formats are: PDF, Excel, Word.");
            }
        }
        public IActionResult ReportProduct()
        {
            var Product = _context.Products.ToList();
            var format = Request.Query["format"].ToString();
            DataTable dataTable = new DataTable("dsProduct");
            dataTable.Columns.Add("title", typeof(string));
            dataTable.Columns.Add("price", typeof(double));
            dataTable.Columns.Add("number", typeof(int));
            dataTable.Columns.Add("description", typeof(string));
            foreach (var product in Product)
            {
                dataTable.Rows.Add(
                    product.Title,
                    product.Price,
                    product.Number,
                    product.Description
                );
            }
            string rdlcPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Reports", "ProductRPT.rdlc");
            var report = new LocalReport(rdlcPath);
            report.AddDataSource("DataSet1", dataTable);
            /* var result = report.Execute(RenderType.Pdf, 1);
            return File(result.MainStream, "application/pdf", "BookReport.pdf"); */
            byte[] result = null;
            string fileName = "ProductReport";

            // Kiểm tra định dạng yêu cầu và xuất báo cáo tương ứng
            switch (format?.ToLower())
            {
                case "pdf":
                    result = report.Execute(RenderType.Pdf, 1).MainStream;
                    fileName = "ProductReport.pdf";
                    return File(result, "application/pdf", fileName);

                case "excel":
                    result = report.Execute(RenderType.ExcelOpenXml, 1).MainStream;
                    fileName = "ProductReport.xlsx";
                    return File(result, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);

                case "word":
                    result = report.Execute(RenderType.WordOpenXml, 1).MainStream;
                    fileName = "ProductReport.docx";
                    return File(result, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", fileName);

                default:
                    return BadRequest("Invalid format specified. Supported formats are: PDF, Excel, Word.");
            }
        }
    }
}
