using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Web_banThucPhamSach.Data;
using Web_banThucPhamSach.Models;
using Web_banThucPhamSach.Helpers;

namespace Web_banThucPhamSach.Controllers
{
    public class TrangChuController : BaseController
    {
        private WebBanThucPhamSachContext _context;
        public TrangChuController(WebBanThucPhamSachContext context)
        {
            _context = context;
        }
        public IActionResult Index(string search = null)
        {
            /*var dsProduct = _context.Products.ToList();
            ViewBag.dsProduct = dsProduct.Take(9);
            return View();*/
            // Lấy tất cả sản phẩm nếu không có tìm kiếm
            var products = string.IsNullOrEmpty(search)
                ? _context.Products.Take(9).ToList()
                : _context.Products
                    .Where(p => p.Title.ToLower().Contains(search.ToLower()))
                    .ToList();

            // Truyền sản phẩm đến view
            return View(products);
        }
        public IActionResult About()
        {
            return View();
        }
        public async Task<IActionResult> Category(string category, int pageIndex = 1, int pageSize = 12)
        {
            if (string.IsNullOrEmpty(category))
            {
                return RedirectToAction("Index");
            }

            // Truy vấn sản phẩm theo danh mục
            var productsQuery = _context.Products
                .Where(p => p.Category.Name == category);

            // Tạo đối tượng phân trang
            var paginatedProducts = await PaginatedListViewModel<Product>.FromQueryAsync(productsQuery, pageIndex, pageSize);

            // Truyền thêm dữ liệu qua ViewBag
            ViewBag.dsAllCategories = _context.Categories.Take(12).ToList();
            ViewBag.CategoryName = category;

            // Trả về View với đúng kiểu dữ liệu
            return View(paginatedProducts);
        }

        public async Task<IActionResult> CategoryList(int pageIndex = 1, int pageSize = 15)
        {
            var totalProducts = _context.Products.Count(); // Tổng số sản phẩm
            var dsCategory = await _context.Products
                                           .Skip((pageIndex - 1) * pageSize)
                                           .Take(pageSize)
                                           .ToListAsync();

            var paginatedProducts = new PaginatedListViewModel<Product>(dsCategory, totalProducts, pageIndex, pageSize);

            return View("Category", paginatedProducts);
        }

        public IActionResult Search(string search, string category = null)
        {
            if (string.IsNullOrEmpty(search))
            {
                return RedirectToAction("Index");
            }

            var searchResults = _context.Products
                .Where(p => p.Title.ToLower().Contains(search.ToLower()))
                .ToList();

            // Kiểm tra xem có danh mục nào không
            if (!string.IsNullOrEmpty(category))
            {
                // Nếu có danh mục, chỉ trả về sản phẩm trong danh mục đó
                searchResults = searchResults.Where(p => p.Category.Name == category).ToList();
            }

            ViewBag.SearchResults = searchResults;
            ViewBag.CategoryName = category; // Cung cấp tên danh mục cho view

            return View("Category"); // Trả về view Category
        }


        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            var user = _context.Users.FirstOrDefault(n => n.UserName == username);
            if (user != null && VerifyPassword(user.Password, password))
            {
                var role = _context.Roles.FirstOrDefault(r => r.Id == user.RoleId);
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Role, role?.Name ?? "User"), 
                    new Claim("Id", user.Id),
                    new Claim("PhoneNumber", user.PhoneNumber), 
                    new Claim("Address", user.Address)   
                };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                await HttpContext.SignInAsync(new ClaimsPrincipal(claimsIdentity));

                // Chuyển hướng người dùng
                if (Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return user.RoleId switch
                    {
                        1 => RedirectToAction("Index", "Admin"),
                        2 => RedirectToAction("Index", "TrangChu"),
                        _ => RedirectToAction("Login", "TrangChu")
                    };
                }
            }
            /*ViewData["ErrorMessage"] = "Invalid usrename or password or Address";
            return View();*/
            ModelState.AddModelError(string.Empty, "Tên người dùng hoặc mật khẩu sai kìa");
            return View();
        }
        private bool VerifyPassword(string storedPassword, string inputPassword)
        {
            // Nếu bạn sử dụng mã hóa, kiểm tra mật khẩu ở đây
            return storedPassword == inputPassword; // Chỉ dùng cho mục đích thử nghiệm, không an toàn
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "TrangChu");
        }
        [HttpPost]
        public IActionResult Sendmessage (string messageContent)
        {
            if (string.IsNullOrEmpty(messageContent))
            {
                TempData["Error"] = "Tin nhắn không được để trống!";
                return RedirectToAction("Index");
            }

            // Lấy danh sách tin nhắn từ Session
            var messages = HttpContext.Session.GetObjectFromJson<List<MessageChat>>("CustomerMessages") ?? new List<MessageChat>();

            // Tạo tin nhắn mới
            var newMessage = new MessageChat
            {
                Sender = "Customer",
                Receiver = "Admin",
                MessageContent = messageContent,
                SentAt = DateTime.Now
            };

            // Thêm tin nhắn mới vào danh sách
            messages.Add(newMessage);

            // Lưu lại vào Session
            HttpContext.Session.SetObjectAsJson("CustomerMessages", messages);

            return RedirectToAction("Index");
        }
    }
}