using System;
using System.Collections.Generic;

namespace Web_banThucPhamSach.Data;

public partial class MessageChat
{
    public int Id { get; set; }

    public string? Sender { get; set; }

    public string? Receiver { get; set; }

    public string? MessageContent { get; set; }

    public DateTime? SentAt { get; set; }

    public string? UserId { get; set; }

    public string? AdminId { get; set; }

    public int? ProductId { get; set; }

    public virtual Staff? Admin { get; set; }

    public virtual User? User { get; set; }
}
