using Microsoft.AspNetCore.Http;
using System.Text.Json;
namespace Web_banThucPhamSach.Helpers
{
    public  static class SessionHelper
    {
        // Lưu object vào Session dưới dạng JSON
        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }

        // Lấy object từ Session bằng cách deserialize JSON
        public static T GetObjectFromJson<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : JsonSerializer.Deserialize<T>(value);
        }
    }
}
