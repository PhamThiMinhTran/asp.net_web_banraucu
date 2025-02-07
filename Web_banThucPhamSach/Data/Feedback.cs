using System;
using System.Collections.Generic;

namespace Web_banThucPhamSach.Data;

public partial class Feedback
{
    public string Id { get; set; } = null!;

    public string? UserId { get; set; }

    public string? Fullname { get; set; }

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Subject { get; set; }

    public string? Message { get; set; }

    public string? Type { get; set; }

    public int? Status { get; set; }

    public DateTime? CreateAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public virtual User? User { get; set; }
}
