using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Web_banThucPhamSach.Data;
using Web_banThucPhamSach.Models;

namespace Web_banThucPhamSach.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductsController : Controller
    {
        private readonly WebBanThucPhamSachContext _context;

        public ProductsController(WebBanThucPhamSachContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index(int pageIndex = 1, int pageSize = 5)
        {
            var webBanThucPhamSachContext = _context.Products.Include(p => p.Category);
            //return View(await webBanThucPhamSachContext.ToListAsync());
            return View(await PaginatedListViewModel<Product>.FromQueryAsync(webBanThucPhamSachContext, pageIndex, pageSize));
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        /*public async Task<IActionResult> Create([Bind("Id,CategoryId,Title,Price,Discount,Image,Description,DescriptionDish,CreateAt,UpdateAt,Number")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", product.CategoryId);
            return View(product);
        }*/
        [HttpPost]
        public async Task<IActionResult> Create([Bind("Id,CategoryId,Title,Price,Discount,Description,DescriptionDish,CreateAt,UpdateAt,Number")] Product product, List<IFormFile> Images)
        {
            if (ModelState.IsValid)
            {
                // Danh sách để lưu đường dẫn các hình ảnh
                var imagePaths = new List<string>();
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                if (Images != null && Images.Count > 0)
                {
                    foreach (var image in Images)
                    {
                        var extension = Path.GetExtension(image.FileName).ToLower();
                        if (!allowedExtensions.Contains(extension))
                        {
                            // Bỏ qua tệp nếu không phải định dạng được cho phép
                            ModelState.AddModelError("Image", "Chỉ chấp nhận các định dạng hình ảnh JPG, JPEG, PNG, và GIF.");
                            return View(product);
                        }
                        var fileName = Path.GetFileName(image.FileName);
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/products", fileName);

                        // Lưu từng file vào thư mục
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await image.CopyToAsync(stream);
                        }

                        // Lưu đường dẫn ảnh vào danh sách
                        imagePaths.Add("/images/products/" + fileName);
                    }

                    // Gán các đường dẫn ảnh vào thuộc tính Image, phân cách bằng dấu phẩy
                    product.Image = string.Join(",", imagePaths);
                }

                // Lưu sản phẩm vào cơ sở dữ liệu
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", product.CategoryId);
            return View(product);
        }


        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Edit(string id, List<IFormFile> Images, [Bind("Id,CategoryId,Title,Price,Discount,Image,Description,DescriptionDish,CreateAt,UpdateAt,Number")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Tìm sản phẩm hiện tại từ cơ sở dữ liệu để giữ lại hình ảnh cũ
                    var existingProduct = await _context.Products.FindAsync(product.Id);
                    if (existingProduct == null)
                    {
                        return NotFound();
                    }
                    // Xử lý hình ảnh mới
                    var imagePaths = new List<string>();

                    // Kiểm tra nếu có hình ảnh mới được upload
                    if (Images != null && Images.Count > 0)
                    {
                        string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/products");
                        if (!Directory.Exists(directoryPath))
                        {
                            Directory.CreateDirectory(directoryPath);
                        }

                        foreach (var image in Images)
                        {
                            var fileName = Path.GetFileName(image.FileName);
                            var filePath = Path.Combine(directoryPath, fileName);

                            // Lưu từng file vào thư mục
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await image.CopyToAsync(stream);
                            }

                            // Lưu đường dẫn ảnh vào danh sách
                            imagePaths.Add("/images/products/" + fileName);
                        }
                    }

                    // Nếu không có hình ảnh mới, giữ lại hình ảnh cũ
                    if (imagePaths.Count > 0)
                    {
                        // Gán đường dẫn ảnh mới vào sản phẩm
                        product.Image = string.Join(",", imagePaths);
                    }
                    else
                    {
                        // Giữ lại hình ảnh cũ
                        product.Image = existingProduct.Image;
                    }

                    // Cập nhật thông tin sản phẩm
                    _context.Entry(existingProduct).CurrentValues.SetValues(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
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

            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);
            return View(product);
        }


        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            // Xóa các bản ghi liên quan trong OrderDetails trước
            var orderDetails = _context.OrderDetails.Where(od => od.ProductId == id); // Đảm bảo sử dụng đúng tên khóa ngoại
            if (orderDetails.Any())
            {
                _context.OrderDetails.RemoveRange(orderDetails);
                await _context.SaveChangesAsync(); // Lưu thay đổi sau khi xóa OrderDetails
            }

            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(string id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
