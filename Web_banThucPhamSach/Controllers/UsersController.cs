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
    public class UsersController : Controller
    {
        private readonly WebBanThucPhamSachContext _context;

        public UsersController(WebBanThucPhamSachContext context)
        {
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index(int pageIndex = 1, int pageSize = 5)
        {
            var webBanThucPhamSachContext = _context.Users.Include(u => u.Role);
            /*return View(await webBanThucPhamSachContext.ToListAsync());*/
            return View(await PaginatedListViewModel<User>.FromQueryAsync(webBanThucPhamSachContext, pageIndex, pageSize));
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Id");
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Fullname,Email,PhoneNumber,Address,Password,RoleId,CreateAt,UpdateAt,UserName")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Id", user.RoleId);
            return View(user);
        }

        public string GenerateRandomId(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789"; // Có thể thay đổi theo nhu cầu
            Random random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register([Bind("Id,Fullname,Email,PhoneNumber,Address,Password,RoleId,CreateAt,UpdateAt,UserName")] User user)
        {
            user.Id = GenerateRandomId(5);
            user.Fullname = " ";
            user.Email = " ";
            user.RoleId = 2;
            user.Address = " ";
            user.CreateAt = DateTime.Now;
            user.UpdateAt = DateTime.Now;

            ModelState.ClearValidationState(nameof(user.Id));
            ModelState.ClearValidationState(nameof(user.Fullname));
            ModelState.ClearValidationState(nameof(user.Email));
            ModelState.ClearValidationState(nameof(user.RoleId));
            ModelState.ClearValidationState(nameof(user.Address));
            TryValidateModel(user, nameof(user.Id));
            TryValidateModel(user, nameof(user.Fullname));
            TryValidateModel(user, nameof(user.Email));
            TryValidateModel(user, nameof(user.RoleId));
            TryValidateModel(user, nameof(user.Address));
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction("Login", "TrangChu");
            }
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Id", user.RoleId);
            return View("~/Views/TrangChu/Index.cshtml");
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Id", user.RoleId);
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Fullname,Email,PhoneNumber,Address,Password,RoleId,CreateAt,UpdateAt,UserName")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
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
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Id", user.RoleId);
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(string id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
