using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Web_banThucPhamSach.Controllers
{
    public class BaseController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // Lấy theme từ Session hoặc Cookie
            string theme = HttpContext.Session.GetString("theme") ?? Request.Cookies["theme"] ?? "BinhThuong";
            ViewData["theme"] = theme;

            base.OnActionExecuting(context);
        }
    }
}
