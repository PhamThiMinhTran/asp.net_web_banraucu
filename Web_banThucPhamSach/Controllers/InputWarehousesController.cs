using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Web_banThucPhamSach.Data;

namespace Web_banThucPhamSach.Controllers
{
    public class InputWarehousesController : Controller
    {
        private readonly WebBanThucPhamSachContext _context;

        public InputWarehousesController(WebBanThucPhamSachContext context)
        {
            _context = context;
        }

        // GET: InputWarehouses
        public async Task<IActionResult> Index()
        {
            var webBanThucPhamSachContext = _context.InputWarehouses.Include(i => i.Product).Include(i => i.Suppliers);
            return View(await webBanThucPhamSachContext.ToListAsync());
        }

        // GET: InputWarehouses/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inputWarehouse = await _context.InputWarehouses
                .Include(i => i.Product)
                .Include(i => i.Suppliers)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (inputWarehouse == null)
            {
                return NotFound();
            }

            return View(inputWarehouse);
        }

        // GET: InputWarehouses/Create
        public IActionResult Create()
        {
            ViewBag.SuppliersId = new SelectList(_context.Suppliers, "Id", "Name");
            return View();
        }
        [HttpGet]
        public IActionResult SearchProducts(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return Json(new { success = false, message = "Từ khóa rỗng!" });
            }

            var products = _context.Products
                .Where(p => p.Title.ToLower().Contains(keyword.ToLower()))
                .Select(p => new
                {
                    Id = p.Id,
                    Title = p.Title,
                    Price = p.Price
                })
                .OrderBy(p => p.Title)
                .Take(10) // Giới hạn kết quả để tăng hiệu suất
                .ToList();

            if (!products.Any())
            {
                return Json(new { success = false, message = "Không tìm thấy sản phẩm nào!" });
            }

            return Json(new { success = true, data = products });
        }


        // POST: InputWarehouses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // POST: InputWarehouses/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(InputWarehouseViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Dữ liệu không hợp lệ.");
                ViewData["SuppliersId"] = new SelectList(_context.Suppliers, "Id", "Name", model.SuppliersId);
                return View(model);
            }

            // Kiểm tra danh sách sản phẩm
            if (model.Products == null || !model.Products.Any(p => p.NumberInput > 0))
            {
                ModelState.AddModelError("", "Vui lòng nhập số lượng hợp lệ cho ít nhất một sản phẩm.");
                ViewData["SuppliersId"] = new SelectList(_context.Suppliers, "Id", "Name", model.SuppliersId);
                return View(model);
            }

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    double totalOrderValue = 0;

                    foreach (var productInput in model.Products.Where(p => p.NumberInput > 0))
                    {
                        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == productInput.ProductId);
                        if (product != null)
                        {
                            product.Number += productInput.NumberInput;

                            var inputWarehouse = new InputWarehouse
                            {
                                Id = Guid.NewGuid().ToString().Substring(0, 5),
                                ProductId = productInput.ProductId,
                                SuppliersId = model.SuppliersId,
                                NameProduct = product.Title, // Lấy từ Product.Title
                                NumberInput = productInput.NumberInput,
                                Total = productInput.NumberInput * product.Price, // Giá từ Product
                                DateIn = model.DateIn
                            };

                            totalOrderValue += inputWarehouse.Total ?? 0;

                            _context.InputWarehouses.Add(inputWarehouse);
                        }
                    }

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    TempData["SuccessMessage"] = $"Nhập kho thành công! Tổng giá trị đơn hàng: {totalOrderValue:C}";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    ModelState.AddModelError("", "Lỗi khi nhập kho: " + ex.Message);
                    ViewData["SuppliersId"] = new SelectList(_context.Suppliers, "Id", "Name", model.SuppliersId);
                    return View(model);
                }
            }
        }


        // GET: InputWarehouses/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inputWarehouse = await _context.InputWarehouses.FindAsync(id);
            if (inputWarehouse == null)
            {
                return NotFound();
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Id", inputWarehouse.ProductId);
            ViewData["SuppliersId"] = new SelectList(_context.Suppliers, "Id", "Id", inputWarehouse.SuppliersId);
            return View(inputWarehouse);
        }

        // POST: InputWarehouses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,ProductId,SuppliersId,NameProduct,NumberInput,Total,DateIn")] InputWarehouse inputWarehouse)
        {
            if (id != inputWarehouse.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(inputWarehouse);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InputWarehouseExists(inputWarehouse.Id))
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
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Id", inputWarehouse.ProductId);
            ViewData["SuppliersId"] = new SelectList(_context.Suppliers, "Id", "Id", inputWarehouse.SuppliersId);
            return View(inputWarehouse);
        }

        // GET: InputWarehouses/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inputWarehouse = await _context.InputWarehouses
                .Include(i => i.Product)
                .Include(i => i.Suppliers)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (inputWarehouse == null)
            {
                return NotFound();
            }

            return View(inputWarehouse);
        }

        // POST: InputWarehouses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var inputWarehouse = await _context.InputWarehouses.FindAsync(id);
            if (inputWarehouse != null)
            {
                _context.InputWarehouses.Remove(inputWarehouse);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InputWarehouseExists(string id)
        {
            return _context.InputWarehouses.Any(e => e.Id == id);
        }
    }
}
