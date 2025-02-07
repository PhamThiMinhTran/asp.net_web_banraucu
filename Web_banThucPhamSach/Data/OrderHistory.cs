using System;
using System.Collections.Generic;

namespace Web_banThucPhamSach.Data;

public partial class OrderHistory
{
    public string Id { get; set; } = null!;

    public string? OrderId { get; set; }

    public string? Status { get; set; }

    public DateTime? ChangeDate { get; set; }

    public virtual Order? Order { get; set; }
}
