using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Globalization;
using System.Security.Claims;
using Web_banThucPhamSach.Data;
using Web_banThucPhamSach.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Web_banThucPhamSach.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Net;

namespace Web_banThucPhamSach.Controllers
{
    public class FunctionController : Controller
    {
        private WebBanThucPhamSachContext _context;
        public FunctionController(WebBanThucPhamSachContext context)
        {
            _context = context;
        }
        public IActionResult Cart()
        {
            var user = User.FindFirst("Id")?.Value;
            if (user == null)
            {
                return RedirectToAction("Login", "TrangChu");
            }
            var cart = GetCartItems();
            ViewBag.TotalQuantity = TotalQuantityInCart();
            return View(cart);
        }
        [Route("addCart/{productId?}", Name = "addcart")]
        public IActionResult AddToCart([FromRoute] string productId)
        {

            var product = _context.Products
                .AsNoTracking()
                .Where(c => c.Id == productId)
                .FirstOrDefault();
            if (product == null)
            {
                return NotFound("Không tìm thấy sản phẩm");
            }
            var cart = GetCartItems();
            var cartitem = cart.Find(p => p.ProductId == productId);
            if (cartitem != null)
            {
                // Đã tồn tại, tăng thêm 1
                cartitem.Quantity++;
                cartitem.AllTotal = cartitem.Quantity * (cartitem.Product?.Price ?? 0);
            }
            else
            {
                //  Thêm mới
                //cart.Add(new Cart() { ProductId = productId, Quantity = 1, Product = product });
                cart.Add(new Cart()
                {
                    ProductId = productId,
                    Quantity = 1,
                    Product = product,
                    AllTotal = product.Price // Khởi tạo tổng tiền với số lượng 1
                });
            }

            // Lưu cart vào Session
            SaveCartSession(cart);
            ViewBag.QuantityCart = TotalQuantityInCart();
            // Chuyển đến trang hiện thị Cart
            return RedirectToAction(nameof(Cart));
        }

        public const string Cartkey = "cart";
        List<Cart> GetCartItems()
        {
            var session = HttpContext.Session;
            string jsoncart = session.GetString(Cartkey);
            if (jsoncart != null)
            {
                try
                {
                    return JsonConvert.DeserializeObject<List<Cart>>(jsoncart);
                }
                catch (JsonException)
                {
                    // Xử lý lỗi khi đọc dữ liệu JSON
                    session.Remove(Cartkey); // Xóa giỏ hàng không hợp lệ
                    return new List<Cart>();
                }
            }
            return new List<Cart>();
        }
        void SaveCartSession(List<Cart> ls)
        {
            var session = HttpContext.Session;
            string jsoncart = JsonConvert.SerializeObject(ls);
            session.SetString(Cartkey, jsoncart);
        }

        [Route("/RemoveCart/{productId}", Name = "removecart")]
        public IActionResult RemoveCart([FromRoute] string productId)
        {
            var cart = GetCartItems();
            var cartItem = cart.SingleOrDefault(x => x.ProductId == productId);
            if (cartItem != null)
            {
                cart.Remove(cartItem);
            }
            SaveCartSession(cart);  // Lưu giỏ hàng sau khi xóa
            return RedirectToAction(nameof(Cart));
        }
        [Route("/updatecart", Name = "updatecart")]
        [HttpPost]
        public IActionResult UpdateCart([FromForm] string productId, [FromForm] int quantity)
        {
            if (quantity < 1)
            {
                return Json(new { success = false, message = "Số lượng phải là số nguyên dương." });
            }
            var cart = GetCartItems();
            var cartItem = cart.SingleOrDefault(x => x.ProductId == productId);

            if (cartItem != null)
            {
                // Cập nhật số lượng và tổng tiền cho sản phẩm
                cartItem.Quantity = quantity;
                cartItem.AllTotal = cartItem.Quantity * (cartItem.Product?.Price ?? 0);
            }

            // Lưu lại giỏ hàng vào session sau khi cập nhật
            SaveCartSession(cart);

            // Tính tổng tiền cho tất cả sản phẩm trong giỏ
            var cartTotal = cart.Sum(item => item.Quantity * (item.Product?.Price ?? 0));

            // Trả về JSON để cập nhật UI
            return Json(new
            {
                success = true,
                total = cartItem.AllTotal,
                cartTotal = cartTotal,
                cartItems = cart.Select(item => new
                {
                    productId = item.ProductId,
                    quantity = item.Quantity,
                    product = item.Product
                }).ToList()
            });
        }
        public int TotalQuantityInCart()
        {
            var cart = GetCartItems();
            return cart.Sum(c => c.Quantity);
        }
        public JsonResult BagCart()
        {
            int total_item = TotalQuantityInCart();
            return Json(new { quantity = total_item });
        }
        [HttpPost]
        public IActionResult ThanhToan()
        {
            var cartItems = GetCartItems(); // Giả sử phương thức này lấy danh sách sản phẩm trong giỏ hàng
            if (cartItems == null || !cartItems.Any())
            {
                return View("Error", "Giỏ hàng trống.");
            }
            // Lấy thông tin người dùng hiện tại từ Claims
            var userId = User.FindFirst("Id")?.Value;
            if (userId == null)
            {
                return NotFound("Người dùng không xác định.");
            }
            var user = _context.Users
        .Include(u => u.BankAccounts) // Bao gồm danh sách tài khoản ngân hàng
        .FirstOrDefault(u => u.Id == userId);

            if (user == null)
            {
                return NotFound("Người dùng không tồn tại.");
            }

            var viewModel = new CheckoutViewModel
            {
                CartItems = cartItems,
                User = user
            };

            return View(viewModel); // Trả về view với CheckoutViewModel
        }
        public string GenerateRandomId(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789"; // Có thể thay đổi theo nhu cầu
            Random random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        [HttpPost]
        public IActionResult CreateOrder([Bind("PaymentMethod")] Order order, string note)
        {
            try
            {
                var cart = GetCartItems();
                if (cart == null || !cart.Any())
                {
                    ModelState.AddModelError("", "Giỏ hàng trống. Vui lòng thêm sản phẩm.");
                    return RedirectToAction(nameof(Cart));
                }

                order.Id = GenerateRandomId(5);
                order.Fullname = User.Identity.Name;
                order.PhoneNumber = User.FindFirst("PhoneNumber")?.Value;
                order.Address = User.FindFirst("Address")?.Value;
                order.TotalMoney = cart.Sum(item => item.Quantity * item.Product.Price);
                order.OrderDate = DateTime.Now;
                order.Note = note;
                order.Status = 0; // Đơn hàng mới

                // Lấy phương thức thanh toán từ form và xử lý
                var paymentMethod = Request.Form["PaymentMethod"];
                if (string.IsNullOrEmpty(paymentMethod))
                {
                    ModelState.AddModelError("", "Vui lòng chọn phương thức thanh toán.");
                    return View();
                }
                order.PaymentMethod = paymentMethod;

                // Thiết lập UserId cho đơn hàng
                var userId = User.FindFirst("Id")?.Value;
                if (userId == null)
                {
                    return NotFound("Không tìm thấy ID người dùng trong claims");
                }
                order.UserId = userId;

                _context.Orders.Add(order);

                foreach (var item in cart)
                {
                    var orderDetail = new OrderDetail
                    {
                        Id = GenerateRandomId(5),
                        OrderId = order.Id,
                        ProductId = item.Product.Id,
                        Price = item.Product.Price,
                        NumberProducts = item.Quantity,
                        TotalMoney = item.Quantity * item.Product.Price
                    };
                    _context.OrderDetails.Add(orderDetail);

                    var product = _context.Products.FirstOrDefault(p => p.Id == item.Product.Id);
                    if (product != null)
                    {
                        product.Number -= item.Quantity; // Giảm số lượng sản phẩm
                        if (product.Number < 0)
                        {
                            ModelState.AddModelError("", "Sản phẩm " + product.Title +" không đủ số lượng trong kho.");
                            return View("ThanhToan"); // Trả về trang hiện tại nếu sản phẩm không đủ
                        }
                    }
                }

                _context.SaveChanges();
                ClearCartAction();
                return RedirectToAction("Success");
            }
            catch (Exception ex)
            {
                var innerException = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return Content("Lỗi xảy ra: " + innerException);
            }
        }

        // Xóa cart khỏi session
        private void ClearCart()
        {
            var session = HttpContext.Session;
            session.Remove(Cartkey);  // Xóa giỏ hàng khỏi session
            Console.WriteLine("Giỏ hàng đã được xóa khỏi session.");
        }
        public IActionResult ClearCartAction()
        {
            ClearCart();  // Gọi hàm xóa giỏ hàng
            return RedirectToAction(nameof(Cart));  // Điều hướng về trang giỏ hàng (hoặc trang khác tùy ý)
        }
        public IActionResult Success()
        {
            return View();
        }

        /*--------------------------------------------------*/

        public IActionResult Profile(string Id)
        {
            var userId = User.FindFirst("Id")?.Value;
            if (userId == null)
            {
                return NotFound("Không tìm thấy ID người dùng trong claims");
            }
            var user = _context.Users.FirstOrDefault(x => x.Id == userId);
            if (user == null)
            {
                return NotFound();
            }

            var orders = _context.Orders.Where(o => o.UserId == Id).ToList();
            var viewModel = new UserOrderViewModel
            {
                User = user,
                Orders = orders
            };
            return View(viewModel);
        }
        [HttpPost]
        [Authorize]
        public IActionResult EditProfile([Bind("Id,Fullname,UserName,Email,PhoneNumber,Address,Password,CreateAt,UpdateAt")] User updatedUser)
        {
            var userId = User.FindFirst("Id")?.Value;
            if (userId == null)
            {
                return NotFound("Không tìm thấy ID người dùng trong claims");
            }
            var user = _context.Users.FirstOrDefault(x => x.Id == userId);
            if (user == null)
            {
                return NotFound();
            }
            updatedUser.Email = string.IsNullOrWhiteSpace(updatedUser.Email) ? "" : updatedUser.Email;
            // Kiểm tra xem dữ liệu form có hợp lệ không
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Dữ liệu nhập vào không hợp lệ!";
                // Nếu không hợp lệ, trả lại View cùng với dữ liệu đã nhập
                return View(updatedUser);
            }
            user.Fullname = updatedUser.Fullname;
            user.UserName = updatedUser.UserName;
            user.Email = updatedUser.Email;
            user.PhoneNumber = updatedUser.PhoneNumber;
            user.Address = updatedUser.Address;
            if (!string.IsNullOrEmpty(updatedUser.Password))
            {
                // Bạn nên mã hóa mật khẩu trước khi lưu
                user.Password = updatedUser.Password; // Mã hóa trước khi lưu vào DB
            }

            // Cập nhật thời gian sửa đổi cuối cùng
            user.UpdateAt = DateTime.Now;

            // Lưu thay đổi vào cơ sở dữ liệu
            _context.SaveChanges();
            //TempData["SuccessMessage"] = "Cập nhật thông tin người dùng thành công!";
            // Chuyển hướng người dùng về trang Profile sau khi lưu
            return RedirectToAction("Profile", new { Id = user.Id });
        }

        private bool UserExists(string id)
        {
            return _context.Users.Any(e => e.Id == id);
        }

        public IActionResult TT_Product(string Id)
        {
            var product = _context.Products.First(x => x.Id == Id);

            if (product == null)
            {
                return NotFound(); // Nếu không tìm thấy sản phẩm
            }
            return View(product);
        }
        public IActionResult AllOrderUser()
        {
            // Lấy ID người dùng từ claims
            var userId = User.FindFirst("Id")?.Value;
            if (userId == null)
            {
                return NotFound("Không tìm thấy ID người dùng trong claims");
            }

            // Tìm người dùng trong cơ sở dữ liệu
            var user = _context.Users.FirstOrDefault(x => x.Id == userId);
            if (user == null)
            {
                return NotFound();
            }
            var userOrders = _context.Orders
                                      .Where(o => o.UserId == userId)
                                      .Include(o => o.OrderDetails)
                                      .ThenInclude(od => od.Product)
                                      .ToList();
            if (!userOrders.Any())
            {
                // Ghi log hoặc debug để kiểm tra
                Console.WriteLine($"Không có đơn hàng nào cho userId: {userId}");
            }
            Console.WriteLine($"Số lượng đơn hàng: {userOrders.Count}");
            var viewModel = new UserOrderViewModel
            {
                User = user,
                Orders = userOrders
            };

            return View(viewModel);
        }
        /*Xóa đơn hàng từ khách hàng*/
        [HttpPost]
        public IActionResult DeleteOrder(string orderId)
        {
            var userId = User.FindFirst("Id")?.Value;  // Lấy ID người dùng từ claims
            if (userId == null)
            {
                return Json(new { success = false, message = "Không tìm thấy ID người dùng." });
            }

            var order = _context.Orders.Include(o => o.OrderDetails).FirstOrDefault(o => o.Id == orderId && o.UserId == userId);

            if (order == null || order.Status != 0)  // Kiểm tra trạng thái đơn hàng (chỉ cho phép xóa đơn hàng đang xử lý)
            {
                return Json(new { success = false, message = "Đơn hàng không tồn tại hoặc không thể hủy." });
            }

            // Xóa chi tiết đơn hàng
            _context.OrderDetails.RemoveRange(order.OrderDetails);

            // Xóa đơn hàng
            _context.Orders.Remove(order);
            _context.SaveChanges();

            return Json(new { success = true, message = "Đơn hàng đã được hủy thành công." });
        }

        [HttpPost]
        public IActionResult ConfirmReceipt(string orderId)
        {
            var userId = User.FindFirst("Id")?.Value;
            var order = _context.Orders.FirstOrDefault(o => o.Id == orderId && o.UserId == userId);

            if (order == null)
            {
                return Json(new { success = false, message = "Đơn hàng không tồn tại." });
            }

            order.Status = 2; // Đặt trạng thái thành "Hoàn thành"
            _context.SaveChanges();

            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> AddReview([FromBody] Review review)
        {
            try
            {
                var userId = User.FindFirst("Id")?.Value;
                if (userId == null)
                {
                    return NotFound("Không tìm thấy ID người dùng trong claims");
                }
                if (string.IsNullOrEmpty(review.ProductId))
                {
                    return BadRequest(new { success = false, message = "ProductId không hợp lệ." });
                }
                review.Id = GenerateRandomId(5);
                review.UserId = userId; // Gán UserId từ Claims
                review.ProductId = review.ProductId;
                review.CreateAt = DateTime.Now;
                review.UpdateAt = DateTime.Now;
                if (ModelState.IsValid)
                {
                    _context.Reviews.Add(review);
                    await _context.SaveChangesAsync();

                    return Ok(new { success = true, message = "Đánh giá đã được gửi thành công." });
                }
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return BadRequest(new { success = false, message = "Dữ liệu không hợp lệ.", errors });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Outer Exception: " + ex.Message);
                if (ex.InnerException != null)
                {
                    Console.WriteLine("Inner Exception: " + ex.InnerException.Message);
                    Console.WriteLine("Inner Stack Trace: " + ex.InnerException.StackTrace);
                }
                return StatusCode(500, new { success = false, message = "Lỗi hệ thống: " + ex.Message });
            }
        }
        public async Task<IActionResult> LienKetNganHangAsync()
        {
            var userId = User.FindFirst("Id")?.Value;
            if (userId == null)
            {
                return NotFound("Không tìm thấy ID người dùng trong claims");
            }

            // Lấy danh sách tài khoản của người dùng từ cơ sở dữ liệu
            var accounts = await _context.BankAccounts
                .Where(b => b.UserId == userId)
                .ToListAsync();

            // Truyền danh sách tài khoản vào Model hoặc ViewBag
            return View(accounts);
        }
        [HttpPost]
        public async Task<IActionResult> LienKetNganHang([Bind("UserId, AccountName, AccountNumber, BankName, AccountType")] BankAccount bankAccount)
        {
            var userId = User.FindFirst("Id")?.Value;
            if (userId == null)
            {
                return NotFound("Không tìm thấy ID người dùng trong claims");
            }

            bankAccount.UserId = userId;
            ModelState.Remove("UserId");
            if (!ModelState.IsValid)
            {
                return View(bankAccount);
            }

            bankAccount.CreatedAt = DateTime.Now;
            bankAccount.UpdatedAt = DateTime.Now;

            _context.BankAccounts.Add(bankAccount);
            await _context.SaveChangesAsync();

            return RedirectToAction("Success");
        }
    }
}
