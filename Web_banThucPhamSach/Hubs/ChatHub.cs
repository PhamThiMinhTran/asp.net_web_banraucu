using Microsoft.AspNetCore.SignalR;

namespace Web_banThucPhamSach.Hubs
{
    public class ChatHub : Hub
    {
        private static bool _adminOnline = false;

        // Gửi tin nhắn từ khách hàng đến admin và ngược lại trong nhóm riêng của khách hàng
        public async Task SendMessage(string userId, string message)
        {
            // Gửi tin nhắn đến nhóm của khách hàng, bao gồm admin
            await Clients.Group(userId).SendAsync("ReceiveMessage", userId, message);
        }

        // Khi khách hàng tham gia nhóm chat riêng của họ
        public async Task JoinCustomerGroup(string userId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, userId);
            Console.WriteLine($"User {userId} đã tham gia nhóm.");
        }

        // Khi admin tham gia nhóm chat của khách hàng
        public async Task JoinAdminToCustomerGroup(string customerId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, customerId);
            Console.WriteLine($"Admin joined customer group: {customerId}");
        }

        // Xử lý khi kết nối được thiết lập
        public override async Task OnConnectedAsync()
        {
            if (Context.User.IsInRole("Admin"))
                _adminOnline = true;

            await base.OnConnectedAsync();
        }

        // Xử lý khi kết nối bị ngắt
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            if (Context.User.IsInRole("Admin"))
                _adminOnline = false;

            await base.OnDisconnectedAsync(exception);
        }

        // Kiểm tra trạng thái online của admin
        public async Task<bool> CheckAdminOnlineStatus()
        {
            return _adminOnline;
        }
    }
}
