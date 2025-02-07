using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Web_banThucPhamSach.Data;
using Web_banThucPhamSach.Models;

namespace Web_banThucPhamSach.Controllers
{
    [Authorize(Roles = "Admin")]
    public class OrdersController : Controller
    {
        private readonly WebBanThucPhamSachContext _context;

        public OrdersController(WebBanThucPhamSachContext context)
        {
            _context = context;
        }
        public IActionResult OrderList(string title)
        {
            var orders = _context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .AsQueryable();

            if (!string.IsNullOrEmpty(title))
            {
                orders = orders.Where(o => o.OrderDetails.Any(od => od.Product.Title == title));
            }


            return View(orders.ToList());
        }


        // GET: Orders
        public async Task<IActionResult> Index(string accountType, string paymentMethod,string status, int pageIndex = 1, int pageSize = 5)
        {
            var webBanThucPhamSachContext = _context.Orders
        .Include(o => o.OrderDetails)
        .ThenInclude(od => od.Product)
        .Include(o => o.User)
        .ThenInclude(u => u.BankAccounts)
        .AsQueryable();

            // Lọc theo AccountType từ BankAccount
            if (!string.IsNullOrEmpty(accountType) && accountType != "All")
            {
                webBanThucPhamSachContext = webBanThucPhamSachContext
                    .Where(o => o.User.BankAccounts.Any(b => b.AccountType == accountType));
            }

            // Lọc đơn hàng theo hình thức thanh toán nếu có
            if (!string.IsNullOrEmpty(paymentMethod) && paymentMethod != "All")
            {
                webBanThucPhamSachContext = webBanThucPhamSachContext.Where(o => o.PaymentMethod == paymentMethod);
            }
            // Lọc theo trạng thái đơn hàng
            if (!string.IsNullOrEmpty(status) && status != "All")
            {
                if (status == "Pending")
                {
                    webBanThucPhamSachContext = webBanThucPhamSachContext.Where(o => o.Status == 0); // Giả định 0 là 'chưa duyệt'
                }
                else if (status == "Approved")
                {
                    webBanThucPhamSachContext = webBanThucPhamSachContext.Where(o => o.Status == 1); // Giả định 1 là 'đã duyệt'
                }
                else if (status == "Completed")
                {
                    webBanThucPhamSachContext = webBanThucPhamSachContext.Where(o => o.Status == 2); // Hoàn thành
                }
            }
            /*webBanThucPhamSachContext = webBanThucPhamSachContext.Include(o => o.OrderDetails);*/
            /*return View(await _context.Orders.ToListAsync());*/
            return View(await PaginatedListViewModel<Order>.FromQueryAsync(webBanThucPhamSachContext, pageIndex, pageSize));
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            /*var order = await _context.Orders
                .FirstOrDefaultAsync(m => m.Id == id);*/
            var order = await _context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Fullname,Email,PhoneNumber,Address,Note,OrderDate,Status,TotalMoney,PaymentMethod")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Fullname,Email,PhoneNumber,Address,Note,OrderDate,Status,TotalMoney,PaymentMethod")] Order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }



        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            //var order = await _context.Orders.FindAsync(id);
            var order = await _context.Orders.Include(o => o.OrderDetails).FirstOrDefaultAsync(o => o.Id == id);
            if (order != null)
            {
                _context.OrderDetails.RemoveRange(order.OrderDetails);  // Xóa tất cả các chi tiết đơn hàng liên quan
                _context.Orders.Remove(order);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(string id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }

        [HttpPost]
        public IActionResult ApproveOrder(string id)
        {
            var order = _context.Orders.FirstOrDefault(o => o.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            order.Status = 1; // Giả định trạng thái '1' là đã đóng gói hoặc duyệt đơn thành công
            _context.SaveChanges();

            return RedirectToAction(nameof(Index)); // Trở về trang quản lý đơn hàng
        }
        [HttpPost]
        public IActionResult ConfirmReceipt(string orderId)
        {
            var userId = User.FindFirst("Id")?.Value;
            if (userId == null)
            {
                return Json(new { success = false, message = "Không tìm thấy ID người dùng." });
            }

            var order = _context.Orders.FirstOrDefault(o => o.Id == orderId && o.UserId == userId);

            if (order == null || order.Status != 1)  // Kiểm tra trạng thái đơn hàng (chỉ cho phép hoàn thành đơn hàng đang xử lý)
            {
                return Json(new { success = false, message = "Đơn hàng không tồn tại hoặc không thể xác nhận." });
            }

            order.Status = 2;  // Đặt trạng thái thành "Hoàn thành"
            _context.SaveChanges();

            return Json(new { success = true, message = "Đơn hàng đã được xác nhận hoàn thành." });
        }
    }
}
