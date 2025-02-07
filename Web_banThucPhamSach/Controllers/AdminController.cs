using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using System.Diagnostics;
using Web_banThucPhamSach.Data;
using Web_banThucPhamSach.Hubs;

namespace Web_banThucPhamSach.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : BaseController
    {
        private readonly WebBanThucPhamSachContext _context;
        private readonly IHubContext<ChatHub> _hubContext;

        public AdminController(IHubContext<ChatHub> hubContext, WebBanThucPhamSachContext context)
        {
            _hubContext = hubContext;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Setting()
        {
            var theme = HttpContext.Session.GetString("theme") ?? "BinhThuong";
            ViewData["theme"] = theme;
            return View();
        }

        [HttpPost]
        public IActionResult ChangeTheme([FromBody] Dictionary<string, string> themeData)
        {
            if (themeData?.ContainsKey("theme") != true)
            {
                Debug.WriteLine("Theme is invalid or null");
                return BadRequest("Theme is invalid");
            }

            string theme = themeData["theme"];
            HttpContext.Session.SetString("theme", theme);
            Debug.WriteLine("Theme saved to session: " + theme);
            Response.Cookies.Append("theme", theme, new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddMinutes(30),
            });

            return Ok();
        }

        public IActionResult FunctionChat()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ThongKe(int month = 0, int quarter = 0)
        {
            // Khởi tạo các biến
            var year = DateTime.Now.Year;
            double totalRevenue = 0;
            double totalImportCost = 0;

            // Lọc tháng hoặc quý
            var orderQuery = _context.Orders.AsQueryable();
            var warehouseQuery = _context.InputWarehouses.AsQueryable();

            if (month > 0)
            {
                // Lọc theo tháng
                orderQuery = orderQuery.Where(o => o.OrderDate.Value.Month == month && o.OrderDate.Value.Year == year);
                warehouseQuery = warehouseQuery.Where(w => w.DateIn.Value.Month == month && w.DateIn.Value.Year == year);
            }
            else if (quarter > 0)
            {
                // Lọc theo quý
                int startMonth = (quarter - 1) * 3 + 1; // Tháng bắt đầu của quý
                int endMonth = startMonth + 2;         // Tháng kết thúc của quý
                orderQuery = orderQuery.Where(o => o.OrderDate.Value.Month >= startMonth &&
                                                   o.OrderDate.Value.Month <= endMonth &&
                                                   o.OrderDate.Value.Year == year);
                warehouseQuery = warehouseQuery.Where(w => w.DateIn.Value.Month >= startMonth &&
                                                           w.DateIn.Value.Month <= endMonth &&
                                                           w.DateIn.Value.Year == year);
            }
            else
            {
                // Tất cả dữ liệu trong năm
                orderQuery = orderQuery.Where(o => o.OrderDate.Value.Year == year);
                warehouseQuery = warehouseQuery.Where(w => w.DateIn.Value.Year == year);
            }
            // Tính tổng doanh thu (từ Orders)
            totalRevenue = orderQuery.Sum(o => o.TotalMoney ?? 0);

            // Tính tổng chi phí nhập hàng (từ InputWarehouses)
            totalImportCost = warehouseQuery.Sum(w => w.Total ?? 0);

            // Tính lợi nhuận
            var profit = totalRevenue - totalImportCost;

            // Tạo model
            var model = new ReportViewModel
            {
                Year = year,
                Month = month,
                Quarter = quarter,
                TotalRevenue = totalRevenue,
                TotalImportCost = totalImportCost,
                Profit = profit
            };

            return View(model);
        }
        
        public async Task<IActionResult> Review()
        {
            var reviews = await _context.Reviews
                .Include(r => r.Product)
                .Include(r => r.User)
                .ToListAsync();

            return View(reviews);
        }
    }
}
