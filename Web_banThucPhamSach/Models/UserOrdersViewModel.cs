using Microsoft.AspNetCore.Mvc;
using Web_banThucPhamSach.Data;

namespace Web_banThucPhamSach.Models
{
    public class UserOrderViewModel
    {
        public User User { get; set; }
        public List<Order> Orders { get; set; }
    }
}
