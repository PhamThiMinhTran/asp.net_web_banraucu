using Web_banThucPhamSach.Data;

namespace Web_banThucPhamSach.Models
{
    public class CheckoutViewModel
    {
        public List<Cart> CartItems { get; set; }  // Dữ liệu giỏ hàng
        public User User { get; set; }  // Thông tin người dùng
    }
}
