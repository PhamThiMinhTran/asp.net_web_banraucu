using System;
using System.Collections.Generic;

namespace Web_banThucPhamSach.Data;

public partial class NewsletterSubscription
{
    public int Id { get; set; }

    public string? UserId { get; set; }

    public string? Email { get; set; }

    public DateTime? SubscribeDate { get; set; }

    public virtual User? User { get; set; }
}
